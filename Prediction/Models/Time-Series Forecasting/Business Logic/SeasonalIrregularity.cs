using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction.Models.Time_Series_Forecasting.Business_Logic
{
    internal class SeasonalIrregularity
    {
        public static double? Calculate(int index, PhoneCollection items)
        {
            if (items.Items[index].CenteredMovingAverage != null && items.Items[index].CenteredMovingAverage.GetValueOrDefault() != 0)
            {
                return (items.Items[index].Price / items.Items[index].CenteredMovingAverage);
            }
            else
            {
                return null;
            }
        }
    }
}
