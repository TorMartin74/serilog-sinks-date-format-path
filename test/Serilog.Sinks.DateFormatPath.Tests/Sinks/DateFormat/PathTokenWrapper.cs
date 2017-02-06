using System;
using System.Linq;
using Xunit.Abstractions;

namespace Serilog.Sinks.DateFormat
{
    public class PathTokenWrapper : IXunitSerializable
    {
        PathToken _pt;

        public PathTokenWrapper( PathToken pt )
        {
            _pt = pt;
        }

        public string Value => _pt.Value;
        public PathTokenType Type => _pt.Type;



        public void Deserialize( IXunitSerializationInfo info )
        {
            _pt.Value = info.GetValue<string>( "Value" );
            _pt.Type = info.GetValue<PathTokenType>( "Type" );
        }

        public void Serialize( IXunitSerializationInfo info )
        {
            info.AddValue( "Value", _pt.Value );
            info.AddValue( "Type", _pt.Type );
        }


        public static implicit operator PathTokenWrapper( PathToken pt )
        {
            return new PathTokenWrapper( pt );
        }
    }
}