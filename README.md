# TacitusLogger.Destinations.Trace

> Extension destination for TacitusLogger that sends logs to System.Diagnostics.Trace listeners.
 
Dependencies:  
* .Net Standard >=2.0  
* TacitusLogger >=0.1.0
  
> Attention: `TacitusLogger.Destinations.Trace` is currently in **Alpha phase**. This means you should not use it in any production code.

## Installation

The NuGet <a href="https://www.nuget.org/packages/TacitusLogger.Destinations.Trace" target="_blank">package</a>:

```powershell
PM> Install-Package TacitusLogger.Destinations.Trace
```

## Examples

#### Adding trace destination with default parameters
Using builders:

```cs
ILogger logger = LoggerBuilder.Logger().ForAllLogs()
                                       .Trace().Add()
                                       .BuildLogger();
```
Or directly:
```cs
TraceDestination traceDestination = new TraceDestination();
Logger logger = new Logger();
logger.AddLogDestinations(traceDestination);
```
---
#### Trace destination with simple template log serializer
Using builders:
```cs
string template = "[$LogDate]-[$LogType]-[$Description]-[From: $Context]-[Src: $Source]-[Id: $LogId]";
ILogger logger = LoggerBuilder.Logger()
                              .ForAllLogs()
                              .Trace().WithSimpleTemplateLogText(template)
                                      .Add()
                              .BuildLogger();
```
Or directly:
```cs
string template = "[$LogDate]-[$LogType]-[$Description]-[From: $Context]-[Src: $Source]-[Id: $LogId]";
TraceDestination traceDestination = new TraceDestination(template);
Logger logger = new Logger();
logger.NewLogGroup(x => true).AddDestinations(traceDestination);
```
---
#### Trace destination with custom log serializer
 Using builders:
 ```cs
ILogSerializer customLogSerializer = new Mock<ILogSerializer>().Object;

ILogger logger = LoggerBuilder.Logger()
                              .ForAllLogs()
                              .Trace().WithCustomLogSerializer(customLogSerializer)
                                      .Add()
                              .BuildLogger();
 ```
Or directly:
```cs
ILogSerializer customLogSerializer = new Mock<ILogSerializer>().Object;
TraceDestination traceDestination = new TraceDestination(customLogSerializer);
Logger logger = new Logger();
logger.NewLogGroup(x => true).AddDestinations(traceDestination);
```

