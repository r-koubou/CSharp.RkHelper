using System;

namespace RkHelper.Time
{
    public static class DateTimeHelper
    {
        public static DateTime NowUtc()
            => TimeZoneInfo.ConvertTimeToUtc( DateTime.Now );
        public static DateTime ToUtc( DateTime dateTime )
            => TimeZoneInfo.ConvertTimeToUtc( dateTime );
        public static DateTime UtcToLocalTime( DateTime utcDateTime )
            => TimeZoneInfo.ConvertTimeFromUtc( utcDateTime, TimeZoneInfo.Local );
    }
}