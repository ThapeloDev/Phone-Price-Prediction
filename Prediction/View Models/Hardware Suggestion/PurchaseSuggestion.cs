using Prediction.Models;
using Prediction.Models.Enums;
using Prediction.Models.Hardware;
using Prediction.Models.NewChart;
using Prediction.Models.Time_Series_Forecasting;
using Prediction.Models.Time_Series_Forecasting.Cleaning;
using Prediction.Utilities;
using Prediction.View_Models.Chart.Misc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prediction.View_Models.Hardware_Suggestion
{
    public class PurchaseSuggestion
    {
        public PurchaseSuggestion(List<Item> transactions, List<PhoneProperties> hardware, List<int> ids)
        {
            this.Transactions = transactions;
            this.Hardware = hardware;
            this.Ids = ContainsEnoughData(ids.Distinct().ToList());
            this.PhoneScores = GetPhoneScores(new List<int> { 5, 6 });
        }

        private List<Item> Transactions { get; set; }
        public List<PhoneProperties> Hardware { get; set; }
        public List<int> Ids { get; set; }
        public List<ChartItem> ChartItems { get; } = new List<ChartItem>();
        public List<string> Errors { get; set; } = new List<string>();
        public Dictionary<int, double> PhoneScores { get; set; } = new Dictionary<int, double>();

        private const int ALLOWED_TRANSACTION_GAP_MONTHS = 3;


        public List<ChartItem> DrawChart(List<int> hardwareId)
        {
            List<ChartItem> allCharts = new List<ChartItem>();

            for (int i = 0; i<hardwareId.Count; i++)
            {
                var currentBrand = Hardware.FirstOrDefault(x => x.ConfigId == hardwareId[i]).Brand;
                var currentModel = Hardware.FirstOrDefault(x => x.ConfigId == hardwareId[i]).Model;

                DateTime today = DateTime.Today;
                DateTime latestDate = Transactions.Where(x => x.Brand == currentBrand && x.Model == currentModel).Max(x => x.Date);
                var datetime = DateTimeSpan.CompareDates(latestDate, today);
                int monthDifference = datetime.Years * 12 + datetime.Months;

                allCharts.Add(new ChartItem
                {
                    Label = $"{currentBrand.ToString()} {currentModel}",
                    Fill = false,
                    BorderWidth = 1,
                    LstData = ComputeForecast(hardwareId[i], (12 + monthDifference))
                });



            }

            return allCharts;
        }

        private List<ChartTransaction> ComputeForecast(int id, int forecastMonths = 12)
        {
            List<ChartTransaction> transactions = new List<ChartTransaction>();

            Brand currentBrand = Hardware.FirstOrDefault(x => x.ConfigId == id).Brand;
            string currentModel = Hardware.FirstOrDefault(x => x.ConfigId == id).Model;

            TimeSeriesPrediction prediction = new TimeSeriesPrediction(Transactions, currentBrand, currentModel);
            prediction.GenerateFutureForecast(forecastMonths);

            DateTime today = DateTime.Today;

            foreach(Phone p in prediction.PhoneCollection.Phones)
            {
                DateTime currentDate = p.Date;
                var datetime = DateTimeSpan.CompareDates(currentDate, today);
                int yearDifference = datetime.Years;

                if (yearDifference <= 2)
                {
                    this.AddTransactionToRecord(id, new ChartTransaction { Date = p.Date, Price = p.Forecast.Value });
                    transactions.Add(new ChartTransaction
                    {
                        Date = p.Date,
                        Price = p.Forecast.Value
                    });
                }
            }

            return transactions;
        }

        public List<int> RemoveUnforecastableIds(List<int> hardwareId)
        {
            List<int> AcceptedIds = new List<int>();

            foreach (int id in hardwareId)
            {
                Brand selectedBrand = Hardware.FirstOrDefault(m => m.ConfigId == id).Brand;
                string selectedModel = Hardware.FirstOrDefault(m => m.ConfigId == id).Model;
                if (DataAuditing.HasEnoughTransactions(Transactions, selectedBrand, selectedModel))
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

        private void AddTransactionToRecord(int id, ChartTransaction transaction)
        {
            if (ForecastRecord._dict.ContainsKey(id))
                ForecastRecord._dict[id].Add(transaction);
            else
                ForecastRecord._dict.Add(id, new List<ChartTransaction> { { transaction } });
        }

        private List<int> ContainsEnoughData(List<int> selectedIds)
        {
            List<int> eligibleIds = new List<int>();

            DateTime today = DateTime.Today;

            foreach (int id in selectedIds)
            {
                Brand selectedBrand = Hardware.FirstOrDefault(x => x.ConfigId == id).Brand;
                string selectedModel = Hardware.FirstOrDefault(x => x.ConfigId == id).Model;

                DateTime? latestTransactionDate = null;

                try
                {
                    latestTransactionDate = Transactions.Where(x => x.Brand == selectedBrand && x.Model == selectedModel).Max(x => x.Date);
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

                if (latestTransactionDate != null && monthDifference <= ALLOWED_TRANSACTION_GAP_MONTHS)
                {
                    eligibleIds.Add(id);
                }
                else
                {
                    Errors.Add($"{selectedBrand} {selectedModel} - {monthDifference} months of data is missing.");
                    Errors = Errors.Distinct().ToList();
                    break;
                }
            }

            return eligibleIds;
        }

        private Dictionary<int, double> GetPhoneScores(List<int> ids)
        {
            Dictionary<int, double> AllScores = new Dictionary<int, double>();

            foreach(int id in ids)
            {
                List<PhoneProperties> ComparisonSpecs = Hardware.Where(x => x.ConfigId != id).ToList();
                PhoneProperties CurrentSpecs = Hardware.FirstOrDefault(x => x.ConfigId == id);

                System.Diagnostics.Debug.WriteLine($"");

                System.Diagnostics.Debug.WriteLine($"ALGORITHM: Checking {CurrentSpecs.Brand} {CurrentSpecs.Model}");


                List<double> CurrentPhoneScores = new List<double>();

                foreach(PhoneProperties p in ComparisonSpecs)
                {
                    System.Diagnostics.Debug.WriteLine($"ALGORITHM: Calculating percent difference between {CurrentSpecs.Brand} and {p.Brand} -- {GetPercentDifference(CurrentSpecs, p)}");
                    CurrentPhoneScores.Add(GetPercentDifference(CurrentSpecs, p));
                }

                double phoneScore = CurrentPhoneScores.Average();

                AllScores.Add(id, phoneScore);
            }

            foreach(var i in AllScores)
            {
                System.Diagnostics.Debug.WriteLine($"PHONE ID: {i.Key}; PHONE SCORE: {i.Value}");
            }

            return AllScores;
        }


        private double GetPercentDifference(PhoneProperties phone, PhoneProperties comparisonPhone)
        {
            int mainStorage = phone.Storage;
            int compStorage = comparisonPhone.Storage;
            double storageDiff = GetPercentDifference(mainStorage, compStorage);

            int mainCpuCount = phone.CpuCoreCount;
            int compCpuCount = comparisonPhone.CpuCoreCount;
            double cpuCountDiff = GetPercentDifference(mainCpuCount, compCpuCount);

            double mainCpuSpeed = phone.CpuSpeed;
            double compCpuSpeed = comparisonPhone.CpuSpeed;
            double cpuSpeedDiff = GetPercentDifference(mainCpuSpeed, compCpuSpeed);

            int mainRam = phone.RAM;
            int compRam = comparisonPhone.RAM;
            double ramDiff = GetPercentDifference(mainRam, compRam);

            int mainFrontCameraMgpx = phone.FrontCameraMegapixel;
            int compFrontCameraMgpx = comparisonPhone.FrontCameraMegapixel;
            double frontCameraMgpxDiff = GetPercentDifference(mainFrontCameraMgpx, compFrontCameraMgpx);

            int mainBackCameraMgpx = phone.BackCameraMegapixel;
            int compBackCameraMgpx = comparisonPhone.BackCameraMegapixel;
            double backCameraMgpxDiff = GetPercentDifference(mainBackCameraMgpx, compBackCameraMgpx);

            int mainRearCamera = phone.RearCameraCount;
            int compRearCameraMgpx = comparisonPhone.RearCameraCount;
            double rearCameraDiff = GetPercentDifference(mainRearCamera, compRearCameraMgpx);

            System.Diagnostics.Debug.WriteLine($"DIFF: {storageDiff} {cpuCountDiff} {cpuSpeedDiff} {ramDiff} {frontCameraMgpxDiff} {backCameraMgpxDiff} {rearCameraDiff}");

            return new List<double> { storageDiff, cpuCountDiff, cpuSpeedDiff, ramDiff, frontCameraMgpxDiff, backCameraMgpxDiff, rearCameraDiff}.Average();
        } 

        private double GetPercentDifference(int number, int comparisonNumber)
        {
            return (((number - comparisonNumber) / Math.Abs(comparisonNumber)) * 100);
        }
        private double GetPercentDifference(double number, double comparisonNumber)
        {
            return (((number - comparisonNumber) / Math.Abs(comparisonNumber)) * 100);
        }
    }
}
