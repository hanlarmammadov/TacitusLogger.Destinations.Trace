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

## License

[APACHE LICENSE 2.0](https://www.apache.org/licenses/LICENSE-2.0)

## See also

TacitusLogger:  

- [TacitusLogger](https://github.com/khanlarmammadov/TacitusLogger) - A simple yet powerful .NET logging library.

Destinations:

- [TacitusLogger.Destinations.MongoDb](https://github.com/khanlarmammadov/TacitusLogger.Destinations.MongoDb) - Extension destination for TacitusLogger that sends logs to MongoDb database.
- [TacitusLogger.Destinations.RabbitMq](https://github.com/khanlarmammadov/TacitusLogger.Destinations.RabbitMq) - Extension destination for TacitusLogger that sends logs to the RabbitMQ exchanges.
- [TacitusLogger.Destinations.Email](https://github.com/khanlarmammadov/TacitusLogger.Destinations.Email) - Extension destination for TacitusLogger that sends logs as emails using SMTP protocol.
- [TacitusLogger.Destinations.EntityFramework](https://github.com/khanlarmammadov/TacitusLogger.Destinations.EntityFramework) - Extension destination for TacitusLogger that sends logs to database using Entity Framework ORM. 
  
Dependency injection:
- [TacitusLogger.DI.Ninject](https://github.com/khanlarmammadov/TacitusLogger.DI.Ninject) - Extension for Ninject dependency injection container that helps to configure and add TacitusLogger as a singleton.
- [TacitusLogger.DI.Autofac](https://github.com/khanlarmammadov/TacitusLogger.DI.Autofac) - Extension for Autofac dependency injection container that helps to configure and add TacitusLogger as a singleton.
- [TacitusLogger.DI.MicrosoftDI](https://github.com/khanlarmammadov/TacitusLogger.DI.MicrosoftDI) - Extension for Microsoft dependency injection container that helps to configure and add TacitusLogger as a singleton.  

Log contributors:

- [TacitusLogger.Contributors.ThreadInfo](https://github.com/khanlarmammadov/TacitusLogger.Contributors.ThreadInfo) - Extension contributor for TacitusLogger that generates additional info related to the thread on which the logger method was called.
- [TacitusLogger.Contributors.MachineInfo](https://github.com/khanlarmammadov/TacitusLogger.Contributors.MachineInfo) - Extension contributor for TacitusLogger that generates additional info related to the machine on which the log was produced.
