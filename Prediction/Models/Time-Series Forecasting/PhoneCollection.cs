using Prediction.Models.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction.Models.Time_Series_Forecasting
{
    public class PhoneCollection : IEnumerable<PhoneCollection>
    {
        public PhoneCollection(List<Phone> phones)
        {
            this.Items = phones;
        }

        private List<Phone> items;
        public List<Phone> Items { get => items; set => items = value; }

        public void AddItem(Brand brand, string model, DateTime datetime)
        {
            Phone newPhone = new Phone(brand, model, datetime);
            Items.Add(newPhone);
        }

        public IEnumerator<PhoneCollection> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
