using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction.Models.Time_Series_Forecasting.Cleaning
{
    // The degree to which the measures conform to defined business rules or constraints
    internal class DataValidity
    {
        public static List<Phone> FillGaps(PhoneCollection items)
        {
            // Finds the earliest date in the collection of phones
            var earliestDateInCollection = items.Items
                                           .Where(i => i.Date.HasValue)
                                           .Min(i => i.Date);

            // Finds the latest date in the collection of phones
            var latestDateIncollection = items.Items
                                         .Where(i => i.Date.HasValue)
                                         .Max(i => i.Date);

            // The LINQ returns optional DateTime
            // Some operations cannot be done on optionals
            // Earliest and latest dates are casted to non-optionals

            var start = earliestDateInCollection ?? DateTime.MinValue;
            var end = latestDateIncollection ?? DateTime.MinValue;

            // Throw exception if LINQ returns DateTime.Min from casting
            if (start == DateTime.MinValue || end == DateTime.MinValue)
            {
                throw new Exception("Pre-processing exception: Collection's start and end date are ambiguous.");
            }
            if (start == end)
            {
                throw new Exception("Pre-processing exception: Collection's start and end date are the same.");
            }
            if (start > end)
            {
                throw new Exception("Pre-processing exception: Collection's end date is earlier than the start date.");
            }

            items.Items.RemoveAll(x => x.Date.Value.Month == 3);
            items.Items.RemoveAll(x => x.Date.Value.Month == 4);

            for (var date = start; date < end; date = date.AddMonths(1))
            {
                if (ContainsSpecifiedMonth(items.Items, date) == false)
                {
                    if(CheckForConsistency(items.Items))
                    {
                        var brand = items.Items.First().Brand;
                        var model = items.Items.First().Model;

                        Phone newPhone = new Phone(items.Items[0].Brand,
                                                    items.Items[0].Model,
                                                    new DateTime(date.Year, date.Month, 1),
                                                    CreateItem(items.Items, new DateTime(date.Year, date.Month, 1)));
                        items.Items.Add(newPhone);
                    }
                }
            }

            // Sort all phones in ascending order by Datetime
            items.Items.Sort((x, y) => DateTime.Compare(x.Date.Value, y.Date.Value));

            return items.Items;
        }

        private static double? CreateItem(List<Phone> phones, DateTime date)
        {
            // Finds the phone object with date before the passed date
            var phoneBefore = phones
                              .Where(i => i.Date.Value < date)
                              .OrderByDescending(i => i.Date.Value)
                              .FirstOrDefault();

            // Finds the phone object with date after the passed date
            var phoneAfter = phones
                             .Where(i => i.Date.Value > date)
                             .OrderByDescending(i => i.Date.Value)
                             .LastOrDefault();

            if (phoneBefore == null || phoneAfter == null)
            {
                throw new Exception("Pre-processing exception: Collection is empty.");
            }
            if (phoneBefore == phoneAfter)
            {
                throw new Exception("Pre-processing exception: Collection is ambiguous.");
            }

            if (CheckForConsistency(phones))
            {
                return CalculatePrice(phoneBefore, phoneAfter);
            }
            else
            {
                return null;
            }
        }

        private static bool CheckForConsistency(List<Phone> list)
        {
            var brand = list.First().Brand;
            var model = list.First().Model;

            if (list.All(x => x.Brand == brand) && list.All(x => x.Model == model))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static double CalculatePrice(Phone phoneBefore, Phone phoneAfter)
        {
            // The LINQ returns optional DateTime
            // Some operations cannot be done on optionals
            // Earliest and latest dates are casted to non-optionals
            var dateBefore = phoneBefore.Date ?? DateTime.MinValue;
            var dateAfter = phoneAfter.Date ?? DateTime.MinValue;

            // Throw exception if LINQ returns DateTime.Min from casting
            if (dateBefore == DateTime.MinValue || dateAfter == DateTime.MinValue)
            {
                throw new Exception("Pre-processing exception: Collection's start and end date are ambiguous.");
            }
            if (dateBefore == dateAfter)
            {
                throw new Exception("Pre-processing exception: Collection's start and end date are the same.");
            }
            if (dateBefore > dateAfter)
            {
                throw new Exception("Pre-processing exception: Collection's end date is earlier than the start date.");
            }

            int monthDifference = GetMonthDifference(dateBefore, dateAfter);

            if (!phoneBefore.Price.HasValue || !phoneAfter.Price.HasValue)
            {
                throw new Exception("Pre-processing exception: Ambiguously priced phones, cannot finish pre-processing.");
            }

            double phonePriceBefore = phoneBefore.Price.Value;
            double phonePriceAfter = phoneAfter.Price.Value;

            double price = (phonePriceBefore - ((phonePriceBefore - phonePriceAfter) / monthDifference));
            return price;
        }

        private static int GetMonthDifference(DateTime date1, DateTime date2)
        {
            // Note: it doesn't matter which dates comes before the other.
            var dateSpan = DateTimeSpan.CompareDates(date1, date2);
            return (dateSpan.Years * 12) + dateSpan.Months;
        }

        private static bool ContainsSpecifiedMonth(List<Phone> phones, DateTime date)
        {
            var monthIsFound = phones
                               .Where(i => i.Date.HasValue)
                               .Where(i => i.Date.Value.Month == date.Month)
                               .Where(i => i.Date.Value.Year == date.Year)
                               .ToList();

            if (monthIsFound.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
