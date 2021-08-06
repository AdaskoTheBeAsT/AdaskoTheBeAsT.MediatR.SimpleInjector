# AdaskoTheBeAsT.MediatR.SimpleInjector and AdaskoTheBeAsT.MediatR.SimpleInjector.AspNetCore

MediatR extensions for SimpleInjector.

## Badges
[![CodeFactor](https://www.codefactor.io/repository/github/adaskothebeast/adaskothebeast.mediatr.simpleinjector/badge)](https://www.codefactor.io/repository/github/adaskothebeast/adaskothebeast.mediatr.simpleinjector)
[![Total alerts](https://img.shields.io/lgtm/alerts/g/AdaskoTheBeAsT/AdaskoTheBeAsT.MediatR.SimpleInjector.svg?logo=lgtm&logoWidth=18)](https://lgtm.com/projects/g/AdaskoTheBeAsT/AdaskoTheBeAsT.MediatR.SimpleInjector/alerts/)
[![Build Status](https://adaskothebeast.visualstudio.com/AdaskoTheBeAsT.MediatR.SimpleInjector/_apis/build/status/AdaskoTheBeAsT.AdaskoTheBeAsT.MediatR.SimpleInjector?branchName=master)](https://adaskothebeast.visualstudio.com/AdaskoTheBeAsT.MediatR.SimpleInjector/_build/latest?definitionId=7&branchName=master)
![Azure DevOps tests](https://img.shields.io/azure-devops/tests/AdaskoTheBeAsT/AdaskoTheBeAsT.MediatR.SimpleInjector/17)
![Azure DevOps coverage](https://img.shields.io/azure-devops/coverage/AdaskoTheBeAsT/AdaskoTheBeAsT.MediatR.SimpleInjector/17?style=plastic)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=AdaskoTheBeAsT_AdaskoTheBeAsT.MediatR.SimpleInjector&metric=alert_status)](https://sonarcloud.io/dashboard?id=AdaskoTheBeAsT_AdaskoTheBeAsT.MediatR.SimpleInjector)
![Sonar Tests](https://img.shields.io/sonar/tests/AdaskoTheBeAsT_AdaskoTheBeAsT.MediatR.SimpleInjector?server=https%3A%2F%2Fsonarcloud.io)
![Sonar Test Count](https://img.shields.io/sonar/total_tests/AdaskoTheBeAsT_AdaskoTheBeAsT.MediatR.SimpleInjector?server=https%3A%2F%2Fsonarcloud.io)
![Sonar Test Execution Time](https://img.shields.io/sonar/test_execution_time/AdaskoTheBeAsT_AdaskoTheBeAsT.MediatR.SimpleInjector?server=https%3A%2F%2Fsonarcloud.io)
![Sonar Coverage](https://img.shields.io/sonar/coverage/AdaskoTheBeAsT_AdaskoTheBeAsT.MediatR.SimpleInjector?server=https%3A%2F%2Fsonarcloud.io&style=plastic)
![Nuget](https://img.shields.io/nuget/dt/AdaskoTheBeAsT.MediatR.SimpleInjector)

## Usage in AspNetCore

Scans assemblies and adds handlers, preprocessors, and postprocessors implementations to the SimpleInjector container. Additionaly it register decorator which passes HttpContext.RequestAborted cancellation token from asp.net core controllers to MediatR.  
Install package ```AdaskoTheBeAsT.MediatR.SimpleInjector.AspNetCore```.  
There are few options to use with `Container` instance:

1. Marker type from assembly which will be scanned

    ```cs
    container.AddMediatRAspNetCore(typeof(MyHandler), type2 /*, ...*/);
    ```

1. Assembly which will be scanned

    ```cs
    container.AddMediatRAspNetCore(assembly, assembly2 /*, ...*/);
    ```

1. Full configuration

   ```cs
    var testMediator = new Mock<IMediator>();
    container.AddMediatR(
        cfg =>
        {
            cfg.Using(() => testMediator.Object);
            cfg.WithHandlerAssemblyMarkerTypes(typeof(MyMarkerType));
            cfg.UsingBuiltinPipelineProcessorBehaviors(true);
            cfg.UsingPipelineProcessorBehaviors(typeof(CustomPipelineBehavior<,>));
        });
   ``` 


## Usage in other project types

Scans assemblies and adds handlers, preprocessors, and postprocessors implementations to the SimpleInjector container.  
Install package ```AdaskoTheBeAsT.MediatR.SimpleInjector```.  
There are few options to use with `Container` instance:

1. Marker type from assembly which will be scanned

    ```cs
    container.AddMediatR(typeof(MyHandler), type2 /*, ...*/);
    ```

1. List of assemblies which will be scanned.

   Below is sample for scanning assemblies from some solution.

    ```cs
    [ExcludeFromCodeCoverage]
    public static class MediatRConfigurator
    {
        private const string NamespacePrefix = "YourNamespace";

        public static void Configure(Container container)
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            var assemblies = new List<Assembly>();
            var mainAssembly = typeof(MediatRConfigurator).GetTypeInfo().Assembly;
            var refAssemblies = mainAssembly.GetReferencedAssemblies();
            foreach (var assemblyName in refAssemblies
                .Where(a => a.FullName.StartsWith(NamespacePrefix, StringComparison.OrdinalIgnoreCase)))
                {
                    var assembly = loadedAssemblies.Find(l => l.FullName == assemblyName.FullName)
                        ?? AppDomain.CurrentDomain.Load(assemblyName);
                    assemblies.Add(assembly);
                }
            container.AddMediatR(assemblies);
        }
    }
   ```

This will register:

- `IMediator` as Singleton
- `IRequestHandler<>` concrete implementations as Transient
- `INotificationHandler<>` concrete implementations as Transient

## Advanced usage

### Setting up custom `IMediator` instance and marker type from assembly for unit testing (Moq sample)

   ```cs
    var testMediator = new Mock<IMediator>();
    container.AddMediatR(
        cfg =>
        {
            cfg.Using(() => testMediator.Object);
            cfg.WithHandlerAssemblyMarkerTypes(typeof(MyMarkerType));
        });
   ```

### Setting up custom `IMediator` implementation and marker type from assembly

   ```cs
    container.AddMediatR(
        cfg =>
        {
            cfg.Using<MyCustomMediator>();
            cfg.WithHandlerAssemblyMarkerTypes(typeof(MyMarkerType));
        });
   ```

### Setting up custom `IMediator` implementation and assemblies to scan

   ```cs
    container.AddMediatR(
        cfg =>
        {
            cfg.Using<MyCustomMediator>();
            cfg.WithAssembliesToScan(assemblies);
        });
   ```

### Setting assemblies to scan and different lifetime for IMediator implementation

   ```cs
    container.AddMediatR(
        cfg =>
        {
            cfg.WithAssembliesToScan(assemblies);
            cfg.AsScoped();
        });
   ```

### Setting assemblies to scan and additonaly enabling all builtin behaviors and user defined processors/handlers

This will register following behaviors:

- `RequestPreProcessorBehavior<,>`
- `RequestPostProcessorBehavior<,>`
- `RequestExceptionProcessorBehavior<,>`
- `RequestExceptionActionProcessorBehavior<,>`

and all user defined implementation of processors and handlers:

- `IRequestPreProcessor<>`
- `IRequestPostProcessor<,>`
- `IRequestExceptionHandler<,,>`
- `IRequestExceptionActionHandler<,>`

   ```cs
    container.AddMediatR(
        cfg =>
        {
            cfg.WithAssembliesToScan(assemblies);
            cfg.UsingBuiltinPipelineProcessorBehaviors(true);
        });
   ```

### Setting assemblies to scan and additonaly enabling choosen builtin behaviors and user defined processors/handlers

This will register following behaviors:

- `RequestPreProcessorBehavior<,>`
- `RequestExceptionProcessorBehavior<,>`

and all user defined implementation of processors and handlers:

- `IRequestPreProcessor<>`
- `IRequestExceptionHandler<,,>`

   ```cs
    container.AddMediatR(
        cfg =>
        {
            cfg.WithAssembliesToScan(assemblies);
            cfg.UsingBuiltinPipelineProcessorBehaviors(
                requestPreProcessorBehaviorEnabled: true,
                requestPostProcessorBehaviorEnabled: false,
                requestExceptionProcessorBehaviorEnabled: true,
                requestExceptionActionProcessorBehaviorEnabled: false);
        });
   ```

### Setting assemblies to scan and additonaly enabling choosen builtin behaviors and user defined processors/handlers also with custom Pipeline Process Behaviours

   ```cs
    container.AddMediatR(
        cfg =>
        {
            cfg.WithAssembliesToScan(assemblies);
            cfg.UsingBuiltinPipelineProcessorBehaviors(
                requestPreProcessorBehaviorEnabled: true,
                requestPostProcessorBehaviorEnabled: false,
                requestExceptionProcessorBehaviorEnabled: true,
                requestExceptionActionProcessorBehaviorEnabled: false);
            cfg.UsingPipelineProcessorBehaviors(typeof(CustomPipelineBehavior<,>));
        });
   ```

# Thanks to:

- Jimmy Boggard for MediatR
- Steven van Deursen for SimpleInjector
- Sebastian Kleinschmager for [idea of automatic passing RequestAborted to MediatR](https://github.com/jbogard/MediatR/issues/496)
- Konrad Rudolph for [idea of IsAssignableToGenericType](https://gist.github.com/klmr/4174727)

Code originates from MediatR.Extensions.Microsoft.DependencyInjection and was changed to work with SimpleInjector.
