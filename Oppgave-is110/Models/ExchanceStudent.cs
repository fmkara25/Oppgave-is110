using System;

namespace UniversitySystem.Models
{
    public class ExchangeStudent : Student
    {
        public string HomeUniversity { get; private set; } = string.Empty;
        public string Country { get; private set; } = string.Empty;
        public ExchangePeriod Period { get; private set; } = null!;

        public ExchangeStudent(
            string studentId,
            string name,
            string email,
            string homeUniversity,
            string country,
            ExchangePeriod period)
            : base(studentId, name, email)
        {
            SetHomeUniversity(homeUniversity);
            SetCountry(country);
            SetPeriod(period);
        }

        public void SetHomeUniversity(string homeUniversity)
        {
            if (string.IsNullOrWhiteSpace(homeUniversity))
                throw new ArgumentException("HomeUniversity cannot be empty.");

            HomeUniversity = homeUniversity.Trim();
        }

        public void SetCountry(string country)
        {
            if (string.IsNullOrWhiteSpace(country))
                throw new ArgumentException("Country cannot be empty.");

            Country = country.Trim();
        }

        public void SetPeriod(ExchangePeriod period)
        {
            Period = period ?? throw new ArgumentNullException(nameof(period));
        }

        public override string ToString()
        {
            return $"Exchange {base.ToString()} - {HomeUniversity}, {Country} - {Period}";
        }
    }
}
