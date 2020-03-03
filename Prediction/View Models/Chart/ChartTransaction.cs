using System;
using System.Collections;

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
