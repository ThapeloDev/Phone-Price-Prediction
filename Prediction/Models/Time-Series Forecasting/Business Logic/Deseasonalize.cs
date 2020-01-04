using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction.Models.Time_Series_Forecasting.Business_Logic
{
    internal class Deseasonalize
    {
        public static double? Calculate(int index, PhoneCollection items)
        {
            if (items.Items[index].Seasonality != null)
            {
                return (items.Items[index].Price / items.Items[index].Seasonality);
            }
            return null;
        }
    }
}
