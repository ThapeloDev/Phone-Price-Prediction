using Prediction.Models.NewChart;
using System.Collections.Generic;

namespace Prediction.View_Models.Chart.Misc
{
    static class ForecastRecord
    {
        public static Dictionary<int, List<ChartTransaction>> _dict = new Dictionary<int, List<ChartTransaction>>();
    }


}
