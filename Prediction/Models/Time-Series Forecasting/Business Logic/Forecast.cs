using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction.Models.Time_Series_Forecasting.Business_Logic
{
    internal class Forecast
    {
        public static double Calculate(int index, PhoneCollection items)
        {
            return items.Items[index].Seasonality.GetValueOrDefault(0) * items.Items[index].Trend.GetValueOrDefault(0);
        }
    }
}
