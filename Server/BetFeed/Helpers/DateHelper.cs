using System;

namespace BetFeed.Helpers
{
    public static class DateHelper
    {
        public static bool IsWithin24Hours(DateTime date)
        {
            var now = DateTime.Now;
            var tomorrow = DateTime.Now.AddDays(1);

            if(date < now || date > tomorrow)
            {
                return false;
            }

            return true;
        }
    }
}