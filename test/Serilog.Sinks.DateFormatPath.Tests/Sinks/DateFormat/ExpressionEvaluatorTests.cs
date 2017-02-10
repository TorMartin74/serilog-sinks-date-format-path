using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Serilog.Sinks.DateFormat
{
    public class ExpressionEvaluatorTests
    {
        public static IEnumerable<string[]> Data
        {
            get
            {
                yield return new[] { "{date:format=yyyy}", "2017" };
                yield return new[] { "{date:format=MM}", "02" };
                yield return new[] { "{date:format=dd}", "05" };
                yield return new[] { "{date:format=HH}", "16" };
                yield return new[] { @"{date:format=HH\:mm\:ss}", "16:30:23" };
                yield return new[] { @"{date:format=HH\:mm\:ss:culture=nb-NO}", "16:30:23" };
            }
        }


        [Theory]
        [MemberData( nameof( Data ) )]
        public void TestExpressionEvaluator( string expression, string expected )
        {
            // Arrange
            var dt = new DateTime( 2017, 2, 5, 16, 30, 23, 20 );
            Clock.SetTimeProvider( () => dt );


            ExpressionEvaluator evaluator = new ExpressionEvaluator();


            // Act
            string result = evaluator.Evaluate( expression, dt );


            // Assert
            Assert.Equal( expected, result );
        }
    }
}
