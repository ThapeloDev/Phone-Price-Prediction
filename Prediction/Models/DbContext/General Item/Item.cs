using Prediction.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
