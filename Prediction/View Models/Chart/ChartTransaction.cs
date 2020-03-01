using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction.Models.NewChart
{
    public class ChartTransaction : IEnumerable
    {
        public DateTime Date { get; set; }
        public double Price { get; set; }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
