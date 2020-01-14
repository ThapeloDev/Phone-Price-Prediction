using Prediction.Models.Chart.Misc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction.Models.Chart
{
    public class ChartItemModel
    {
        public string Label { get; set; }
        public RGBAColor BackgroundColor { get; set; }
        public RGBAColor BorderColor { get; set; }
        public bool Fill { get; set; }
        public int BorderWidth { get; set; }
        public List<ChartTransactionModel> LstData { get; set; }
    }
}
