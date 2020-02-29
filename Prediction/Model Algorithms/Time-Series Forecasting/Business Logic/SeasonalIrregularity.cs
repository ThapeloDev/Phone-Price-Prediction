using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction.Models.Time_Series_Forecasting.Business_Logic
{
    internal class SeasonalIrregularity
    {
        public static List<Phone> Calculate(PhoneCollection collection)
        {
            for (int index = 0; index < collection.Phones.Count(); index++)
            {
                if (collection.Phones[index].CenteredMovingAverage != null && collection.Phones[index].CenteredMovingAverage.GetValueOrDefault() != 0)
                {
                    collection.Phones[index].SeasonalIrregularity = (collection.Phones[index].Price / collection.Phones[index].CenteredMovingAverage);
                }
            }

            return collection.Phones;
        }
    }
}
