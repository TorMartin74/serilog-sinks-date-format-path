using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serilog.Sinks.DateFormat
{
    public class FileRoller
    {
        PathToken[] _tokenizedPath;
        ExpressionEvaluator _evaluator;
        TimeFormatAnalyzer.FormatComponent _dtComponent;

        DateTimeOffset? _snapshot = null;
        string _path;

        public FileRoller( string pathFormat )
        {
            var tokenizer = new PathTokenizer();
            _tokenizedPath = tokenizer.Tokenize( pathFormat ).ToArray();

            // Get lesser time part
            var formats = _tokenizedPath
                .Where( t => t.Type == PathTokenType.Expression )
                .Select( e => e.Value );

            _dtComponent = TimeFormatAnalyzer.GetMinInterval( formats );

            _evaluator = new ExpressionEvaluator();
        }


        public string GetLogFilePath( DateTimeOffset now )
        {
            if( _path == null || now > _snapshot )
            {
                _path = CreatePath( now );
                _snapshot = CreateNextOffset( _dtComponent, now );
            }

            return _path;
        }


        private DateTimeOffset CreateNextOffset(TimeFormatAnalyzer.FormatComponent fc, DateTimeOffset now )
        {
            switch( fc )
            {
                case TimeFormatAnalyzer.FormatComponent.Year:
                    return new DateTimeOffset( now.Year, 1, 1, 0, 0, 0, now.Offset ).AddYears(1);

                case TimeFormatAnalyzer.FormatComponent.Month:
                    return new DateTimeOffset( now.Year, now.Month, 1, 0, 0, 0, now.Offset ).AddMonths( 1 );

                case TimeFormatAnalyzer.FormatComponent.Day:
                    return new DateTimeOffset( now.Year, now.Month, now.Day, 0, 0, 0, now.Offset ).AddDays( 1 );

                case TimeFormatAnalyzer.FormatComponent.Hour:
                    return new DateTimeOffset( now.Year, now.Month, now.Day, now.Hour, 0, 0, now.Offset ).AddHours( 1 );

                case TimeFormatAnalyzer.FormatComponent.Minute:
                    return new DateTimeOffset( now.Year, now.Month, now.Day, now.Hour, now.Minute, 0, now.Offset ).AddMinutes( 1 );

                default:
                    throw new InvalidOperationException("Unknown Date Format Component");
            }


        }


        private string CreatePath( DateTimeOffset now )
        {
            var sb = new StringBuilder();
            foreach( var token in _tokenizedPath )
            {
                if( token.Type == PathTokenType.Delimiter )
                    sb.Append( '\\' );

                else if( token.Type == PathTokenType.Part )
                    sb.Append( token.Value );

                else if( token.Type == PathTokenType.Expression )
                {
                    var value = _evaluator.Evaluate( token.Value, now );
                    sb.Append( value );
                }
            }

            return sb.ToString();
        }
    }
}
