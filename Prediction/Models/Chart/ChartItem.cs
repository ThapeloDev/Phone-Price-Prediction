using Prediction.Models.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction.Models.Chart
{
    public class ChartItem
    {
        public ChartItem(Brand brand, string model, List<double> priceHistory, List<DateTime> dates, Color backgroundColor, Color borderColor, bool fill, int borderWidth)
        {
            this.Label = brand.ToString() + " " + model;
            this.PriceHistory = priceHistory;
            this.Dates = dates;
            this.BackgroundColor = backgroundColor;
            this.BorderColor = borderColor;
            this.Fill = fill;
            this.BorderColor = borderColor;
        }

        public string Label { get; private set; }
        public List<double> PriceHistory { get; private set; }
        public List<DateTime> Dates { get; private set; }
        public Color BackgroundColor { get; private set; }
        public Color BorderColor { get; private set; }
        public bool Fill { get; private set; }
        public int BorderWidth { get; private set; }

        public class JSON
        {
            private readonly ChartItem chartItem;
            public JSON(ChartItem chartItem)
            {
                this.chartItem = chartItem;
            }

            public string Label()
            {
                return chartItem.Label;
            }

            public string PriceHistory()
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(chartItem.PriceHistory.ToList());
            }

            public string BackgroundColor()
            {
                return $"rgba({chartItem.BackgroundColor.R}, {chartItem.BackgroundColor.G}, {chartItem.BackgroundColor.B}, {chartItem.BackgroundColor.A})";
            }
            public string BorderColor()
            {
                return $"rgba({chartItem.BorderColor.R}, {chartItem.BorderColor.G}, {chartItem.BorderColor.B}, {chartItem.BorderColor.A})";
            }

            public bool Fill()
            {
                return chartItem.Fill;
            }

            public int BorderWidth()
            {
                return chartItem.BorderWidth;
            }

        }
    }
}
