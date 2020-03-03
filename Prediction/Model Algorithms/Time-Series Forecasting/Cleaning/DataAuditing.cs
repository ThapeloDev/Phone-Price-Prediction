using Prediction.Models.Enums;
using Prediction.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prediction.Models.Time_Series_Forecasting.Cleaning
{
    // The data is audited with the use of statistical and database methods to detect anomalies and contradictions.
    public class DataAuditing
    {
        public static bool HasConsistentBranding(List<Phone> list)
        {
            var brand = list.First().Brand;
            var model = list.First().Model;

            if (list.All(x => x.Brand == brand) && list.All(x => x.Model == model))
                return true;
            else
                return false;
        }

        public static bool HasEnoughTransactions(List<Item> list, Brand brand, string model)
        {
            List<Item> transactions = list.Where(m => m.Brand == brand).Where(m => m.Model == model).ToList();


            if (transactions.Count() <= 1 || transactions == null)
                return false;

            DateTime earliestDate = transactions.Select(m => m.Date).Min();
            DateTime latestDate = transactions.Select(m => m.Date).Max();
            var datetime = DateTimeSpan.CompareDates(earliestDate, latestDate);
            int monthDifference = datetime.Years * 12 + datetime.Months;

            System.Diagnostics.Debug.WriteLine($"DATE: {earliestDate.ToString()} - {latestDate.ToString()} = {monthDifference}");

            if (monthDifference < 24)
                return false;
            if (transactions.Count() < ((monthDifference / 100) * 90))
                return false;

            return true;
        }
    }
}
