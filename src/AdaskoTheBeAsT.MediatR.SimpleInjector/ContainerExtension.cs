using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MediatR;
using MediatR.Pipeline;
using SimpleInjector;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector;

#pragma warning disable RCS1247 // Fix documentation comment tag.
/// <summary>
/// Extensions to scan for MediatR handlers and registers them in SimpleInjector.
/// - Scans for any handler interface implementations and registers them as <see cref="Lifestyle.Transient"/>
/// - Scans for any
///   <see cref="IRequestPreProcessor{TRequest}"/>
///   <see cref="IRequestPostProcessor{TRequest,TResponse}"/>
///   <see cref="IRequestExceptionHandler{TRequest,TResponse,TException}"/>
///   <see cref="IRequestExceptionAction{TRequest,TException}"/>
///   <see cref="IStreamRequestHandler{TRequest,TException}"/>
///   implementations and registers them as <see cref="Lifestyle.Transient"/> instances
/// Registers <see cref="IMediator"/> as <see cref="Lifestyle.Singleton"/> instances
/// After calling AddMediatR you can use the container to resolve an <see cref="IMediator"/> instance.
/// This scans for any <see cref="IPipelineBehavior{TRequest,TResponse}"/>
/// instances and also if flags for builtin processor behavior are enabled scans for
/// <see cref="RequestPreProcessorBehavior{TRequest,TResponse}"/>
/// <see cref="RequestPostProcessorBehavior{TRequest,TResponse}"/>
/// <see cref="RequestExceptionActionProcessorBehavior{TRequest,TResponse}"/>
/// <see cref="RequestExceptionProcessorBehavior{TRequest,TResponse}"/>.
/// To register behaviors, use method with the open generic or closed generic types.
/// <code>container.Register&lt;Type, Type&gt;();</code>.
/// </summary>
public static class ContainerExtension
#pragma warning restore RCS1247 // Fix documentation comment tag.
{
    /// <summary>
    /// Registers handlers and mediator types from the specified assemblies.
    /// </summary>
    /// <param name="container"><see cref="Container"/>.</param>
    /// <param name="assemblies">Assemblies to scan.</param>
    /// <returns><see cref="Container"/>.</returns>
    public static Container AddMediatR(
        this Container container,
        params Assembly[] assemblies)
    {
        return AddMediatR(
            container,
            config => config.WithAssembliesToScan(assemblies));
    }

    /// <summary>
    /// Registers handlers and mediator types from the specified assemblies.
    /// </summary>
    /// <param name="container"><see cref="Container"/>.</param>
    /// <param name="assemblies">Assemblies to scan.</param>
    /// <returns><see cref="Container"/>.</returns>
    public static Container AddMediatR(
        this Container container,
        IEnumerable<Assembly> assemblies)
    {
        return AddMediatR(
            container,
            config => config.WithAssembliesToScan(assemblies));
    }

    /// <summary>
    /// Registers handlers and mediator types from the assemblies that contain the specified types.
    /// </summary>
    /// <param name="container"><see cref="Container"/>.</param>
    /// <param name="handlerAssemblyMarkerTypes">Types used to mark assemblies to scan.</param>.
    /// <returns><see cref="Container"/>.</returns>
    public static Container AddMediatR(
        this Container container,
        params Type[] handlerAssemblyMarkerTypes)
    {
        return AddMediatR(
            container,
            config => config.WithHandlerAssemblyMarkerTypes(handlerAssemblyMarkerTypes));
    }

    /// <summary>
    /// Registers handlers and mediator types from the assemblies that contain the specified types.
    /// </summary>
    /// <param name="container"><see cref="Container"/>.</param>
    /// <param name="handlerAssemblyMarkerTypes">Types used to mark assemblies to scan.</param>
    /// <returns><see cref="Container"/>.</returns>
    public static Container AddMediatR(
        this Container container,
        IEnumerable<Type> handlerAssemblyMarkerTypes)
    {
        return AddMediatR(
            container,
            config => config.WithHandlerAssemblyMarkerTypes(handlerAssemblyMarkerTypes));
    }

    /// <summary>
    /// Registers handlers and mediator types from the specified assemblies.
    /// </summary>
    /// <param name="container"><see cref="Container"/>.</param>
    /// <param name="configuration">The action used to configure the options.</param>
    /// <returns><see cref="Container"/>.</returns>
    public static Container AddMediatR(
        this Container container,
        Action<MediatRSimpleInjectorConfiguration>? configuration)
    {
        if (container is null)
        {
            throw new ArgumentNullException(nameof(container));
        }

        var serviceConfig = new MediatRSimpleInjectorConfiguration();
        configuration?.Invoke(serviceConfig);

        return SetupContainer(container, serviceConfig);
    }

    internal static Container SetupContainer(
        this Container container,
        MediatRSimpleInjectorConfiguration serviceConfig)
    {
        var uniqueAssemblies = serviceConfig.AssembliesToScan.Distinct().ToArray();
        var allAssemblies = new List<Assembly> { typeof(IMediator).GetTypeInfo().Assembly };
        allAssemblies.AddRange(uniqueAssemblies);

        var customMediatorInstance = serviceConfig.MediatorInstanceCreator();

        if (customMediatorInstance is null)
        {
            if (serviceConfig.MediatorImplementationType == typeof(Mediator))
            {
                container.Register<IServiceProvider>(
                    () => new SimpleInjectorServiceProvider(container),
                    Lifestyle.Singleton);
                container.Register(
                    typeof(INotificationPublisher),
                    serviceConfig.NotificationPublisherType,
                    Lifestyle.Singleton);
                container.Register<IMediator>(
                    () => new Mediator(
                        container.GetInstance<IServiceProvider>(),
                        container.GetInstance<INotificationPublisher>()),
                    serviceConfig.Lifestyle);
            }
            else
            {
                container.Register(
                    typeof(IMediator),
                    serviceConfig.MediatorImplementationType,
                    serviceConfig.Lifestyle);
            }
        }
        else
        {
            container.Register(
                () => customMediatorInstance,
                serviceConfig.Lifestyle);
        }

        container.Register(typeof(IRequestHandler<,>), allAssemblies);
        container.Register(typeof(IRequestHandler<>), allAssemblies);
        container.Register(typeof(IStreamRequestHandler<,>), allAssemblies);
        RegisterNotifications(container, uniqueAssemblies);
        RegisterBehaviors(container, serviceConfig, uniqueAssemblies);
        return container;
    }

    internal static void RegisterNotifications(
        Container container,
        Assembly[] uniqueAssemblies)
    {
        RegisterIncludingGenericTypeDefinitions(
            container,
            uniqueAssemblies,
            typeof(INotificationHandler<>),
            Type.EmptyTypes);
    }

    internal static void RegisterBehaviors(
        Container container,
        MediatRSimpleInjectorConfiguration serviceConfig,
        Assembly[] uniqueAssemblies)
    {
        var processorBehaviors = new List<Type>();

        RegisterBehaviorsAndProcessors(
            container,
            uniqueAssemblies,
            processorBehaviors,
            typeof(RequestPreProcessorBehavior<,>),
            typeof(IRequestPreProcessor<>),
            serviceConfig.RequestPreProcessorBehaviorEnabled,
            serviceConfig.RequestPreProcessorTypes);

        RegisterBehaviorsAndProcessors(
            container,
            uniqueAssemblies,
            processorBehaviors,
            typeof(RequestPostProcessorBehavior<,>),
            typeof(IRequestPostProcessor<,>),
            serviceConfig.RequestPostProcessorBehaviorEnabled,
            serviceConfig.RequestPostProcessorTypes);

        RegisterBehaviorsAndProcessors(
            container,
            uniqueAssemblies,
            processorBehaviors,
            typeof(RequestExceptionProcessorBehavior<,>),
            typeof(IRequestExceptionHandler<,,>),
            serviceConfig.RequestExceptionProcessorBehaviorEnabled,
            serviceConfig.RequestExceptionHandlerTypes);

        RegisterBehaviorsAndProcessors(
            container,
            uniqueAssemblies,
            processorBehaviors,
            typeof(RequestExceptionActionProcessorBehavior<,>),
            typeof(IRequestExceptionAction<,>),
            serviceConfig.RequestExceptionActionProcessorBehaviorEnabled,
            serviceConfig.RequestExceptionActionTypes);

        processorBehaviors.AddRange(serviceConfig.PipelineBehaviorTypes);

        container.Collection.Register(typeof(IPipelineBehavior<,>), processorBehaviors);

        container.Collection.Register(typeof(IStreamPipelineBehavior<,>), serviceConfig.StreamPipelineBehaviorTypes);
    }

    internal static void RegisterBehaviorsAndProcessors(
        Container container,
        Assembly[] uniqueAssemblies,
        List<Type> behaviorTypes,
        Type behaviourType,
        Type processorType,
        bool behaviourEnabled,
        ICollection<Type> implementingTypes)
    {
        if (behaviourEnabled)
        {
            behaviorTypes.Add(behaviourType);
            RegisterIncludingGenericTypeDefinitions(
                container,
                uniqueAssemblies,
                processorType,
                implementingTypes);
        }
        else
        {
            container.Collection.Register(processorType, Enumerable.Empty<Type>());
        }
    }

    internal static void RegisterIncludingGenericTypeDefinitions(
        Container container,
        Assembly[] uniqueAssemblies,
        Type type,
        ICollection<Type> implementingTypes)
    {
        if (implementingTypes.Count > 0)
        {
            container.Collection.Register(type, implementingTypes);
            return;
        }

        // we have to do this because by default, generic type definitions (such as the Constrained Notification Handler) won't be registered
        var implementingTypes2 = container.GetTypesToRegister(
            type,
            uniqueAssemblies,
            new TypesToRegisterOptions
            {
                IncludeGenericTypeDefinitions = true,
                IncludeComposites = false,
            });

        container.Collection.Register(type, implementingTypes2);
    }
}
