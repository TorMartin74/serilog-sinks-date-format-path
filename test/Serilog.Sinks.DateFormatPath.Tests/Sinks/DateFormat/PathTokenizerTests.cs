using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Serilog.Sinks.DateFormat
{
    public class PathTokenizerTests
    {

        [Theory]
        [MemberData( nameof( Data ) )]
        public void Types( string path, PathTokenWrapper[] tokens )
        {
            var tokenizer = new PathTokenizer();
            var result = tokenizer.Tokenize( path ).Select( t => t.Type ).ToArray();
            var expected = tokens.Select( t => t.Type ).ToArray();

            Assert.Equal( expected, result );
        }

        [Theory]
        [MemberData( nameof( Data ) )]
        public void Values( string path, PathTokenWrapper[] tokens )
        {
            var tokenizer = new PathTokenizer();
            var result = tokenizer.Tokenize( path ).Select( t => t.Value ).ToArray();
            var expected = tokens.Select( t => t.Value );

            Assert.Equal( expected, result );
        }


        public static IEnumerable<object[]> Data = new[] {
            new object[] { @"", new PathToken[0] },

            new object[] { @"C:\test.log", new PathTokenWrapper[] {
                PathToken.Part( "C:" ) ,
                PathToken.Delimiter(),
                PathToken.Part( "test.log" )
            } },

            new object[] { @"C:\directory\test.log", new PathTokenWrapper[] {
                PathToken.Part( "C:" ) ,
                PathToken.Delimiter(),
                PathToken.Part( "directory" ) ,
                PathToken.Delimiter(),
                PathToken.Part( "test.log" )
            } },

            new object[] { @"C:\{expression}\test.log", new PathTokenWrapper[] {
                PathToken.Part( "C:" ) ,
                PathToken.Delimiter(),
                PathToken.Expression( "{expression}" ) ,
                PathToken.Delimiter(),
                PathToken.Part( "test.log" )
            } },

            new object[] { @"C:\dir-{expression}\test.log", new PathTokenWrapper[] {
                PathToken.Part( "C:" ) ,
                PathToken.Delimiter(),
                PathToken.Part( "dir-" ) ,
                PathToken.Expression( "{expression}" ) ,
                PathToken.Delimiter(),
                PathToken.Part( "test.log" )
            } },

            new object[] { @"C:\{expression}-dir\test.log", new PathTokenWrapper[] {
                PathToken.Part( "C:" ) ,
                PathToken.Delimiter(),
                PathToken.Expression( "{expression}" ) ,
                PathToken.Part( "-dir" ) ,
                PathToken.Delimiter(),
                PathToken.Part( "test.log" )
            } },

            new object[] { @"C:\dir-{expression}-dir\test.log", new PathTokenWrapper[] {
                PathToken.Part( "C:" ) ,
                PathToken.Delimiter(),
                PathToken.Part( "dir-" ) ,
                PathToken.Expression( "{expression}" ) ,
                PathToken.Part( "-dir" ) ,
                PathToken.Delimiter(),
                PathToken.Part( "test.log" )
            } }

        };

    }
}    