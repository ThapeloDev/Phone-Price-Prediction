using Prediction.Models.NewChart;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction.View_Models.Chart.Misc
{
    static class ForecastRecord
    {
        public static Dictionary<int, List<ChartTransaction>> _dict = new Dictionary<int, List<ChartTransaction>>();
    }


}
