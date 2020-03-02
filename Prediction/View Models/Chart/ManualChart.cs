using Microsoft.AspNetCore.Mvc;
using Prediction.Models.Enums;
using Prediction.Models.Hardware;
using Prediction.Models.NewChart;
using Prediction.Models.Time_Series_Forecasting;
using Prediction.Models.Time_Series_Forecasting.Cleaning;
using Prediction.View_Models.Chart.Misc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prediction.Utilities;


namespace Prediction.Models.ChartManual
{
    public class ManualChart
    {

        public ManualChart(List<Item> phones, List<PhoneProperties> hardware, int months = 12, List<int> selectedItems = null)
        {
            this.Hardware = hardware;
            this.Phones = phones;
            this.FutureForecastMonths = months;
            this.SelectedItems = ContainsEnoughData(selectedItems.Distinct().ToList());
        }

        public List<PhoneProperties> Hardware { get; set; } = new List<PhoneProperties>();
        public List<Item> Phones { get; set; } = new List<Item>();
        public List<int> SelectedItems { get; set; }
        public List<ChartItem> ChartItems { get; set; } = new List<ChartItem>();
        public List<string> Errors { get; set; } = new List<string>();
        public int FutureForecastMonths { get; set; }
        private const int ALLOWED_TRANSACTION_GAP_MONTHS = 3;


        #region Generate Chart Data
        public List<ChartItem> DrawChart(List<int> hardwareId)
        {
            List<ChartItem> allCharts = new List<ChartItem>();


            // Itterates through all passed id
            for (int i = 0; i < hardwareId.Count; i++)
            {
                // Find brand and model corresponding to id
                var currentBrand = Hardware.FirstOrDefault(m => m.ConfigId == hardwareId[i]).Brand;
                var currentModel = Hardware.FirstOrDefault(m => m.ConfigId == hardwareId[i]).Model;

                // The constructor already removes phones where gap between last transaction and today is more than ALLOWED_TRANSACTION_GAP_MONTHS
                // I don't need to validate here, it will always be less then that specified month gap.
                DateTime today = DateTime.Today;
                DateTime latestDate = Phones.Where(x => x.Brand == currentBrand && x.Model == currentModel).Max(x => x.Date);
                var datetime = DateTimeSpan.CompareDates(latestDate, today);
                int monthDifference = datetime.Years * 12 + datetime.Months;

                // If there are enough transactions -> draws a chart and computers a forecasted price
                allCharts.Add(new ChartItem
                {
                    Label = $"{currentBrand.ToString()} {currentModel}",
                    Fill = false,
                    BorderWidth = 1,
                    // Computers price forecast. Returns a list of objects containing purchase date and price.
                    LstData = ComputeForecast(hardwareId[i], (FutureForecastMonths + monthDifference))
                });
            }

            return allCharts;
        }

        private List<ChartTransaction> ComputeForecast(int id, int forecastMonths = 12)
        {
            var asd = ContainsEnoughData(SelectedItems);

            // A ChartTransaction object contains date of purchase and price.
            List<ChartTransaction> transactions = new List<ChartTransaction>();

            // Find brand and model based on passed id
            Brand selectedBrand = Hardware.FirstOrDefault(m => m.ConfigId == id).Brand;
            string selectedModel = Hardware.FirstOrDefault(m => m.ConfigId == id).Model;

            // Generates time series forecast.
            // RemoveUnforecastableIds method already checks if there are enough transactions to compute.
            TimeSeriesPrediction prediction = new TimeSeriesPrediction(Phones, selectedBrand, selectedModel);
            prediction.GenerateFutureForecast(forecastMonths);

            DateTime today = DateTime.Today;
            // Itterate through all objects
            foreach (Phone p in prediction.PhoneCollection.Phones)
            {
                DateTime currentDate = p.Date;
                var datetime = DateTimeSpan.CompareDates(currentDate, today);
                int yearDifference = datetime.Years;

                // Forecast is generated using ALL data
                // Chart only displays data from the last 2 years + the forecast
                if(yearDifference <= 2)
                {
                    // Adds to the List<Dict<int, ChartTrans>> so calculations can be done
                    // In order to find the best/worst future price
                    this.AddTransactionToRecord(id, new ChartTransaction { Date = p.Date, Price = p.Forecast.Value });
                    System.Diagnostics.Debug.WriteLine($"GENERATING FORECAST: {p.Date} - {p.Forecast.Value}");
                    // Records their date and price.
                    transactions.Add(new ChartTransaction
                    {
                        Date = p.Date,
                        Price = p.Forecast.Value
                    });
                }
            }

            return transactions;
        }
        #endregion

        #region Filter Unforecastable Ids
        public List<int> RemoveUnforecastableIds(List<int> hardwareId)
        {
            List<int> AcceptedIds = new List<int>();

            foreach (int id in hardwareId)
            {
                Brand selectedBrand = Hardware.FirstOrDefault(m => m.ConfigId == id).Brand;
                string selectedModel = Hardware.FirstOrDefault(m => m.ConfigId == id).Model;
                if (DataAuditing.HasEnoughTransactions(Phones, selectedBrand, selectedModel))
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

        public List<int> RemoveDuplicateIds()
        {
            return SelectedItems.Distinct().ToList();
        }
        #endregion

        #region Select/Unselect
        public void UpdateSelectedItems(List<int> hardwareId)
        {
            foreach (PhoneProperties prop in Hardware)
            {
                if (hardwareId.Contains(prop.ConfigId))
                    prop.isSelected = true;
                else
                    prop.isSelected = false;
            }
        }
        #endregion

        #region Meta: Min/Max Prices
        private void AddTransactionToRecord(int id, ChartTransaction transaction)
        {
            if (ForecastRecord._dict.ContainsKey(id))
                ForecastRecord._dict[id].Add(transaction);
            else
                ForecastRecord._dict.Add(id, new List<ChartTransaction> { { transaction } });
        }

        public Tuple<int, DateTime, double> FindLowestPriceOverall()
        {
            int id = new int();
            DateTime date = new DateTime();
            double price = int.MaxValue;

            foreach (int currentId in ForecastRecord._dict.Keys)
            {
                // Gets the minimum price from the forecasted months, not the whole dataset
                var min = ForecastRecord._dict[currentId].TakeLast(FutureForecastMonths).Min(x => x.Price);
                if (price > min)
                {
                    price = min;
                    id = currentId;
                    date = ForecastRecord._dict[currentId].Where(x => x.Price == min).Select(x => x.Date).FirstOrDefault();
                }
            }

            return new Tuple<int, DateTime, double>(id, date, price);
        }

        public Tuple<int, DateTime, double> FindHighestPriceOverall()
        {
            int id = new int();
            DateTime date = new DateTime();
            double price = int.MinValue;

            foreach (int currentId in ForecastRecord._dict.Keys)
            {
                // Gets the minimum price from the forecasted months, not the whole dataset
                var max = ForecastRecord._dict[currentId].TakeLast(FutureForecastMonths).Max(x => x.Price);
                if (max > price)
                {
                    price = max;
                    id = currentId;
                    date = ForecastRecord._dict[currentId].Where(x => x.Price == max).Select(x => x.Date).FirstOrDefault();
                }
            }

            return new Tuple<int, DateTime, double>(id, date, price);
        }

        public List<Tuple<int, DateTime, double>> FindLowestPrices()
        {
            List<Tuple<int, DateTime, double>> lowestPrices = new List<Tuple<int, DateTime, double>>();

            foreach (int currentId in ForecastRecord._dict.Keys)
            {
                double minPrice = ForecastRecord._dict[currentId].TakeLast(FutureForecastMonths).Min(x => x.Price);
                DateTime date = ForecastRecord._dict[currentId].Where(x => x.Price == minPrice).Select(x => x.Date).FirstOrDefault();
                lowestPrices.Add(new Tuple<int, DateTime, double>(currentId, date, minPrice));
            }

            return lowestPrices;
        }

        public List<Tuple<int, DateTime, double>> FindHighestPrices()
        {
            List<Tuple<int, DateTime, double>> highestPrices = new List<Tuple<int, DateTime, double>>();

            foreach(int currentId in ForecastRecord._dict.Keys)
            {
                double maxPrice = ForecastRecord._dict[currentId].TakeLast(FutureForecastMonths).Max(x => x.Price);
                DateTime date = ForecastRecord._dict[currentId].Where(x => x.Price == maxPrice).Select(x => x.Date).FirstOrDefault();
                highestPrices.Add(new Tuple<int, DateTime, double>(currentId, date, maxPrice));
            }

            return highestPrices;
        }
        #endregion

        private List<int> AutogenerateMissingDataIfPossible(List<int> selectedPhoneIds, int months = 3)
        {
            List<int> eligibleIds = selectedPhoneIds;

            DateTime today = DateTime.Today;
            int currentMonth = DateTime.Today.Month;
            int currentYear = DateTime.Today.Year;

            foreach (int id in selectedPhoneIds)
            {
                Brand selectedBrand = Hardware.FirstOrDefault(x => x.ConfigId == id).Brand;
                string selectedModel = Hardware.FirstOrDefault(x => x.ConfigId == id).Model;

                DateTime latestDate = Phones.Where(x => x.Brand == selectedBrand && x.Model == selectedModel).Max(x => x.Date);
                var datetime = DateTimeSpan.CompareDates(latestDate, today);
                int monthDifference = datetime.Years * 12 + datetime.Months;

                // Remove id if latest date cannot be found or the gap is too big
                if (latestDate == null || monthDifference > 3)
                {
                    eligibleIds.RemoveAll(x => x == id);
                    Errors.Add($"{selectedBrand} {selectedModel} - Data for the last {monthDifference} months is unavailable.");
                    Errors = Errors.Distinct().ToList();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Generating data");

                    TimeSeriesPrediction prediction = new TimeSeriesPrediction(Phones, selectedBrand, selectedModel);
                    prediction.GenerateFutureForecast(monthDifference);
                }
            }

            return new List<int>();
        }

        private List<int> ContainsEnoughData(List<int> selectedIds)
        {
            List<int> eligibleIds = new List<int>();

            DateTime today = DateTime.Today;

            foreach(int id in selectedIds)
            {
                Brand selectedBrand = Hardware.FirstOrDefault(x => x.ConfigId == id).Brand;
                string selectedModel = Hardware.FirstOrDefault(x => x.ConfigId == id).Model;

                DateTime? latestTransactionDate = null;

                try
                {
                    latestTransactionDate = Phones.Where(x => x.Brand == selectedBrand && x.Model == selectedModel).Max(x => x.Date);
                }
                catch (InvalidOperationException e)
                {
                    Errors.Add($"{selectedBrand} {selectedModel} - No data available.");
                    Errors = Errors.Distinct().ToList();
                    break;
                }

                var datetime = DateTimeSpan.CompareDates(latestTransactionDate.Value, today);
                int monthDifference = datetime.Years * 12 + datetime.Months;

                System.Diagnostics.Debug.WriteLine($"INSIDE CONTAINS ID:\n    Brand {selectedBrand} Model {selectedModel}\n    Today: {today} Latest Date {latestTransactionDate}\n    Month Difference {monthDifference}");

                if(latestTransactionDate != null && monthDifference <= ALLOWED_TRANSACTION_GAP_MONTHS)
                {
                    eligibleIds.Add(id);
                } else
                {
                    Errors.Add($"{selectedBrand} {selectedModel} - {monthDifference} months of data is missing.");
                    Errors = Errors.Distinct().ToList();
                    break;
                }
            }

            return eligibleIds;
        }
    }
}
