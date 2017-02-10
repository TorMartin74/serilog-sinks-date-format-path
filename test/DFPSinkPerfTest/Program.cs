using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Serilog;


namespace DFPSinkPerfTest
{
    public class Program
    {
        public static void Main( string[] args )
        {
            var path = @"e:\logs\{date:format=yyyy}\{date:format=MM}\{date:format=dd}\perftest-{date:format=HH}.log";
            var log = new LoggerConfiguration()
                .WriteTo.DateFormatPath( path )
                .CreateLogger();


            int numberOfitems = 100000;

            Stopwatch sw = new Stopwatch();

            Console.WriteLine( $@"Writing {numberOfitems} lines to the DateFormatLog..." );

            sw.Start();
            for( int i = 0; i < numberOfitems; i++ )
            {
                log.Information( "Writing line number {lineNr}.", i );
            }
            sw.Stop();
            var elapsed1 = sw.Elapsed;

            sw.Reset();



            log = new LoggerConfiguration()
                .WriteTo.File( @"e:\logs\perftestest.log" )
                .CreateLogger();

            Console.WriteLine( $@"Writing {numberOfitems} lines to the normal File log..." );
            sw.Start();
            for( int i = 0; i < numberOfitems; i++ )
            {
                log.Information( "Writing line number {lineNr}.", i );
            }
            sw.Stop();
            var elapsed2 = sw.Elapsed;

            Console.WriteLine();

            Console.WriteLine( $@"DateFormatPath: {elapsed1.ToString()}" );
            Console.WriteLine( $@"File:           {elapsed2.ToString()}" );
            Console.WriteLine( "---------------------------------------------------------------------" );
            Console.WriteLine( $@"               {(elapsed2 - elapsed1).ToString()}" );


        }
    }
}
