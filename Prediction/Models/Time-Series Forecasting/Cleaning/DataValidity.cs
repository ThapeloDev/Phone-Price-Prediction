using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction.Models.Time_Series_Forecasting.Cleaning
{
    // The degree to which the measures conform to defined business rules or constraints
    internal class DataValidity
    {
        public static List<Phone> FillGaps(PhoneCollection collection)
        {
            // Finds the earliest date in the collection of phones
            var start = collection.Phones
                        .Min(i => i.Date);

            // Finds the latest date in the collection of phones
            var end = collection.Phones
                      .Max(i => i.Date);

            if (start == end)
            {
                throw new Exception("Pre-processing exception: Collection's start and end date are the same.");
            }
            if (start > end)
            {
                throw new Exception("Pre-processing exception: Collection's end date is earlier than the start date.");
            }

            //collection.Phones.RemoveAll(x => x.Date.Month == 3);
            //collection.Phones.RemoveAll(x => x.Date.Month == 4);

            // Checks if all phones in collection have the same brand and model.
            if (DataAuditing.HasConsistentBranding(collection.Phones))
            {
                // Loops - month by month - through all dates from the collection's beginning to the end.
                for (var date = start; date < end; date = date.AddMonths(1))
                {
                    if (collection.ContainsDate(date) == false)
                    {
                        DateTime currentDate = new DateTime(date.Year, date.Month, 1);

                        collection.AddItem(collection.Phones[0].Brand,
                                           collection.Phones[0].Model,
                                           currentDate,
                                           GenerateSuggestedPrice(collection.Phones, currentDate));
                    }
                }
            }

            // Sort all phones in ascending order by Datetime
            collection.Phones.Sort((x, y) => DateTime.Compare(x.Date, y.Date));

            return collection.Phones;
        }

        #region DataValidity Private Functions
        private static double? GenerateSuggestedPrice(List<Phone> phones, DateTime date)
        {
            // Finds the phone object with date before the passed date
            var phoneBefore = phones
                              .Where(i => i.Date < date)
                              .OrderByDescending(i => i.Date)
                              .FirstOrDefault();

            // Finds the phone object with date after the passed date
            var phoneAfter = phones
                             .Where(i => i.Date > date)
                             .OrderByDescending(i => i.Date)
                             .LastOrDefault();

            if (phoneBefore == null || phoneAfter == null)
            {
                throw new Exception("Pre-processing exception: Collection is empty.");
            }
            if (phoneBefore == phoneAfter)
            {
                throw new Exception("Pre-processing exception: Collection is ambiguous.");
            }
            
            return CalculatePrice(phoneBefore, phoneAfter);

        }

        private static double CalculatePrice(Phone phoneBefore, Phone phoneAfter)
        {
            var dateBefore = phoneBefore.Date;
            var dateAfter = phoneAfter.Date;

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
        #endregion
    }
}
