[![Build status](https://ci.appveyor.com/api/projects/status/48iiyw5d826f858o?svg=true)](https://ci.appveyor.com/project/TorMartin74/serilog-sinks-date-format-path)


# Serilog.Sink.DateFormatPath

A Serilog sink that write events to rolling text files, where the path is determined on date and time.

#### Configuration Syntax
```
{date:format=string:isUtc=boolean:culture=string}
```

#### Examples
```
c:\\logs\\{date:format=yyyy}\\{date:format=MM}\\{date:format=dd}\\file-{date:format=HH}.log
```

## Parameters

#### format: 
Date format. Can be any argument accepted by DateTime.ToString(format). Formats that contains colons needs to be escaped with a backslash.

#### isUtc:
Indicates whether to output the DateTime as UTC instead if local time

#### culture:
The culture used for rendering.
