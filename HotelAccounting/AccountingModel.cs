using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelAccounting
{
    public class AccountingModel : ModelBase
    {
        private double _price;
        private int _nightsCount;
        private double _discount;

        public double Price
        {
            get
            {
                return _price;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Price must be non-negative");
                _price = value;

                Notify(nameof(Price));
                Notify(nameof(Total));
            }
        }

        public int NightsCount
        {
            get
            {
                return _nightsCount;
            }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Count must be positive");
                _nightsCount = value;

                Notify(nameof(NightsCount));
                Notify(nameof(Total));
            }
        }

        public double Discount
        {
            get
            {
                return _discount;
            }
            set
            {
                if (value > 100)
                    throw new ArgumentException("Discount cannot be greater then 100%");
                _discount = value;

                Notify(nameof(Discount));
                Notify(nameof(Total));
            }
        }

        public double Total
        {
            get
            {
                return (_price * _nightsCount) * (1 - _discount / 100);
            }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Total pirce must be non-negative");

                if (_price * _nightsCount == 0)
                {
                    if (value != 0)
                        throw new ArgumentException("Impossible total price value");
                    Discount = 0;
                }
                else
                {
                    Discount = (1 - value / (_price * _nightsCount)) * 100;
                }
            }
        }
    }
}