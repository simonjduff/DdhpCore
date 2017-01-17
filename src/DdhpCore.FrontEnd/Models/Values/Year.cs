using System;

namespace DdhpCore.FrontEnd.Models.Values
{
    public struct Year
    {
        private readonly int _year;

        public Year(long year)
        {
            if (year < 2008 || year > 2050)
            {
                throw new ArgumentOutOfRangeException(nameof(year), year, "Year should be between 2008 and 2050");
            }

            _year = (int)year;
        }

        public static implicit operator Year(int year)
        {
            return new Year(year);
        }

        public static implicit operator Year(long year)
        {
            return new Year((int)year);
        }

        public static implicit operator string(Year year)
        {
            return year.ToString();
        }

        public static implicit operator Year(string year)
        {
            int intYear;
            if (!int.TryParse(year, out intYear))
            {
                throw new ArgumentOutOfRangeException(nameof(year), year, $"Cannot convert string year {year} to a Year");
            }

            return new Year(intYear);
        }

        public override string ToString()
        {
            return _year.ToString();
        }
    }
}