using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace DFPSinkLogRunning
{
    public class Program
    {
        public static void Main( string[] args )
        {
            Run();
            Console.ReadKey();
        }


        async static void Run()
        {
            var path = @"e:\logs\{date:format=yyyy}\{date:format=MM}\{date:format=dd}\perftest-{date:format=HH-mm}.log";
            var log = new LoggerConfiguration()
                .WriteTo.DateFormatPath( path )
                .CreateLogger();




            Console.WriteLine( $@"Writing to log once every second" );

            long i = 0;
            while( true )
            {
                log.Information( "Writing line number {lineNr}.", i++ );
                await Task.Delay( TimeSpan.FromSeconds( 1 ) );
            }
        }
    }
}
