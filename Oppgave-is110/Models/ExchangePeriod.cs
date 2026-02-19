using System;

namespace UniversitySystem.Models
{
    public class ExchangePeriod
    {
        public DateTime From { get; private set; }
        public DateTime To { get; private set; }

        public ExchangePeriod(DateTime from, DateTime to)
        {
            SetPeriod(from, to);
        }

        public void SetPeriod(DateTime from, DateTime to)
        {
            if (to < from)
                throw new ArgumentException("Period end date cannot be earlier than start date.");

            From = from.Date;
            To = to.Date;
        }

        public override string ToString()
        {
            return $"{From:yyyy-MM-dd} to {To:yyyy-MM-dd}";
        }
    }
}
