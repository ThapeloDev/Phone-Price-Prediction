using Prediction.Models.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction.Models.Time_Series_Forecasting
{
    public class Phone : IEnumerable<Phone>
    {
        public Phone(Brand brand,
                     string model,
                     DateTime? date,
                     double? price = null,
                     double? movingAverage = null,
                     double? centeredMovingAverage = null,
                     double? seasonalIrregularity = null,
                     double? seasonality = null,
                     double? deseasonalized = null,
                     double? trend = null,
                     double? forecast = null)
        {
            this.Brand = brand;
            this.Model = model;
            this.Date = date;
            this.Price = price;
            this.MovingAverage = movingAverage;
            this.CenteredMovingAverage = centeredMovingAverage;
            this.SeasonalIrregularity = seasonalIrregularity;
            this.Seasonality = seasonality;
            this.Deseasonalized = deseasonalized;
            this.Trend = trend;
            this.Forecast = forecast;
        }

        public Phone(Item item)
        {
            this.Item = item;
            this.Brand = Item.Brand;
            this.Model = Item.Model;
            this.Date = Item.Date;
            this.Price = Item.Price;
        }

        private Item item = null;
        private Brand brand;
        private string model;
        private DateTime? date;
        private double? price;
        private double? movingAverage;
        private double? centeredMovingAverage;
        private double? seasonalIrregularity;
        private double? seasonality;
        private double? deseasonalized;
        private double? trend;
        private double? forecast;

        public Item Item { get => item; set => item = value; }
        public DateTime? Date { get => date; set => date = value; }
        public double? Price { get => price; set => price = value; }
        public Brand Brand { get => brand; set => brand = value; }
        public string Model { get => model; set => model = value; }
        public double? MovingAverage { get => movingAverage; set => movingAverage = value; }
        public double? CenteredMovingAverage { get => centeredMovingAverage; set => centeredMovingAverage = value; }
        public double? SeasonalIrregularity { get => seasonalIrregularity; set => seasonalIrregularity = value; }
        public double? Seasonality { get => seasonality; set => seasonality = value; }
        public double? Deseasonalized { get => deseasonalized; set => deseasonalized = value; }
        public double? Trend { get => trend; set => trend = value; }
        public double? Forecast { get => forecast; set => forecast = value; }

        public IEnumerator<Phone> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
