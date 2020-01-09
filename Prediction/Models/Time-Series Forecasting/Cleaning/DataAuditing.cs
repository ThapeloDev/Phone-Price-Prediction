using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction.Models.Time_Series_Forecasting.Cleaning
{
    // The data is audited with the use of statistical and database methods to detect anomalies and contradictions.
    public class DataAuditing
    {
        public static bool HasConsistentBranding(List<Phone> list)
        {
            var brand = list.First().Brand;
            var model = list.First().Model;

            if (list.All(x => x.Brand == brand) && list.All(x => x.Model == model))
                return true;
            else
                return false;
        }
    }
}
