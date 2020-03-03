using Prediction.Models.Enums;
using System.Collections.Generic;
using System.Linq;

namespace Prediction.Models.Time_Series_Forecasting.Business_Logic
{
    internal class MovingAverage
    {
        public static List<Phone> Calculate(PhoneCollection collection, Timeframe timeframe = Timeframe.Quarterly)
        {
            for (int index = 0; index < collection.Phones.Count(); index++)
            {
                if (collection.Phones.ElementAtOrDefault(index - 2) != null && collection.Phones.ElementAtOrDefault(index - 3 + (int)timeframe) != null)
                {
                    double sum = 0.0;

                    for (int i = (index - 2); i < (index - 2 + (int)timeframe); i++)
                        sum += collection.Phones[i].Price.Value;

                    collection.Phones[index].MovingAverage = sum / (int)timeframe;
                }
            }

            return collection.Phones;
        }
    }
}
