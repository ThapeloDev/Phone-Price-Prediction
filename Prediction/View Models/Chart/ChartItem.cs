using System.Collections.Generic;

namespace Prediction.Models.NewChart
{
    public class ChartItem
    {
        public string Label { get; set; }
        public bool Fill { get; set; }
        public int BorderWidth { get; set; }
        public List<ChartTransaction> LstData { get; set; }
    }
}
