using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction.Models.Time_Series_Forecasting.Business_Logic
{
    internal class Trend
    {
        public static List<Phone> Calculate(PhoneCollection collection)
        {
            for(int index = 0; index < collection.Phones.Count(); index++)
            {
                collection.Phones[index].Trend = Calculate(index, collection);
            }
            return collection.Phones;
        }

        public static double Calculate(int index, PhoneCollection collection)
        {
            int xValueCount = collection.Phones
                              .Where(x => x.Deseasonalized.HasValue)
                              .Count();

            int[] xValues = Enumerable
                            .Range(1, xValueCount)
                            .ToArray();
            double[] yValues = collection.Phones
                               .Where(x => x.Deseasonalized.HasValue)
                               .Select(x => x.Deseasonalized)
                               .Cast<double>()
                               .ToArray();

            LinearRegression.Calculate(xValues, yValues, out double rSquared, out double yIntercept, out double slope);

            return CalculateTrend(index, yIntercept, slope);
        }

        private static double CalculateTrend(int index, double intercept, double slope)
        {
            return intercept + slope * (index + 1);
        }
    }
}
