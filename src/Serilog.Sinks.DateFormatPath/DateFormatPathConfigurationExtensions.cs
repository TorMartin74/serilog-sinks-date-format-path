using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Display;
using Serilog.Sinks.DateFormat;

namespace Serilog
{
    public static class DateFormatPathConfigurationExtensions
    {
        const string DefaultOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}";
        const long DefaultFileSizeLimitBytes = 1L * 1024 * 1024 * 1024;
        const int DefaultRetainedFileCountLimit = 31; // A long month of logs



        public static LoggerConfiguration DateFormatPath(
            this LoggerSinkConfiguration sinkConfiguratio,
            string pathFormat,
            LogEventLevel restrictedMinimumLevel = LevelAlias.Minimum,
            string outputTemplate = DefaultOutputTemplate,
            IFormatProvider formatProvider = null,
            long? fileSizeLimitBytes = DefaultFileSizeLimitBytes,
            int? retainedFileCountLimit = DefaultRetainedFileCountLimit,
            LoggingLevelSwitch levelSwitch = null,
            bool buffered = false,
            bool shared = true,
            TimeSpan? flushToDiskInterval = null )
        {
            var formatter = new MessageTemplateTextFormatter( outputTemplate, formatProvider );
            var sink = new DateFormatSink( pathFormat, formatter, null, buffered, shared);

            return sinkConfiguratio.Sink( sink, restrictedMinimumLevel, levelSwitch );
        }

    }
}
