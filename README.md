# AdaskoTheBeAsT.MediatR.SimpleInjector

MediatR extensions for SimpleInjector.

## Usage

Scans assemblies and adds handlers, preprocessors, and postprocessors implementations to the SimpleInjector container. There are few options to use with `Container` instance:

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
            var mainAssembly = typeof(MediatrConfigurer).GetTypeInfo().Assembly;
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

Code originates from MediatR.Extensions.Microsoft.DependencyInjection and was changed to work with SimpleInjector.
