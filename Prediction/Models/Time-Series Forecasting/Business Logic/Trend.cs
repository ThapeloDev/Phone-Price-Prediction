using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction.Models.Time_Series_Forecasting.Business_Logic
{
    internal class Trend
    {
        public static double Calculate(int index, PhoneCollection items)
        {
            double rSquared, yIntercept, slope;

            int xValueCount = items.Items
                              .Where(x => x.Deseasonalized.HasValue)
                              .Count();

            int[] xValues = Enumerable.Range(1, xValueCount).ToArray();
            double[] yValues = items.Items
                               .Where(x => x.Deseasonalized.HasValue)
                               .Select(x => x.Deseasonalized)
                               .Cast<double>()
                               .ToArray();

            LinearRegression.Calculate(xValues, yValues, out rSquared, out yIntercept, out slope);

            return CalculateTrend(index, yIntercept, slope);
        }

        private static double CalculateTrend(int index, double intercept, double slope)
        {
            return intercept + slope * (index + 1);
        }
    }
}
