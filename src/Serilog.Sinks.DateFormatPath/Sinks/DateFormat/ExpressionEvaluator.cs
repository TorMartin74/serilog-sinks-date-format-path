using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Serilog.Sinks.DateFormat
{
    public class ExpressionEvaluator
    {


        public ExpressionEvaluator()
        {

        }

        public string Evaluate( string expression, DateTimeOffset now )
        {
            expression = CleanExpression( expression );

            if( expression.StartsWith( "date" ) == false )
                throw new ArgumentException( "Only date is supported at the moment." );

            string format = FindFormat( expression ) ?? "";
            bool isUtc = FindUtcParameter( expression );
            string culture = FindCulture( expression );

            if( isUtc )
                now = now.ToUniversalTime();

            var ci = culture != null ? new CultureInfo( culture ) : null;
            return now.ToString( format, ci );
        }

        public string GetExpressionFormat( string expression )
        {
            expression = CleanExpression( expression );

            var exp = expression.Replace( "\\:", "{@}" );
            var format = FindFormat( expression ) ?? "";



            return format.Replace( "{@}", "\\:" );
        }


        private static string CleanExpression( string expression )
        {
            expression = expression.Trim();

            if( expression.StartsWith( "{" ) )
                expression = expression.Substring( 1 );

            if( expression.EndsWith( "}" ) )
                expression = expression.Substring( 0, expression.Length - 1 );
            return expression;
        }


        string FindFormat( string expression )
        {
            var exp = expression.Replace( "\\:", "{@}" );

            var argument = GetParameter( exp, "format" );
            if( argument == null )
                return null;

            return argument.Replace( "{@}", "\\:" );
        }


        bool FindUtcParameter( string expression )
        {
            var argument = GetParameter( expression, "isUtc" );
            if( argument == null )
                return false;


            bool b = false;
            if( bool.TryParse( argument, out b ) == false )
                return false;

            return b;
        }


        string FindCulture( string expression )
        {
            var argument = GetParameter( expression, "culture" );
            if( argument == null )
                return null;

            return argument;
        }


        string GetParameter( string exp, string argument )
        {
            var pos = exp.IndexOf( argument );
            if( pos < 0 )
                return null;


            var eqPos = exp.IndexOf( '=', pos );
            if( eqPos < 0 )
                return null;

            pos = exp.IndexOf( ':', eqPos );
            eqPos += 1;

            if( pos >= 0 )
                return exp.Substring( eqPos, pos - eqPos ).Trim();
            else
                return exp.Substring( eqPos ).Trim();
        }
    }
}
