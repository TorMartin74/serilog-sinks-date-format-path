using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serilog.Sinks.DateFormat
{
    public class PathTokenizer
    {
        const char Delimiter = '\\';


        public PathTokenizer()
        {
        }

        public IEnumerable<PathToken> Tokenize( string path )
        {
            if( path.EndsWith( Delimiter.ToString() ) == true )
                throw new ArgumentException( "Filename not provided in path.", nameof( path ) );

            if( string.IsNullOrWhiteSpace( path ) )
                yield break;

        
            var cTokenType = PathTokenType.Part;

            var sStack = new Stack<string>();
            var cString = new StringBuilder();



            for(int i = 0; i < path.Length; i++  )
            {
                var ch = path[ i ];

                if( ch == '\\' )
                {
                    if( sStack.Any() == true )
                    {
                        var sb = sStack.ConcatToStringBuilder();

                        sb.Append( cString.ToString() );
                        cString.Clear();

                        yield return PathToken.Part( sb.ToString() );
                    }


                    if( cString.Length > 0 )
                    {
                        yield return PathToken.Part( cString.ToString() );
                        cString.Clear();
                    }

                    yield return PathToken.Delimiter();
                    continue;
                }

                else if( ch == '{' )
                {
                    if( cString.Length > 0 )
                    {
                        sStack.Push( cString.ToString() );
                        cString.Clear();
                    }

                    cTokenType = PathTokenType.Expression;
                    cString.Append( ch );

                    continue;
                }

                else if( ch == '}' && cTokenType == PathTokenType.Expression )
                {
                    if( sStack.Any() == true )
                    {
                        var sb = sStack.ConcatToStringBuilder();

                        yield return PathToken.Part( sb.ToString() );
                    }


                    cString.Append( ch );
                    yield return PathToken.Expression( cString.ToString() );
                    cString.Clear();

                    cTokenType = PathTokenType.Part;

                    continue;
                }

                cString.Append( ch );


                // yeald what we have if we are at the last character
                if( i == path.Length - 1 )
                {
                    var sb = sStack.ConcatToStringBuilder();

                    sb.Append( cString.ToString() );

                    yield return PathToken.Part( sb.ToString() );
                }
            }

        }

    }

    internal static class PathTokenExtensions
    {
        public static IEnumerable<PathToken> Compress( this IEnumerable<PathToken> tokens )
        {
            PathToken currentToken = null;

            foreach( var pathToken in tokens )
            {
                if( pathToken.Type == PathTokenType.Expression )
                {
                    yield return currentToken;
                    yield return pathToken;

                    currentToken = null;

                    continue;
                }

                if( currentToken == null )
                    currentToken = new PathToken( PathTokenType.Part, pathToken.Value );
                else
                    currentToken.Append( pathToken.Value );
            }
        }

    }
}
