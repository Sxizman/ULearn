using System;

namespace AccountingModelTask
{
    public class AccountingModel
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
                if (value < 0 || value > 100)
                    throw new ArgumentException("Discount must be in percentage range 0-100");
                _discount = value;
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
                if (value < 0 && value > _price * _nightsCount)
                    throw new ArgumentException("Impossible total price value");
                _discount = (1 - value / (_price * _nightsCount)) * 100;
            }
        }
    }
}
