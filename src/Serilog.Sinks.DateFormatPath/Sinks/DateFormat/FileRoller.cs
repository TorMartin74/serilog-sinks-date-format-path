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

        DateTime _snapshot;
        ExpressionEvaluator _evaluator;

        public FileRoller( string pathFormat )
        {
            var tokenizer = new PathTokenizer();
            _tokenizedPath = tokenizer.Tokenize( pathFormat ).ToArray();

            _evaluator = new ExpressionEvaluator();

        }


        public string GetLogFilePath()
        {
            StringBuilder sb = new StringBuilder();
            foreach( var token in _tokenizedPath )
            {
                if( token.Type == PathTokenType.Delimiter )
                    sb.Append( '\\' );

                else if( token.Type == PathTokenType.Part )
                    sb.Append( token.Value );

                else if( token.Type == PathTokenType.Expression )
                {
                    var value = _evaluator.Evaluate( token.Value );
                    sb.Append( value );
                }
            }

            return sb.ToString();
        }
    }
}
