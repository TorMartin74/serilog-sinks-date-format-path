using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;


namespace Serilog.Sinks.DateFormat
{
    public class FileRollerTests
    {
        public static IEnumerable<string[]> Data
        {
            get
            {
                yield return new[] { @"c:\logs\test.log", @"c:\logs\test.log" };
                yield return new[] { @"c:\logs\{date:format=yyyy}", @"c:\logs\2017" };
                yield return new[] { @"c:\logs\{date:format=yyyy}\{date:format=MM}\{date:format=dd}\test-{date:format=HH}.log", @"c:\logs\2017\02\06\test-13.log" };
            }
        }


        [Theory]
        [MemberData( nameof( Data ) )]
        public void Test( string path, string expected )
        {
            // Assign
            var dt = new DateTime( 2017, 2, 6, 13, 30, 22 );
            Clock.SetTimeProvider( () => dt );
            var roller = new FileRoller( path );


            // Act
            var result = roller.GetLogFilePath( Clock.DateTimeNow );


            // Assert
            Assert.Equal( expected, result );

        }
    }
}
