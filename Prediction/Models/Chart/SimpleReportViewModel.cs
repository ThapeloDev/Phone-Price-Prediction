using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction.Models.Chart
{
    public class SimpleReportViewModel : IEnumerable
    {
        public string DimensionOne { get; set; }
        public double Quantity { get; set; }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
