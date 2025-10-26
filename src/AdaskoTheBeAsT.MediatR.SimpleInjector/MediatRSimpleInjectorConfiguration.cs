using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MediatR;
using MediatR.NotificationPublishers;
using MediatR.Pipeline;
using SimpleInjector;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector;

/// <summary>
/// MediatR SimpleInjector configuration file.
/// </summary>
public class MediatRSimpleInjectorConfiguration
{
    public MediatRSimpleInjectorConfiguration()
    {
        MediatorImplementationType = typeof(Mediator);
        Lifestyle = Lifestyle.Singleton;
        AssembliesToScan = Array.Empty<Assembly>();
        PipelineBehaviorTypes = Array.Empty<Type>();
        StreamPipelineBehaviorTypes = Array.Empty<Type>();
        MediatorInstanceCreator = () => null;
        NotificationPublisherType = typeof(ForeachAwaitPublisher);
        RequestPreProcessorTypes = Array.Empty<Type>();
        RequestPostProcessorTypes = Array.Empty<Type>();
        RequestExceptionHandlerTypes = Array.Empty<Type>();
        RequestExceptionActionTypes = Array.Empty<Type>();
        LicenseKey = string.Empty;
    }

    /// <summary>
    /// Custom implementation of <see cref="IMediator"/>.
    /// Default is <see cref="Mediator"/>.
    /// </summary>
    public Type MediatorImplementationType { get; private set; }

    /// <summary>
    /// Custom instance creator of <see cref="IMediator"/>.
    /// Can be used for mocking Mediator.
    /// If not null then it takes precedence over MediatorImplementationType.
    /// </summary>
    public Func<IMediator?> MediatorInstanceCreator { get; private set; }

    /// <summary>
    /// Lifestyle in which IMediator implementation will be registered.
    /// Default is <see cref="Lifestyle"/>.Singleton.
    /// </summary>
    public Lifestyle Lifestyle { get; private set; }

    /// <summary>
    /// Flag which indicates if default implementation
    /// <see cref="RequestPreProcessorBehavior{TRequest,TResponse}"/> should be registered
    /// and all implementations of <see cref="IRequestPreProcessor{TRequest}"/>.
    /// </summary>
    public bool RequestPreProcessorBehaviorEnabled { get; private set; }

    /// <summary>
    /// Flag which indicates if default implementation
    /// <see cref="RequestPostProcessorBehavior{TRequest,TResponse}"/> should be registered
    /// and all implementations of <see cref="IRequestPostProcessor{TRequest,TResponse}"/>.
    /// </summary>
    public bool RequestPostProcessorBehaviorEnabled { get; private set; }

    /// <summary>
    /// Flag which indicates if default implementation
    /// <see cref="RequestExceptionProcessorBehavior{TRequest,TResponse}"/> should be registered
    /// and all implementations of <see cref="IRequestExceptionHandler{TRequest,TResponse,TException}"/>.
    /// </summary>
    public bool RequestExceptionProcessorBehaviorEnabled { get; private set; }

    /// <summary>
    /// Flag which indicates if default implementation
    /// <see cref="RequestExceptionActionProcessorBehavior{TRequest,TResponse}"/> should be registered
    /// and all implementations of <see cref="IRequestExceptionAction{TRequest,TException}"/>.
    /// </summary>
    public bool RequestExceptionActionProcessorBehaviorEnabled { get; private set; }

    /// <summary>
    /// Flag which indicates if default implementation
    /// <see cref="IStreamPipelineBehavior{TRequest,TResponse}"/> should be registered
    /// and all implementations of <see cref="IStreamRequestHandler{TRequest,TException}"/>.
    /// </summary>
    public bool StreamPipelineBehaviorEnabled { get; private set; }

    /// <summary>
    /// Assemblies which will be scanned for
    /// auto registering types implementing MediatR interfaces.
    /// </summary>
    public ICollection<Assembly> AssembliesToScan { get; private set; }

    /// <summary>
    /// Custom implementations of <see cref="IPipelineBehavior{TRequest,TResponse}"/>.
    /// </summary>
    public ICollection<Type> PipelineBehaviorTypes { get; private set; }

    /// <summary>
    /// Custom implementations of <see cref="IStreamPipelineBehavior{TRequest,TResponse}"/>.
    /// </summary>
    public ICollection<Type> StreamPipelineBehaviorTypes { get; private set; }

    /// <summary>
    /// Custom implementation of <see cref="INotificationPublisher"/>.
    /// Default is <see cref="ForeachAwaitPublisher "/>.
    /// </summary>
    public Type NotificationPublisherType { get; private set; }

    /// <summary>
    /// Custom implementations of <see cref="IRequestPreProcessor{TRequest}"/>.
    /// </summary>
    public ICollection<Type> RequestPreProcessorTypes { get; private set; }

    /// <summary>
    /// Custom implementations of <see cref="IRequestPostProcessor{TRequest, TResponse}"/>.
    /// </summary>
    public ICollection<Type> RequestPostProcessorTypes { get; private set; }

    /// <summary>
    /// Custom implementations of <see cref="IRequestExceptionHandler{TRequest, TResponse, TException}"/>.
    /// </summary>
    public ICollection<Type> RequestExceptionHandlerTypes { get; private set; }

    /// <summary>
    /// Custom implementations of <see cref="IRequestExceptionAction{TRequest, TException}"/>.
    /// </summary>
    public ICollection<Type> RequestExceptionActionTypes { get; private set; }

    /// <summary>
    /// Gets the license key associated with the MediatR.
    /// </summary>
    public string LicenseKey { get; private set; }

    /// <summary>
    /// Register custom implementation of <see cref="IMediator"/> type
    /// instead of default one <see cref="Mediator"/>.
    /// </summary>
    /// <typeparam name="TMediator">Custom <see cref="IMediator"/> implementation.</typeparam>
    /// <returns><see cref="MediatRSimpleInjectorConfiguration"/>
    /// with custom <see cref="IMediator"/> implementation.</returns>
    public MediatRSimpleInjectorConfiguration Using<TMediator>()
        where TMediator : IMediator
    {
        MediatorImplementationType = typeof(TMediator);
        return this;
    }

    /// <summary>
    /// Register custom IMediator instance creator
    /// instead of default one <see cref="Mediator"/>.
    /// </summary>
    /// <param name="instanceCreator">Custom <see cref="IMediator"/> instance creator function.</param>
    /// <returns><see cref="MediatRSimpleInjectorConfiguration"/>
    /// with custom <see cref="IMediator"/> instance creator function.</returns>
    public MediatRSimpleInjectorConfiguration Using(Func<IMediator> instanceCreator)
    {
        MediatorInstanceCreator =
            instanceCreator
            ?? throw new ArgumentNullException(nameof(instanceCreator));
        return this;
    }

    /// <summary>
    /// Set lifestyle of custom or default
    /// <see cref="IMediator"/> implementation to <see cref="Lifestyle"/>.Singleton.
    /// </summary>
    /// <returns><see cref="MediatRSimpleInjectorConfiguration"/>
    /// with <see cref="IMediator"/> implementation to <see cref="Lifestyle"/>.Singleton.</returns>
    public MediatRSimpleInjectorConfiguration AsSingleton()
    {
        Lifestyle = Lifestyle.Singleton;
        return this;
    }

    /// <summary>
    /// Set lifestyle of custom or default
    /// <see cref="IMediator"/> implementation to <see cref="Lifestyle"/>.Scoped.
    /// </summary>
    /// <returns><see cref="MediatRSimpleInjectorConfiguration"/>
    /// with <see cref="IMediator"/> implementation to <see cref="Lifestyle"/>.Scoped.</returns>
    public MediatRSimpleInjectorConfiguration AsScoped()
    {
        Lifestyle = Lifestyle.Scoped;
        return this;
    }

    /// <summary>
    /// Set lifestyle of custom or default
    /// <see cref="IMediator"/> implementation to <see cref="Lifestyle"/>.Transient.
    /// </summary>
    /// <returns><see cref="MediatRSimpleInjectorConfiguration"/>
    /// with <see cref="IMediator"/> implementation to <see cref="Lifestyle"/>.Transient.</returns>
    public MediatRSimpleInjectorConfiguration AsTransient()
    {
        Lifestyle = Lifestyle.Transient;
        return this;
    }

    /// <summary>
    /// Setup assemblies which will be scanned for
    /// auto registering types implementing MediatR interfaces.
    /// </summary>
    /// <param name="assembliesToScan">Assemblies which will be scanned for
    /// auto registering types.</param>
    /// <returns><see cref="MediatRSimpleInjectorConfiguration"/>
    /// with assemblies to scan configured.</returns>
    public MediatRSimpleInjectorConfiguration WithAssembliesToScan(params Assembly[] assembliesToScan)
    {
        AssembliesToScan = assembliesToScan;
        return this;
    }

    /// <summary>
    /// Setup assemblies which will be scanned for
    /// auto registering types implementing MediatR interfaces.
    /// </summary>
    /// <param name="assembliesToScan">Assemblies which will be scanned for
    /// auto registering types.</param>
    /// <returns><see cref="MediatRSimpleInjectorConfiguration"/>
    /// with assemblies to scan configured.</returns>
    public MediatRSimpleInjectorConfiguration WithAssembliesToScan(IEnumerable<Assembly> assembliesToScan)
    {
        AssembliesToScan = assembliesToScan.ToList();
        return this;
    }

    /// <summary>
    /// Setup assemblies which will be scanned for
    /// auto registering types implementing MediatR interfaces
    /// by types from given assemblies (marker types).
    /// </summary>
    /// <param name="handlerAssemblyMarkerTypes">Types from assemblies which will be scanned for
    /// auto registering types.</param>
    /// <returns><see cref="MediatRSimpleInjectorConfiguration"/>
    /// with assemblies to scan configured.</returns>
    public MediatRSimpleInjectorConfiguration WithHandlerAssemblyMarkerTypes(params Type[] handlerAssemblyMarkerTypes)
    {
        AssembliesToScan = handlerAssemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly).ToList();
        return this;
    }

    /// <summary>
    /// Setup assemblies which will be scanned for
    /// auto registering types implementing MediatR interfaces
    /// by types from given assemblies (marker types).
    /// </summary>
    /// <param name="handlerAssemblyMarkerTypes">Types from assemblies which will be scanned for
    /// auto registering types.</param>
    /// <returns><see cref="MediatRSimpleInjectorConfiguration"/>
    /// with assemblies to scan configured.</returns>
    public MediatRSimpleInjectorConfiguration WithHandlerAssemblyMarkerTypes(IEnumerable<Type> handlerAssemblyMarkerTypes)
    {
        AssembliesToScan = handlerAssemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly).ToList();
        return this;
    }

    /// <summary>
    /// Setup flags which indicates which default behaviors and processors should be registered.
    /// Default is all false.
    /// </summary>
    /// <param name="requestPreProcessorBehaviorEnabled">Flag which indicates if default implementation
    /// <see cref="RequestPreProcessorBehavior{TRequest,TResponse}"/> should be registered
    /// and all implementations of <see cref="IRequestPreProcessor{TRequest}"/>.</param>
    /// <param name="requestPostProcessorBehaviorEnabled">Flag which indicates if default implementation
    /// <see cref="RequestPostProcessorBehavior{TRequest,TResponse}"/> should be registered
    /// and all implementations of <see cref="IRequestPostProcessor{TRequest,TResponse}"/>.</param>
    /// <param name="requestExceptionProcessorBehaviorEnabled">Flag which indicates if default implementation
    /// <see cref="RequestExceptionProcessorBehavior{TRequest,TResponse}"/> should be registered
    /// and all implementations of <see cref="IRequestExceptionHandler{TRequest,TResponse,TException}"/>.</param>
    /// <param name="requestExceptionActionProcessorBehaviorEnabled">Flag which indicates if default implementation
    /// <see cref="RequestExceptionActionProcessorBehavior{TRequest,TResponse}"/> should be registered
    /// and all implementations of <see cref="IRequestExceptionAction{TRequest,TException}"/>.</param>
    /// <returns><see cref="MediatRSimpleInjectorConfiguration"/>
    /// with flags for behavior configured.</returns>
    public MediatRSimpleInjectorConfiguration UsingBuiltinPipelineProcessorBehaviors(
        bool requestPreProcessorBehaviorEnabled,
        bool requestPostProcessorBehaviorEnabled,
        bool requestExceptionProcessorBehaviorEnabled,
        bool requestExceptionActionProcessorBehaviorEnabled)
    {
        RequestPreProcessorBehaviorEnabled = requestPreProcessorBehaviorEnabled;
        RequestPostProcessorBehaviorEnabled = requestPostProcessorBehaviorEnabled;
        RequestExceptionProcessorBehaviorEnabled = requestExceptionProcessorBehaviorEnabled;
        RequestExceptionActionProcessorBehaviorEnabled = requestExceptionActionProcessorBehaviorEnabled;
        return this;
    }

    /// <summary>
    /// Setup flag which indicates if all behaviors and processors should be registered.
    /// Default is false.
    /// </summary>
    /// <param name="processorBehaviorEnabled">Flag which enable all behaviors and processors at once.</param>
    /// <returns><see cref="MediatRSimpleInjectorConfiguration"/>
    /// with flags for behavior configured.</returns>
    public MediatRSimpleInjectorConfiguration UsingBuiltinPipelineProcessorBehaviors(bool processorBehaviorEnabled)
    {
        RequestPreProcessorBehaviorEnabled = processorBehaviorEnabled;
        RequestPostProcessorBehaviorEnabled = processorBehaviorEnabled;
        RequestExceptionProcessorBehaviorEnabled = processorBehaviorEnabled;
        RequestExceptionActionProcessorBehaviorEnabled = processorBehaviorEnabled;
        StreamPipelineBehaviorEnabled = processorBehaviorEnabled;
        return this;
    }

    /// <summary>
    /// Setup additional custom implementations of <see cref="IPipelineBehavior{TRequest,TResponse}"/>.
    /// </summary>
    /// <param name="pipelineBehaviorTypes"><see cref="IPipelineBehavior{TRequest,TResponse}"/> implementation types.</param>
    /// <returns><see cref="MediatRSimpleInjectorConfiguration"/>
    /// with <see cref="IPipelineBehavior{TRequest,TResponse}"/> implementation types configured.</returns>
    public MediatRSimpleInjectorConfiguration UsingPipelineProcessorBehaviors(IEnumerable<Type> pipelineBehaviorTypes)
    {
        var pipelineBehaviorTypeList = pipelineBehaviorTypes.ToList();
        if (pipelineBehaviorTypeList.Exists(t => !t.IsAssignableToGenericType(typeof(IPipelineBehavior<,>))))
        {
            throw new InvalidPipelineBehaviorTypeException(
                "Elements should implement interface IPipelineBehavior<,>");
        }

        PipelineBehaviorTypes = pipelineBehaviorTypeList;
        return this;
    }

    /// <summary>
    /// Setup additional custom implementations of <see cref="IPipelineBehavior{TRequest,TResponse}"/>.
    /// </summary>
    /// <param name="pipelineBehaviorTypes"><see cref="IPipelineBehavior{TRequest,TResponse}"/> implementation types.</param>
    /// <returns><see cref="MediatRSimpleInjectorConfiguration"/>
    /// with <see cref="IPipelineBehavior{TRequest,TResponse}"/> implementation types configured.</returns>
    public MediatRSimpleInjectorConfiguration UsingPipelineProcessorBehaviors(params Type[] pipelineBehaviorTypes)
    {
        if (Array.Exists(pipelineBehaviorTypes, t => !t.IsAssignableToGenericType(typeof(IPipelineBehavior<,>))))
        {
            throw new InvalidPipelineBehaviorTypeException(
                "Elements should implement interface IPipelineBehavior<,>");
        }

        PipelineBehaviorTypes = pipelineBehaviorTypes;
        return this;
    }

    /// <summary>
    /// Setup additional custom implementations of <see cref="IStreamPipelineBehavior{TRequest,TResponse}"/>.
    /// </summary>
    /// <param name="pipelineBehaviorTypes"><see cref="IStreamPipelineBehavior{TRequest,TResponse}"/> implementation types.</param>
    /// <returns><see cref="MediatRSimpleInjectorConfiguration"/>
    /// with <see cref="IStreamPipelineBehavior{TRequest,TResponse}"/> implementation types configured.</returns>
    public MediatRSimpleInjectorConfiguration UsingStreamPipelineBehaviors(IEnumerable<Type> pipelineBehaviorTypes)
    {
        var pipelineBehaviorTypeList = pipelineBehaviorTypes.ToList();
        if (pipelineBehaviorTypeList.Exists(t => !t.IsAssignableToGenericType(typeof(IStreamPipelineBehavior<,>))))
        {
            throw new InvalidStreamPipelineBehaviorTypeException(
                "Elements should implement interface IStreamPipelineBehavior<,>");
        }

        StreamPipelineBehaviorTypes = pipelineBehaviorTypeList;
        return this;
    }

    /// <summary>
    /// Setup additional custom implementations of <see cref="IStreamPipelineBehavior{TRequest,TResponse}"/>.
    /// </summary>
    /// <param name="pipelineBehaviorTypes"><see cref="IStreamPipelineBehavior{TRequest,TResponse}"/> implementation types.</param>
    /// <returns><see cref="MediatRSimpleInjectorConfiguration"/>
    /// with <see cref="IStreamPipelineBehavior{TRequest,TResponse}"/> implementation types configured.</returns>
    public MediatRSimpleInjectorConfiguration UsingStreamPipelineBehaviors(params Type[] pipelineBehaviorTypes)
    {
        if (Array.Exists(pipelineBehaviorTypes, t => !t.IsAssignableToGenericType(typeof(IStreamPipelineBehavior<,>))))
        {
            throw new InvalidStreamPipelineBehaviorTypeException(
                "Elements should implement interface IStreamPipelineBehavior<,>");
        }

        StreamPipelineBehaviorTypes = pipelineBehaviorTypes;
        return this;
    }

    /// <summary>
    /// Setup builtin <see cref="ForeachAwaitPublisher"/> notification publisher.
    /// </summary>
    /// <returns><see cref="MediatRSimpleInjectorConfiguration"/>
    /// with <see cref="INotificationPublisher"/> implementation type configured.</returns>
    public MediatRSimpleInjectorConfiguration WithNotificationPublisherForeachAwait()
    {
        NotificationPublisherType = typeof(ForeachAwaitPublisher);
        return this;
    }

    /// <summary>
    /// Setup builtin <see cref="ForeachAwaitPublisher"/> notification publisher.
    /// </summary>
    /// <returns><see cref="MediatRSimpleInjectorConfiguration"/>
    /// with <see cref="INotificationPublisher"/> implementation type configured.</returns>
    public MediatRSimpleInjectorConfiguration WithNotificationPublisherTaskWhenAll()
    {
        NotificationPublisherType = typeof(TaskWhenAllPublisher);
        return this;
    }

    /// <summary>
    /// Setup custom <see cref="INotificationPublisher"/>.
    /// </summary>
    /// <returns><see cref="MediatRSimpleInjectorConfiguration"/>
    /// with <see cref="INotificationPublisher"/> implementation type configured.</returns>
    public MediatRSimpleInjectorConfiguration WithNotificationPublisherCustom<T>()
        where T : INotificationPublisher
    {
        NotificationPublisherType = typeof(T);
        return this;
    }

    /// <summary>
    /// Setup additional custom implementations of <see cref="IRequestPreProcessor{TRequest}"/>.
    /// </summary>
    /// <param name="requestPreProcessorTypes"><see cref="IRequestPreProcessor{TRequest}"/> implementation types.</param>
    /// <returns><see cref="MediatRSimpleInjectorConfiguration"/>
    /// with <see cref="IRequestPreProcessor{TRequest}"/> implementation types configured.</returns>
    public MediatRSimpleInjectorConfiguration WithRequestPreProcessorTypes(IEnumerable<Type> requestPreProcessorTypes)
    {
        var requestPreProcessorTypeList = requestPreProcessorTypes.ToList();
        if (requestPreProcessorTypeList.Exists(t => !t.IsAssignableToGenericType(typeof(IRequestPreProcessor<>))))
        {
            throw new InvalidRequestPreProcessorTypeException(
                "Elements should implement interface IRequestPreProcessor<>");
        }

        RequestPreProcessorTypes = requestPreProcessorTypeList;
        return this;
    }

    /// <summary>
    /// Setup additional custom implementations of <see cref="IRequestPreProcessor{TRequest}"/>.
    /// </summary>
    /// <param name="requestPreProcessorTypes"><see cref="IRequestPreProcessor{TRequest}"/> implementation types.</param>
    /// <returns><see cref="MediatRSimpleInjectorConfiguration"/>
    /// with <see cref="IRequestPreProcessor{TRequest}"/> implementation types configured.</returns>
    public MediatRSimpleInjectorConfiguration WithRequestPreProcessorTypes(params Type[] requestPreProcessorTypes)
    {
        if (Array.Exists(requestPreProcessorTypes, t => !t.IsAssignableToGenericType(typeof(IRequestPreProcessor<>))))
        {
            throw new InvalidRequestPreProcessorTypeException(
                "Elements should implement interface IRequestPreProcessor<>");
        }

        RequestPreProcessorTypes = requestPreProcessorTypes;
        return this;
    }

    /// <summary>
    /// Setup additional custom implementations of <see cref="IRequestPostProcessor{TRequest, TResponse}"/>.
    /// </summary>
    /// <param name="requestPostProcessorTypes"><see cref="IRequestPostProcessor{TRequest, TResponse}"/> implementation types.</param>
    /// <returns><see cref="MediatRSimpleInjectorConfiguration"/>
    /// with <see cref="IRequestPostProcessor{TRequest, TResponse}"/> implementation types configured.</returns>
    public MediatRSimpleInjectorConfiguration WithRequestPostProcessorTypes(IEnumerable<Type> requestPostProcessorTypes)
    {
        var requestPostProcessorTypeList = requestPostProcessorTypes.ToList();
        if (requestPostProcessorTypeList.Exists(t => !t.IsAssignableToGenericType(typeof(IRequestPostProcessor<,>))))
        {
            throw new InvalidRequestPostProcessorTypeException(
                "Elements should implement interface IRequestPostProcessor<,>");
        }

        RequestPostProcessorTypes = requestPostProcessorTypeList;
        return this;
    }

    /// <summary>
    /// Setup additional custom implementations of <see cref="IRequestPostProcessor{TRequest, TResponse}"/>.
    /// </summary>
    /// <param name="requestPostProcessorTypes"><see cref="IRequestPostProcessor{TRequest, TResponse}"/> implementation types.</param>
    /// <returns><see cref="MediatRSimpleInjectorConfiguration"/>
    /// with <see cref="IRequestPostProcessor{TRequest, TResponse}"/> implementation types configured.</returns>
    public MediatRSimpleInjectorConfiguration WithRequestPostProcessorTypes(params Type[] requestPostProcessorTypes)
    {
        if (Array.Exists(requestPostProcessorTypes, t => !t.IsAssignableToGenericType(typeof(IRequestPostProcessor<,>))))
        {
            throw new InvalidRequestPostProcessorTypeException(
                "Elements should implement interface IRequestPostProcessor<,>");
        }

        RequestPostProcessorTypes = requestPostProcessorTypes;
        return this;
    }

    /// <summary>
    /// Setup additional custom implementations of <see cref="IRequestExceptionHandler{TRequest, TResponse, TException}"/>.
    /// </summary>
    /// <param name="requestExceptionHandlerTypes"><see cref="IRequestExceptionHandler{TRequest, TResponse, TException}"/> implementation types.</param>
    /// <returns><see cref="MediatRSimpleInjectorConfiguration"/>
    /// with <see cref="IRequestExceptionHandler{TRequest, TResponse, TException}"/> implementation types configured.</returns>
    public MediatRSimpleInjectorConfiguration WithRequestExceptionHandlerTypes(IEnumerable<Type> requestExceptionHandlerTypes)
    {
        var requestExceptionHandlerTypeList = requestExceptionHandlerTypes.ToList();
        if (requestExceptionHandlerTypeList.Exists(t => !t.IsAssignableToGenericType(typeof(IRequestExceptionHandler<,,>))))
        {
            throw new InvalidRequestExceptionHandlerTypeException(
                "Elements should implement interface IRequestExceptionHandler<,,>");
        }

        RequestExceptionHandlerTypes = requestExceptionHandlerTypeList;
        return this;
    }

    /// <summary>
    /// Setup additional custom implementations of <see cref="IRequestExceptionHandler{TRequest, TResponse, TException}"/>.
    /// </summary>
    /// <param name="requestExceptionHandlerTypes"><see cref="IRequestExceptionHandler{TRequest, TResponse, TException}"/> implementation types.</param>
    /// <returns><see cref="MediatRSimpleInjectorConfiguration"/>
    /// with <see cref="IRequestExceptionHandler{TRequest, TResponse, TException}"/> implementation types configured.</returns>
    public MediatRSimpleInjectorConfiguration WithRequestExceptionHandlerTypes(params Type[] requestExceptionHandlerTypes)
    {
        if (Array.Exists(requestExceptionHandlerTypes, t => !t.IsAssignableToGenericType(typeof(IRequestExceptionHandler<,,>))))
        {
            throw new InvalidRequestExceptionHandlerTypeException(
                "Elements should implement interface IRequestExceptionHandler<,,>");
        }

        RequestExceptionHandlerTypes = requestExceptionHandlerTypes;
        return this;
    }

    /// <summary>
    /// Setup additional custom implementations of <see cref="IRequestExceptionAction{TRequest, TException}"/>.
    /// </summary>
    /// <param name="requestExceptionActionTypes"><see cref="IRequestExceptionAction{TRequest, TException}"/> implementation types.</param>
    /// <returns><see cref="MediatRSimpleInjectorConfiguration"/>
    /// with <see cref="IRequestExceptionAction{TRequest, TException}"/> implementation types configured.</returns>
    public MediatRSimpleInjectorConfiguration WithRequestExceptionActionTypes(IEnumerable<Type> requestExceptionActionTypes)
    {
        var requestExceptionActionTypeList = requestExceptionActionTypes.ToList();
        if (requestExceptionActionTypeList.Exists(t => !t.IsAssignableToGenericType(typeof(IRequestExceptionAction<,>))))
        {
            throw new InvalidRequestExceptionActionTypeException(
                "Elements should implement interface IRequestExceptionAction<,>");
        }

        RequestExceptionActionTypes = requestExceptionActionTypeList;
        return this;
    }

    /// <summary>
    /// Setup additional custom implementations of <see cref="IRequestExceptionAction{TRequest, TException}"/>.
    /// </summary>
    /// <param name="requestExceptionActionTypes"><see cref="IRequestExceptionAction{TRequest, TException}"/> implementation types.</param>
    /// <returns><see cref="MediatRSimpleInjectorConfiguration"/>
    /// with <see cref="IRequestExceptionAction{TRequest, TException}"/> implementation types configured.</returns>
    public MediatRSimpleInjectorConfiguration WithRequestExceptionActionTypes(params Type[] requestExceptionActionTypes)
    {
        if (Array.Exists(requestExceptionActionTypes, t => !t.IsAssignableToGenericType(typeof(IRequestExceptionAction<,>))))
        {
            throw new InvalidRequestExceptionActionTypeException(
                "Elements should implement interface IRequestExceptionAction<,>");
        }

        RequestExceptionActionTypes = requestExceptionActionTypes;
        return this;
    }

    /// <summary>
    /// Sets the license key for the configuration and returns the updated instance.
    /// </summary>
    /// <param name="licenseKey">The license key to associate with the configuration. Cannot be <see langword="null"/>.</param>
    /// <returns>The updated <see cref="MediatRSimpleInjectorConfiguration"/> instance.</returns>
    public MediatRSimpleInjectorConfiguration WithLicenseKey(string licenseKey)
    {
        LicenseKey = licenseKey;
        return this;
    }
}
