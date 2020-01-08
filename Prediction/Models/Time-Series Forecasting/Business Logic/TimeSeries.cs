using Prediction.Models.Enums;
using Prediction.Models.Time_Series_Forecasting.Cleaning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction.Models.Time_Series_Forecasting.Business_Logic
{
    internal class TimeSeries
    {
        public static void Calculate(PhoneCollection phoneCollection, Timeframe timeframe)
        {
            PreprocessFillMissingItems(phoneCollection);

            GenerateMovingAverage(phoneCollection, timeframe);
            GenerateCenteredMovingAverage(phoneCollection, timeframe);
            GenerateSeasonalIrregularity(phoneCollection);
            GenerateSeasonality(phoneCollection, timeframe);
            GenerateDeseasonalizedValues(phoneCollection);
            GenerateTrend(phoneCollection);
            GenerateForecast(phoneCollection);
        }

        private static void PreprocessFillMissingItems(PhoneCollection phoneCollection)
        {
            phoneCollection.Items = DataValidity.FillGaps(phoneCollection);
        }

        private static void GenerateMovingAverage(PhoneCollection phoneCollection, Timeframe timeframe)
        {
            for (int index = 0; index < phoneCollection.Items.Count; index++)
            {
                phoneCollection.Items[index].MovingAverage = MovingAverage.Calculate(index, phoneCollection, timeframe);
            }
        }

        private static void GenerateCenteredMovingAverage(PhoneCollection phoneCollection, Timeframe timeframe)
        {
            for (int index = 0; index < phoneCollection.Items.Count; index++)
            {
                phoneCollection.Items[index].CenteredMovingAverage = CenteredMovingAverage.Calculate(index, phoneCollection, timeframe);
            }
        }

        private static void GenerateSeasonalIrregularity(PhoneCollection phoneCollection)
        {
            for (int index = 0; index < phoneCollection.Items.Count; index++)
            {
                phoneCollection.Items[index].SeasonalIrregularity = SeasonalIrregularity.Calculate(index, phoneCollection);
            }
        }

        private static void GenerateSeasonality(PhoneCollection phoneCollection, Timeframe timeframe)
        {
            for (int index = 0; index < phoneCollection.Items.Count; index++)
            {
                phoneCollection.Items[index].Seasonality = Seasonality.Calculate(index, phoneCollection, timeframe);
            }
        }

        private static void GenerateDeseasonalizedValues(PhoneCollection phoneCollection)
        {
            for (int index = 0; index < phoneCollection.Items.Count; index++)
            {
                phoneCollection.Items[index].Deseasonalized = Deseasonalize.Calculate(index, phoneCollection);
            }
        }

        private static void GenerateTrend(PhoneCollection phoneCollection)
        {
            for (int index = 0; index < phoneCollection.Items.Count; index++)
            {
                phoneCollection.Items[index].Trend = Trend.Calculate(index, phoneCollection);
            }
        }

        private static void GenerateForecast(PhoneCollection phoneCollection)
        {
            for (int index = 0; index < phoneCollection.Items.Count; index++)
            {
                phoneCollection.Items[index].Forecast = Forecast.Calculate(index, phoneCollection);
            }
        }
    }
}
