using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction.Models.Chart
{
    public class StackedViewModel
    {
        public string StackedDimensionOne { get; set; }
        public List<SimpleReportViewModel> LstData { get; set; }
    }
}
