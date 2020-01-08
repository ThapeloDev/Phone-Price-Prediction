using Prediction.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction.Models.Time_Series_Forecasting.Business_Logic
{
    internal class MovingAverage
    {
        public static double? Calculate(int index, PhoneCollection items, Timeframe timeframe = Timeframe.Quarterly)
        {
            if (items.Items.ElementAtOrDefault(index - 2) != null && items.Items.ElementAtOrDefault(index - 3 + (int)timeframe) != null)
            {
                double sum = 0.0;

                for (int i = (index - 2); i < (index - 2 + (int)timeframe); i++)
                    sum += items.Items[i].Price.Value;

                return sum / (int)timeframe;
            }
            else
            {
                return null;
            }
        }
    }
}
