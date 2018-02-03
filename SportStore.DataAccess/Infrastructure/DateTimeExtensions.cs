namespace SportStore.DataAccess.Infrastructure
{
    using System;

    public static class DateTimeExtensions
    {
        public static long GetTotalSeconds(this DateTime date)
        {
            return date.Ticks / TimeSpan.TicksPerSecond;
        }
    }
}
