using Prediction.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction.Models.Time_Series_Forecasting.Business_Logic
{
    internal class Seasonality
    {
        public static List<Phone> Calculate(PhoneCollection collection, Timeframe timeframe = Timeframe.Quarterly)
        {
            for(int index = 0; index < collection.Phones.Count(); index++)
            {
                collection.Phones[index].Seasonality = Calculate(index, collection, timeframe);
            }
            return collection.Phones;
        }

        public static double? Calculate(int index, PhoneCollection items, Timeframe timeframe = Timeframe.Quarterly)
        {
            int month = items.Phones[index].Date.Month;

            if (timeframe == Timeframe.Quarterly)
            {
                switch (month)
                {
                    // January, February, March: calculate Seasonal Irregularity average
                    case 1:
                    case 2:
                    case 3:
                        return items.Phones
                               .Where(x => (x.Date.Month == 1 || x.Date.Month == 2 || x.Date.Month == 3) && x.SeasonalIrregularity != null)
                               .Select(x => x.SeasonalIrregularity)
                               .Average();
                    // April, May, June: calculate Seasonal Irregularity average
                    case 4:
                    case 5:
                    case 6:
                        return items.Phones
                               .Where(x => (x.Date.Month == 4 || x.Date.Month == 5 || x.Date.Month == 6) && x.SeasonalIrregularity != null)
                               .Select(x => x.SeasonalIrregularity)
                               .Average();
                    // July, August, September: calculate Seasonal Irregularity average
                    case 7:
                    case 8:
                    case 9:
                        return items.Phones
                               .Where(x => (x.Date.Month == 7 || x.Date.Month == 8 || x.Date.Month == 9) && x.SeasonalIrregularity != null)
                               .Select(x => x.SeasonalIrregularity)
                               .Average();
                    // October, November, December: calculate Seasonal Irregularity average
                    case 10:
                    case 11:
                    case 12:
                        return items.Phones
                               .Where(x => (x.Date.Month == 10 || x.Date.Month == 11 || x.Date.Month == 12) && x.SeasonalIrregularity != null)
                               .Select(x => x.SeasonalIrregularity)
                               .Average();
                    default:
                        return null;
                }

            }
            else if (timeframe == Timeframe.Monthly)
            {
                switch (month)
                {
                    // January: calculate Seasonal Irregularity average
                    case 1:
                        return items.Phones
                               .Where(x => x.Date.Month == 1 && x.SeasonalIrregularity != null)
                               .Select(x => x.SeasonalIrregularity)
                               .Average();
                    // February: calculate Seasonal Irregularity average
                    case 2:
                        return items.Phones
                               .Where(x => x.Date.Month == 2 && x.SeasonalIrregularity != null)
                               .Select(x => x.SeasonalIrregularity)
                               .Average();
                    // March: calculate Seasonal Irregularity average
                    case 3:
                        return items.Phones
                               .Where(x => x.Date.Month == 3 && x.SeasonalIrregularity != null)
                               .Select(x => x.SeasonalIrregularity)
                               .Average();
                    // April: calculate Seasonal Irregularity average
                    case 4:
                        return items.Phones
                               .Where(x => x.Date.Month == 4 && x.SeasonalIrregularity != null)
                               .Select(x => x.SeasonalIrregularity)
                               .Average();
                    // May: calculate Seasonal Irregularity average
                    case 5:
                        return items.Phones
                               .Where(x => x.Date.Month == 5 && x.SeasonalIrregularity != null)
                               .Select(x => x.SeasonalIrregularity)
                               .Average();
                    // June: calculate Seasonal Irregularity average
                    case 6:
                        return items.Phones
                               .Where(x => x.Date.Month == 6 && x.SeasonalIrregularity != null)
                               .Select(x => x.SeasonalIrregularity)
                               .Average();
                    // July: calculate Seasonal Irregularity average
                    case 7:
                        return items.Phones
                               .Where(x => x.Date.Month == 7 && x.SeasonalIrregularity != null)
                               .Select(x => x.SeasonalIrregularity)
                               .Average();
                    // August: calculate Seasonal Irregularity average
                    case 8:
                        return items.Phones
                               .Where(x => x.Date.Month == 8 && x.SeasonalIrregularity != null)
                               .Select(x => x.SeasonalIrregularity)
                               .Average();
                    // September: calculate Seasonal Irregularity average
                    case 9:
                        return items.Phones
                               .Where(x => x.Date.Month == 9 && x.SeasonalIrregularity != null)
                               .Select(x => x.SeasonalIrregularity)
                               .Average();
                    // October: calculate Seasonal Irregularity average
                    case 10:
                        return items.Phones
                               .Where(x => x.Date.Month == 10 && x.SeasonalIrregularity != null)
                               .Select(x => x.SeasonalIrregularity)
                               .Average();
                    // November: calculate Seasonal Irregularity average
                    case 11:
                        return items.Phones
                               .Where(x => x.Date.Month == 11 && x.SeasonalIrregularity != null)
                               .Select(x => x.SeasonalIrregularity)
                               .Average();
                    // December: calculate Seasonal Irregularity average
                    case 12:
                        return items.Phones
                               .Where(x => x.Date.Month == 12 && x.SeasonalIrregularity != null)
                               .Select(x => x.SeasonalIrregularity)
                               .Average();
                    default:
                        return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
