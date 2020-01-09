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
            this.phones = phones;
        }

        private List<Phone> phones;
        public List<Phone> Phones { get => phones; set => phones = value; }

        #region PhoneCollection Public Functions
        public void AddItem(Brand brand, string model, DateTime datetime)
        {
            phones.Add(new Phone(brand, model, datetime));
        }

        public void AddItem(Brand brand, string model, DateTime date, double? price)
        {
            phones.Add(new Phone(brand, model, date, price));
        }

        public bool ContainsDate(DateTime date, bool precise = false)
        {
            List<Phone> occurancesOfDate = new List<Phone>();

            // If precise is true, it checks for the same day, month and year.
            if (precise)
            {
                occurancesOfDate = phones
                                   .Where(i => i.Date.Day == date.Day)
                                   .Where(i => i.Date.Month == date.Month)
                                   .Where(i => i.Date.Year == date.Year)
                                   .ToList();
            }
            // If precise if false, it checks for same month and year, but not day.
            else
            {
                occurancesOfDate = phones
                                   .Where(i => i.Date.Month == date.Month)
                                   .Where(i => i.Date.Year == date.Year)
                                   .ToList();
            }

            if (occurancesOfDate.Count > 0)
                return true;
            else
                return false;
        }
        #endregion

        // TODO: SORTING

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
