using Prediction.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Prediction.Models.Time_Series_Forecasting
{
    public class Item
    {
        [Key]
        public int ItemId { get; set; }
        public Brand Brand { get; set; }
        public string Model { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public double Price { get; set; }
    }
}
