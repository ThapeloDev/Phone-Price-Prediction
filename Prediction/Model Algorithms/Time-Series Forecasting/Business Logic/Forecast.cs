using System.Collections.Generic;
using System.Linq;

namespace Prediction.Models.Time_Series_Forecasting.Business_Logic
{
    internal class Forecast
    {
        public static List<Phone> Calculate(PhoneCollection collection)
        {
            for (int index = 0; index < collection.Phones.Count(); index++)
            {
                collection.Phones[index].Forecast = Calculate(index, collection);
            }
            return collection.Phones;
        }
        public static double Calculate(int index, PhoneCollection collection)
        {
            return collection.Phones[index].Seasonality.GetValueOrDefault(0) * collection.Phones[index].Trend.GetValueOrDefault(0);
        }
    }
}
