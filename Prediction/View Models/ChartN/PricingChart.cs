using Prediction.Models;
using Prediction.Models.Enums;
using Prediction.Models.Hardware;
using Prediction.Models.NewChart;
using Prediction.Models.Time_Series_Forecasting;
using Prediction.Models.Time_Series_Forecasting.Cleaning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction.View_Models.ChartN
{
    public class PricingChart
    {
        private readonly List<Item> m_phones;
        public List<PhoneProperties> Hardware { get; set; }
        public List<int> SelectedPhones { get; set; }
        public int FutureForecastMonths { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<ChartItem> Charts { get; set; }
        public List<Dictionary<int, List<ChartTransaction>>> ModelPrices { get; set; }

        public PricingChart(List<Item> phones, List<PhoneProperties> hardware, int months = 12, List<int> selectedPhones = null)
        {
            this.m_phones = phones;
            this.Hardware = hardware;
            this.SelectedPhones = RemoveUnforecastableIds(selectedPhones.Distinct().ToList());
            this.FutureForecastMonths = months;
        }

        public List<ChartItem> DrawChart(List<int> hardwareId)
        {
            List<ChartItem> allCharts = new List<ChartItem>();

            if (hardwareId is null)
            {
                throw new ArgumentNullException(null);
            }

            // Itterates through all passed id
            for (int i = 0; i < hardwareId.Count; i++)
            {
                // Find brand and model corresponding to id
                var currentBrand = Hardware.FirstOrDefault(m => m.ConfigId == hardwareId[i]).Brand;
                var currentModel = Hardware.FirstOrDefault(m => m.ConfigId == hardwareId[i]).Model;

                // If there are enough transactions -> draws a chart and computers a forecasted price
                allCharts.Add(new ChartItem
                {
                    Label = $"{currentBrand.ToString()} {currentModel}",
                    Fill = false,
                    BorderWidth = 1,
                    // Computers price forecast. Returns a list of objects containing purchase date and price.
                    LstData = ComputeForecast(hardwareId[i])
                });
            }
            return allCharts;
        }

        private List<ChartTransaction> ComputeForecast(int id)
        {
            List<ChartTransaction> transactions = new List<ChartTransaction>();

            Brand selectedBrand = Hardware.FirstOrDefault(m => m.ConfigId == id).Brand;
            string selectedModel = Hardware.FirstOrDefault(m => m.ConfigId == id).Model;

            // RemoveUnforecastableIds method already checks if there are enough transactions to compute.
            TimeSeriesPrediction prediction = new TimeSeriesPrediction(m_phones, selectedBrand, selectedModel);
            prediction.GenerateFutureForecast(FutureForecastMonths);

            List<ChartTransaction> transactionRecord = new List<ChartTransaction>();
            foreach (Phone p in prediction.PhoneCollection.Phones)
            {
                // Adds to the List<Dict<int, ChartTrans>> so calculations can be done
                // In order to find the best/worst future price
                transactionRecord.Add(new ChartTransaction
                {
                    Date = p.Date,
                    Price = p.Forecast.Value
                });

                // Adds to the view model, in order to display to the user
                transactions.Add(new ChartTransaction
                {
                    Date = p.Date,
                    Price = p.Forecast.Value
                });
            }
            this.PermanentlyRecordForecast(id, transactionRecord);

            return transactions;
        }

        private void PermanentlyRecordForecast(int id, List<ChartTransaction> transactions)
        {
            // Remove previous record of this phone
            if(ModelPrices.Exists(x => x.ContainsKey(id))) {
                ModelPrices.Where(x => x.ContainsKey(id)).First().Remove(id);
            }

            ModelPrices.Add(new Dictionary<int, List<ChartTransaction>> {{ id, transactions }});
        }

        private List<int> RemoveUnforecastableIds(List<int> hardwareId)
        {
            List<int> AcceptedIds = new List<int>();

            foreach (int id in hardwareId)
            {
                Brand selectedBrand = Hardware.FirstOrDefault(m => m.ConfigId == id).Brand;
                string selectedModel = Hardware.FirstOrDefault(m => m.ConfigId == id).Model;
                if (DataAuditing.HasEnoughTransactions(m_phones, selectedBrand, selectedModel))
                {
                    AcceptedIds.Add(id);
                }
                else
                {
                    Errors.Add($"{selectedBrand} {selectedModel} - Not Enough Transactions");
                    Errors = Errors.Distinct().ToList();
                }
            }

            return AcceptedIds;
        }
    }
}
