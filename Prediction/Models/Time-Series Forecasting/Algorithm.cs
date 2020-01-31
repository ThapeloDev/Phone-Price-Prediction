using Prediction.Models.Enums;
using Prediction.Models.Time_Series_Forecasting.Cleaning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction.Models.Time_Series_Forecasting.Business_Logic
{
    internal class Algorithm
    {
        public static void Calculate(PhoneCollection collection, Timeframe timeframe)
        {
            Preprocessing(collection);
            RunAlgorithm(collection, timeframe);
        }

        #region TimeSeries Private Methods
        private static void Preprocessing(PhoneCollection collection)
        {
            //NOTE: Order is important
            collection.Phones = DataDuplication.RemoveDuplicates(collection);
            collection.Phones = DataValidity.FillGaps(collection);
        }

        private static void RunAlgorithm(PhoneCollection collection, Timeframe timeframe)
        {
            collection.Phones = MovingAverage.Calculate(collection, timeframe);
            collection.Phones = CenteredMovingAverage.Calculate(collection, timeframe);
            collection.Phones = SeasonalIrregularity.Calculate(collection);
            collection.Phones = Seasonality.Calculate(collection, timeframe);
            collection.Phones = Deseasonalize.Calculate(collection);
            collection.Phones = Trend.Calculate(collection);
            collection.Phones = Forecast.Calculate(collection);
        }
        #endregion
    }
}
