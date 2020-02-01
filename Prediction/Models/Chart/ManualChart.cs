using Microsoft.AspNetCore.Mvc;
using Prediction.Models.Enums;
using Prediction.Models.Hardware;
using Prediction.Models.NewChart;
using Prediction.Models.Time_Series_Forecasting;
using Prediction.Models.Time_Series_Forecasting.Cleaning;
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

        /// <summary>
        /// Repository of all sold models, including information such as:
        /// Brand, model, ram, cpu, gpu, ssd, and more.
        /// </summary>
        public List<PhoneProperties> Hardware { get; set; } = new List<PhoneProperties>();

        /// <summary>
        /// Transaction information, such as:
        /// Brand, model, date of sale, price
        /// </summary>
        public List<Item> Phones { get; set; } = new List<Item>();

        /// <summary>
        /// Once the user has selected a specific model, said model's id is stored in this list to be queried. 
        /// </summary>
        public List<int> SelectedItems { get; set; }

        /// <summary>
        /// ChartItems contains information relevant to the drawings of line charts, such as:
        /// Label, color, fill, border width, and transaction information to be drawn (Date-Price)
        /// </summary>
        public List<ChartItem> ChartItems { get; set; } = new List<ChartItem>();
        
        /// <summary>
        /// List holding all found errors
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();

        #region Draw Charts
        public List<ChartItem> DrawChart(List<int> hardwareId)
        {
            List<ChartItem> allCharts = new List<ChartItem>();

            // Itterates through all passed id
            for(int i = 0; i < hardwareId.Count(); i++)
            {
                // Find brand and model corresponding to id
                var currentBrand = Hardware.FirstOrDefault(m => m.ConfigId == hardwareId[i]).Brand;
                var currentModel = Hardware.FirstOrDefault(m => m.ConfigId == hardwareId[i]).Model;

                // If there are enough transactions -> draws a chart and computers a forecasted price
                allCharts.Add(new ChartItem
                {
                    Label = $"{currentBrand.ToString()} {currentModel}",
                    BackgroundColor = new Chart.Misc.RGBAColor(255, 0, 0),
                    BorderColor = new Chart.Misc.RGBAColor(255, 0, 0),
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
            // A ChartTransaction object contains date of purchase and price.
            List<ChartTransaction> transactions = new List<ChartTransaction>();

            // Find brand and model based on passed id
            Brand selectedBrand = Hardware.FirstOrDefault(m => m.ConfigId == id).Brand;
            string selectedModel = Hardware.FirstOrDefault(m => m.ConfigId == id).Model;

            // Generates time series forecast.
            // RemoveUnforecastableIds method already checks if there are enough transactions to compute.
            TimeSeriesPrediction prediction = new TimeSeriesPrediction(Phones, selectedBrand, selectedModel);
            prediction.GenerateFutureForecast(12);

            // Itterate through all objects
            foreach (Phone p in prediction.PhoneCollection.Phones)
            {
                // Records their date and price.
                transactions.Add(new ChartTransaction
                {
                    Date = $"{p.Date.Month}/{p.Date.Year}",
                    Price = p.Forecast.Value
                });
            }

            return transactions;
        }
        #endregion

        #region Filter Unforecastable Ids
        public List<int> RemoveUnforecastableIds(List<int> hardwareId)
        {
            List<int> AcceptedIds = new List<int>();

            foreach(int id in hardwareId)
            {
                Brand selectedBrand = Hardware.FirstOrDefault(m => m.ConfigId == id).Brand;
                string selectedModel = Hardware.FirstOrDefault(m => m.ConfigId == id).Model;
                if (DataAuditing.HasEnoughTransactions(Phones, selectedBrand, selectedModel))
                {
                    AcceptedIds.Add(id);
                }
                else
                {
                    Errors.Add($"ERROR: {selectedBrand} {selectedModel} - Not Enough Transactions");
                    Errors = Errors.Distinct().ToList();
                }
            }

            return AcceptedIds;
        }

        public List<int> RemoveDuplicateIds()
        {
            return SelectedItems.Distinct().ToList();
        }
        #endregion

        #region Select/Unselect
        public void UpdateSelectedItems(List<int> hardwareId)
        {
            foreach(PhoneProperties prop in Hardware)
            {
                if(hardwareId.Contains(prop.ConfigId))
                {
                    prop.isSelected = true;
                }
                else
                {
                    prop.isSelected = false;
                }
            }
        }
        #endregion
    }
}
