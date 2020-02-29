using Prediction.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction.Models.Time_Series_Forecasting.Business_Logic
{
    internal class CenteredMovingAverage
    {
        public static List<Phone> Calculate(PhoneCollection collection, Timeframe timeframe = Timeframe.Quarterly)
        {
            for(int index = 0; index < collection.Phones.Count(); index++)
            {
                if (collection.Phones.ElementAtOrDefault(index).MovingAverage != null && collection.Phones.ElementAtOrDefault(index + 1).MovingAverage != null)
                {
                    collection.Phones[index].CenteredMovingAverage = (collection.Phones[index].MovingAverage + collection.Phones[index + 1].MovingAverage) / 2;
                }
            }

            return collection.Phones;
        }
    }
}
