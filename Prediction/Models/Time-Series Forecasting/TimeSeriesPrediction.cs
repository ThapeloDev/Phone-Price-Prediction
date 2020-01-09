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
        public TimeSeriesPrediction(List<Item> items, Timeframe timeframe)
        {
            List<Phone> phones = new List<Phone>(); ;
            foreach(Item item in items)
                phones.Add(new Phone(item.Brand, item.Model, item.Date, item.Price));

            // Create a PhoneCollection from passed items.
            PhoneCollection = new PhoneCollection(phones);
            Algorithm.Calculate(PhoneCollection, timeframe);
        }

        private PhoneCollection phoneCollection;
        public PhoneCollection PhoneCollection { get => phoneCollection; set => phoneCollection = value; }

        public void GenerateFutureForecast(PhoneCollection phoneCollection, 
                                           int months = 12,
                                           Timeframe timeframe = Timeframe.Monthly)
        {
            if (phoneCollection is null)
            {
                throw new ArgumentNullException(nameof(phoneCollection));
            }

            PhoneCollection.AddItem(Brand.iPhone, "8", new DateTime(2005, 1, 1));
            PhoneCollection.AddItem(Brand.iPhone, "8", new DateTime(2005, 2, 1));
            PhoneCollection.AddItem(Brand.iPhone, "8", new DateTime(2005, 3, 1));
            PhoneCollection.AddItem(Brand.iPhone, "8", new DateTime(2005, 4, 1));
            PhoneCollection.AddItem(Brand.iPhone, "8", new DateTime(2005, 5, 1));
            PhoneCollection.AddItem(Brand.iPhone, "8", new DateTime(2005, 6, 1));
            PhoneCollection.AddItem(Brand.iPhone, "8", new DateTime(2005, 7, 1));
            PhoneCollection.AddItem(Brand.iPhone, "8", new DateTime(2005, 8, 1));
            PhoneCollection.AddItem(Brand.iPhone, "8", new DateTime(2005, 9, 1));
            PhoneCollection.AddItem(Brand.iPhone, "8", new DateTime(2005, 10, 1));
            PhoneCollection.AddItem(Brand.iPhone, "8", new DateTime(2005, 11, 1));
            PhoneCollection.AddItem(Brand.iPhone, "8", new DateTime(2005, 12, 1));


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
        

        public string Print()
        {
            string output = "";
            int index = 0;
            foreach (Phone p in PhoneCollection.Phones)
            {
                index++;
                if (p.Forecast != null)
                {
                    output += $"[{index}] {p.Forecast}";
                }
            }
            return output;
        }
    }
}
