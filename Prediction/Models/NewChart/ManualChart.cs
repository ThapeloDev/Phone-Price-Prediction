using Microsoft.AspNetCore.Mvc;
using Prediction.Models.Enums;
using Prediction.Models.Hardware;
using Prediction.Models.NewChart;
using Prediction.Models.Time_Series_Forecasting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction.Models.ChartManual
{
    public class ManualChart
    {

        public ManualChart(List<Item> phones, List<PhoneProperties> hardware, List<int> selectedItems = null)
        {
            this.Hardware = hardware;
            this.SelectedItems = selectedItems;
            this.Phones = phones;
        }

        public List<PhoneProperties> Hardware { get; set; } = new List<PhoneProperties>();
        public List<Item> Phones { get; set; } = new List<Item>();
        public List<int> SelectedItems { get; set; }
        public List<ChartItem> ChartItems { get; set; } = new List<ChartItem>();

        public List<ChartItem> GenerateChart(List<int> hardwareId)
        {
            List<List<ChartTransaction>> allTransactions = new List<List<ChartTransaction>>();

            foreach(int selectedPhoneId in hardwareId)
            {
                Brand selectedBrand = Hardware.FirstOrDefault(m => m.ConfigId == selectedPhoneId).Brand;
                string selectedModel = Hardware.FirstOrDefault(m => m.ConfigId == selectedPhoneId).Model;
                var transactions = Phones.Where(m => m.Brand == selectedBrand).Where(m => m.Model == selectedModel).ToList();

                TimeSeriesPrediction prediction = new TimeSeriesPrediction(transactions, Timeframe.Monthly);
                prediction.GenerateFutureForecast(12);

                allTransactions.Add(new List<ChartTransaction>());
                foreach (Phone p in prediction.PhoneCollection.Phones)
                {
                    allTransactions.Last().Add(new ChartTransaction
                    {
                        Date = $"{p.Date.Month}/{p.Date.Year}",
                        Price = p.Forecast.Value
                    });
                }
            }

            List<ChartItem> allCharts = new List<ChartItem>();

            for(int i = 0; i < allTransactions.Count(); i++)
            {
                allCharts.Add(new ChartItem
                {
                    Label = $"{Hardware.FirstOrDefault(m => m.ConfigId == hardwareId[i]).Brand.ToString()} {Hardware.FirstOrDefault(m => m.ConfigId == hardwareId[i]).Model}",
                    BackgroundColor = new Chart.Misc.RGBAColor(255, 0, 0),
                    BorderColor = new Chart.Misc.RGBAColor(255, 0, 0),
                    Fill = false,
                    BorderWidth = 1,
                    LstData = allTransactions[i]
                });
            }

            return allCharts;
        }
    }
}
