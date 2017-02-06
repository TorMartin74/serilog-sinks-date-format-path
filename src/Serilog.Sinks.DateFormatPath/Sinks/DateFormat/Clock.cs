using System;
using System.Linq;

namespace Serilog.Sinks.DateFormat
{
    public static  class Clock
    {
        static Func<DateTime> _dateTimeNow = () => DateTime.Now;

        public static DateTime DateTimeNow => _dateTimeNow();

        public static void SetTimeProvider( Func<DateTime> dateTimeNow )
        {
            _dateTimeNow = dateTimeNow;
        }
    }
}
