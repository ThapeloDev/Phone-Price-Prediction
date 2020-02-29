using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction.Models.Time_Series_Forecasting.Cleaning
{
    public class DataDuplication
    {
        public static List<Phone> RemoveDuplicates(PhoneCollection collection)
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

            // Checks if all phones in collection have the same brand and model.
            if (DataAuditing.HasConsistentBranding(collection.Phones))
            {
                // Loops - month by month - through all dates from the collection's beginning to the end.
                for (var date = start; date < end; date = date.AddMonths(1))
                {
                    if (collection.ContainsDate(date))
                    {
                        var occurancesOfItem = collection.Phones
                                               .Where(i => i.Date.Month == date.Month)
                                               .Where(i => i.Date.Year == date.Year);
                        
                        if(occurancesOfItem.Count() > 1)
                        {
                            // Retrieves current date
                            DateTime currentDate = new DateTime(date.Year, date.Month, 1);

                            // Calculates the average price for items with the same month and year
                            var averagePrice = occurancesOfItem
                                               .Select(i => i.Price)
                                               .Average();

                            // Remove all phones 
                            collection.Phones.RemoveAll(x => x.Date.Month == date.Month && x.Date.Year == date.Year);

                            collection.AddItem(collection.Phones[0].Brand,
                                               collection.Phones[0].Model,
                                               currentDate,
                                               averagePrice);
                        }
                    }
                }
            }

            collection.Phones.Sort((x, y) => DateTime.Compare(x.Date, y.Date));

            return collection.Phones;
        }
    }
}
