using Prediction.Models.Enums;
using Prediction.Models.Time_Series_Forecasting;
using Prediction.Models.Time_Series_Forecasting.Business_Logic;
using Prediction.Models.Time_Series_Forecasting.Cleaning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction.Models
{
    public class TimeSeriesPrediction
    {
        public TimeSeriesPrediction(List<Item> items, Timeframe timeframe = Timeframe.Monthly)
        {
            List<Phone> phones = new List<Phone>();
            foreach(Item item in items)
                phones.Add(new Phone(item.Brand, item.Model, item.Date, item.Price));

            // Create a PhoneCollection from passed items.
            PhoneCollection = new PhoneCollection(phones);
            Algorithm.Calculate(PhoneCollection, timeframe);
        }

        public TimeSeriesPrediction(List<Item> items, Brand brand, string model, Timeframe timeframe = Timeframe.Monthly)
        {
            List<Phone> phones = new List<Phone>();
            foreach (Item item in items.Where(m => m.Brand == brand).Where(m => m.Model == model).ToList())
                phones.Add(new Phone(item.Brand, item.Model, item.Date, item.Price));

            PhoneCollection = new PhoneCollection(phones);
            Algorithm.Calculate(PhoneCollection, timeframe);
        }

        private PhoneCollection phoneCollection;
        public PhoneCollection PhoneCollection { get => phoneCollection; set => phoneCollection = value; }

        public void GenerateFutureForecast(int months = 12, Timeframe timeframe = Timeframe.Monthly)
        {
            // Generates required amount of items
            for(int i = 0; i < months; i++)
            {
                // Note: Data has already been pre-processed, no need for additional checks.
                // Same brand, same model, increased date by one month from latest date
                phoneCollection.AddItem(PhoneCollection.Phones[0].Brand,
                                        PhoneCollection.Phones[0].Model,
                                        PhoneCollection.Phones[PhoneCollection.Phones.Count() - 1].Date.AddMonths(1));
            }

            // Calculates expected forecast for all items.
            for (int index = 0; index < PhoneCollection.Phones.Count; index++)
            {
                if (PhoneCollection.Phones[index].Seasonality == null)
                {
                    PhoneCollection.Phones[index].Seasonality = Seasonality.Calculate(index, PhoneCollection, timeframe);
                    PhoneCollection.Phones[index].Trend = Trend.Calculate(index, PhoneCollection);
                    PhoneCollection.Phones[index].Forecast = Forecast.Calculate(index, PhoneCollection);
                }
            }
        }
       
    }
}
