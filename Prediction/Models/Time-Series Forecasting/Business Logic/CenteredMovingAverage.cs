using Prediction.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction.Models.Time_Series_Forecasting.Business_Logic
{
    internal class CenteredMovingAverage
    {
        public static double? Calculate(int index, PhoneCollection items, Timeframe timeframe = Timeframe.Quarterly)
        {
            if (items.Items.ElementAtOrDefault(index).MovingAverage != null && items.Items.ElementAtOrDefault(index + 1).MovingAverage != null)
            {
                return (items.Items[index].MovingAverage + items.Items[index + 1].MovingAverage) / 2;
            }
            else
            {
                return null;
            }
        }
    }
}
