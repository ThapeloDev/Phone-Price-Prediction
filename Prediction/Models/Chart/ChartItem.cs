using Prediction.Models.Chart.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction.Models.NewChart
{
    public class ChartItem
    {
        public string Label { get; set; }
        public RGBAColor BackgroundColor { get; set; }
        public RGBAColor BorderColor { get; set; }
        public bool Fill { get; set; }
        public int BorderWidth { get; set; }
        public List<ChartTransaction> LstData { get; set; }
    }
}
