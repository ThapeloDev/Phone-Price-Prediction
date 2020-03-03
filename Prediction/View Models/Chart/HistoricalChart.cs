using Prediction.Models.Enums;
using Prediction.Models.Hardware;
using Prediction.Models.NewChart;
using Prediction.Models.Time_Series_Forecasting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction.View_Models.Chart
{
    public class HistoricalChart
    {
        public HistoricalChart(int id, List<Item> phones, List<PhoneProperties> hardware )
        {
            this.Id = id;
            this.Phones = phones;
            this.Hardware = hardware;
            this.Specs = Hardware.FirstOrDefault(x => x.ConfigId == Id);
        }

        public List<PhoneProperties> Hardware { get; }
        public List<Item> Phones { get; }
        public int Id { get; }
        public ChartItem ChartItem { get; set; } = new ChartItem();
        public PhoneProperties Specs { get; }

        public Tuple<Brand, string> GetPhoneInfo()
        {
            Brand brand = Hardware.Where(x => x.ConfigId == Id).Select(x => x.Brand).FirstOrDefault();
            string model = Hardware.Where(x => x.ConfigId == Id).Select(x => x.Model).FirstOrDefault();
            return new Tuple<Brand, string>(brand, model);
        }

        public ChartItem DrawChart()
        {
            ChartItem chart = new ChartItem();

            var currentBrand = Hardware.FirstOrDefault(m => m.ConfigId == Id).Brand;
            var currentModel = Hardware.FirstOrDefault(m => m.ConfigId == Id).Model;

            chart.Label = $"{currentBrand.ToString()} {currentModel}";
            chart.Fill = false;
            chart.BorderWidth = 1;
            chart.LstData = GetPriceData(currentBrand, currentModel);

            return chart;
        }

        private List<ChartTransaction> GetPriceData(Brand brand, string model)
        {
            List<ChartTransaction> transactions = new List<ChartTransaction>();
            var allTransactions = Phones.Where(x => x.Brand == brand && x.Model == model).ToList();

            foreach(Item item in allTransactions)
            {
                transactions.Add(new ChartTransaction
                {
                    Date = item.Date,
                    Price = item.Price
                });
            }
            
            return transactions;
        }
    }
}
