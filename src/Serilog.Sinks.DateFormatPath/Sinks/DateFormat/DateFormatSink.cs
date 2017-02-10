using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.File;

namespace Serilog.Sinks.DateFormat
{
    public class DateFormatSink : ILogEventSink, IFlushableFileSink, IDisposable
    {
        readonly object _syncRoot = new object();

        string _pathFormat;
        ITextFormatter _formatter;
        Encoding _encoding;
        bool _buffered;
        bool _shared;

        ILogEventSink _currentFile;
        string _currentPath;


        FileRoller _fileRoller;


        /// <summary>
        /// Initializes a new instance of the <see cref="DateFormatSink"/> class.
        /// </summary>
        /// <param name="pathFormat">The path format.</param>
        /// <param name="formatter">The formatter.</param>
        /// <param name="buffered">if set to <c>true</c> [buffered].</param>
        /// <param name="shared">if set to <c>true</c> [shared].</param>
        /// <exception cref="System.ArgumentNullException">
        /// pathFormat
        /// or
        /// formatter
        /// </exception>
        public DateFormatSink( string pathFormat, ITextFormatter formatter, Encoding encoding, bool buffered = false, bool shared = false )
        {
            if( pathFormat == null ) throw new ArgumentNullException( nameof( pathFormat ) );
            if( formatter == null ) throw new ArgumentNullException( nameof( formatter ) );

            _pathFormat = pathFormat;
            _shared = shared;
            _encoding = encoding;
            _buffered = buffered;
            _formatter = formatter;
            _fileRoller = new FileRoller( _pathFormat );

        }
        

        /// <summary>
        /// Emit the provided log event to the sink.
        /// </summary>
        /// <param name="logEvent">The log event to write.</param>
        public void Emit( LogEvent logEvent )
        {
            if( logEvent == null ) throw new ArgumentNullException( nameof( logEvent ) );

            lock( _syncRoot )
            {
                string path = _fileRoller.GetLogFilePath( logEvent.Timestamp );
                if( _currentPath == null || (_currentPath != path) )
                {
                    FlushToDisk();
                    CloseFile();

                    _currentFile = new FileSink( path, _formatter, null, _encoding, _buffered );
                    _currentPath = path;
                }

                _currentFile.Emit( logEvent );
            }
        }


        /// <summary>
        /// Flush buffered contents to disk.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void FlushToDisk()
        {
            lock( _syncRoot )
            {
                (_currentFile as IFlushableFileSink)?.FlushToDisk();
            }
        }


        /// <summary>
        /// Closes the file.
        /// </summary>
        private void CloseFile()
        {
            if( _currentFile != null )
            {
                (_currentFile as IDisposable)?.Dispose();
                _currentFile = null;
            }

            //_nextCheckpoint = null;
            _currentPath = null;
        }


        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            lock( _syncRoot )
            {
                if( _currentFile == null ) return;
                CloseFile();
            }
        }
    }
}
