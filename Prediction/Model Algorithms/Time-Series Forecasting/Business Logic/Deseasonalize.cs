using System.Collections.Generic;
using System.Linq;

namespace Prediction.Models.Time_Series_Forecasting.Business_Logic
{
    internal class Deseasonalize
    {
        public static List<Phone> Calculate(PhoneCollection collection)
        {
            for (int index = 0; index < collection.Phones.Count(); index++)
            {
                if (collection.Phones[index].Seasonality != null)
                {
                    collection.Phones[index].Deseasonalized = (collection.Phones[index].Price / collection.Phones[index].Seasonality);
                }
            }
            return collection.Phones;
        }
    }
}
