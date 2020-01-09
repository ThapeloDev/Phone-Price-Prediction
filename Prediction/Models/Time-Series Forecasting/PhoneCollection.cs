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

        #region Sorting
        public List<Phone> Sort(Enums.Attribute sort, Direction? direction = null)
        {
            List<Phone> temp = new List<Phone>();
            
            if (sort == Enums.Attribute.Brand)
            {
                temp = phones.OrderBy(i => i.Brand).ToList();
            }
            else if (sort == Enums.Attribute.Date)
            {
                temp = phones.OrderBy(i => i.Date).ToList();
            }
            else if (sort == Enums.Attribute.Forecast)
            {
                temp = phones.OrderBy(i => i.Forecast).ToList();
            }
            else if (sort == Enums.Attribute.Price)
            {
                temp = phones.OrderBy(i => i.Price).ToList();
            }

            if (direction.HasValue)
            {
                if (direction == Enums.Direction.Descending)
                {
                    temp.OrderByDescending(i => i);
                }
                else if (direction == Enums.Direction.Ascending)
                {
                    temp.OrderBy(i => i);
                }
            }
            
            return temp;
        }
        #endregion

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
