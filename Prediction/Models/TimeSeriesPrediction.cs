using Prediction.Models.Enums;
using Prediction.Models.Time_Series_Forecasting;
using Prediction.Models.Time_Series_Forecasting.Business_Logic;
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
                phones.Add(new Phone(item));

            // Create a PhoneCollection from passed items.
            PhoneCollection = new PhoneCollection(phones);

            if (checkForBrandConsistency(PhoneCollection) && checkForModelConsistency(PhoneCollection))
            {
                /*
                 * Calculate:
                 *    - Moving Average
                 *    - Centered Moving Average
                 *    - Seasonal Irregularity
                 *    - Seasonality
                 *    - Deseasonalized Values
                 *    - Trend
                 *    - Forecast
                 */
                TimeSeries.Calculate(PhoneCollection, timeframe);
            }
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


            for (int index = 0; index < PhoneCollection.Items.Count; index++)
            {
                if (PhoneCollection.Items[index].Seasonality == null)
                {
                    PhoneCollection.Items[index].Seasonality = Seasonality.Calculate(index, PhoneCollection, timeframe);
                    PhoneCollection.Items[index].Trend = Trend.Calculate(index, PhoneCollection);
                    PhoneCollection.Items[index].Forecast = Forecast.Calculate(index, PhoneCollection);
                }
            }
        }
        
        private bool checkForBrandConsistency(PhoneCollection phoneCollection)
        {
            Brand brand;

            // Check if collection contains items
            if (phoneCollection.Items.ElementAtOrDefault(0) != null)
                // Get brand for first item in PhoneCollection.
                brand = phoneCollection.Items[0].Brand;
            else
                throw new Exception("The passed collection contains no items.");

            foreach (Phone phone in phoneCollection.Items)
            {
                // Check if all phones contain the same brand
                if (phone.Brand != brand)
                    return false;
            }

            return true;
        }

        private bool checkForModelConsistency(PhoneCollection phoneCollection)
        {
            string model;

            // Check if collection contains items
            if (phoneCollection.Items.ElementAtOrDefault(0) != null)
                // Get brand for first item in PhoneCollection.
                model = phoneCollection.Items[0].Model;
            else
                throw new Exception("The passed collection contains no items.");

            foreach (Phone phone in phoneCollection.Items)
            {
                // Check if all phones contain the same model
                if (phone.Model != model)
                    return false;
            }

            return true;
        }

        public string Print()
        {
            string output = "";
            int index = 0;
            foreach (Phone p in PhoneCollection.Items)
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
