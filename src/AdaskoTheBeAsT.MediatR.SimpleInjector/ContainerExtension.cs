using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MediatR;
using MediatR.Pipeline;
using SimpleInjector;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector
{
    /// <summary>
    /// Extensions to scan for MediatR handlers and registers them in SimpleInjector.
    /// - Scans for any handler interface implementations and registers them as <see cref="Lifestyle.Transient"/>
    /// - Scans for any
    ///   <see cref="IRequestPreProcessor{TRequest}"/>
    ///   <see cref="IRequestPostProcessor{TRequest,TResponse}"/>
    ///   <see cref="IRequestExceptionHandler{TRequest,TResponse,TException}"/>
    ///   <see cref="IRequestExceptionAction{TRequest,TException}"/>
    ///   implementations and registers them as <see cref="Lifestyle.Transient"/> instances
    /// Registers <see cref="ServiceFactory"/> and <see cref="IMediator"/> as <see cref="Lifestyle.Singleton"/> instances
    /// After calling AddMediatR you can use the container to resolve an <see cref="IMediator"/> instance.
    /// This scans for any <see cref="IPipelineBehavior{TRequest,TResponse}"/>
    /// instances and also if flags for builtin processor behavior are enabled scans for
    /// <see cref="RequestPreProcessorBehavior{TRequest,TResponse}"/>
    /// <see cref="RequestPostProcessorBehavior{TRequest,TResponse}"/>
    /// <see cref="RequestExceptionActionProcessorBehavior{TRequest,TResponse}"/>
    /// <see cref="RequestExceptionProcessorBehavior{TRequest,TResponse}"/>.
    /// To register behaviors, use method with the open generic or closed generic types.
    /// <code>container.Register&lt;Type, Type&gt;();</code>
    /// </summary>
    public static class ContainerExtension
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
            return AddMediatR(container, config => config.WithAssembliesToScan(assemblies));
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
            return AddMediatR(container, config => config.WithAssembliesToScan(assemblies));
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
            var serviceConfig = new MediatRSimpleInjectorConfiguration();
            configuration?.Invoke(serviceConfig);

            var uniqueAssemblies = serviceConfig.AssembliesToScan.Distinct().ToArray();
            var allAssemblies = new List<Assembly> { typeof(IMediator).GetTypeInfo().Assembly };
            allAssemblies.AddRange(uniqueAssemblies);

            container.Register(typeof(IMediator), serviceConfig.MediatorImplementationType, serviceConfig.Lifestyle);
            container.Register(typeof(IRequestHandler<,>), allAssemblies);
            RegisterNotifications(container, uniqueAssemblies);
            RegisterBehaviors(container, serviceConfig, uniqueAssemblies);

            container.Register(() => new ServiceFactory(container.GetInstance), Lifestyle.Singleton);

            return container;
        }

        internal static void RegisterNotifications(
            Container container,
            Assembly[] uniqueAssemblies)
        {
            RegisterIncludingGenericTypeDefinitions(container, uniqueAssemblies, typeof(INotificationHandler<>));
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
                serviceConfig.RequestPreProcessorBehaviorEnabled);

            RegisterBehaviorsAndProcessors(
                container,
                uniqueAssemblies,
                processorBehaviors,
                typeof(RequestPostProcessorBehavior<,>),
                typeof(IRequestPostProcessor<,>),
                serviceConfig.RequestPostProcessorBehaviorEnabled);

            RegisterBehaviorsAndProcessors(
                container,
                uniqueAssemblies,
                processorBehaviors,
                typeof(RequestExceptionProcessorBehavior<,>),
                typeof(IRequestExceptionHandler<,,>),
                serviceConfig.RequestExceptionProcessorBehaviorEnabled);

            RegisterBehaviorsAndProcessors(
                container,
                uniqueAssemblies,
                processorBehaviors,
                typeof(RequestExceptionActionProcessorBehavior<,>),
                typeof(IRequestExceptionAction<,>),
                serviceConfig.RequestExceptionActionProcessorBehaviorEnabled);

            processorBehaviors.AddRange(serviceConfig.PipelineBehaviorTypes);

            container.Collection.Register(typeof(IPipelineBehavior<,>), processorBehaviors);
        }

        internal static void RegisterBehaviorsAndProcessors(
            Container container,
            Assembly[] uniqueAssemblies,
            List<Type> behaviorTypes,
            Type behaviourType,
            Type processorType,
            bool behaviourEnabled)
        {
            if (behaviourEnabled)
            {
                behaviorTypes.Add(behaviourType);
                RegisterIncludingGenericTypeDefinitions(container, uniqueAssemblies, processorType);
            }
            else
            {
                container.Collection.Register(processorType, Enumerable.Empty<Type>());
            }
        }

        internal static void RegisterIncludingGenericTypeDefinitions(
            Container container,
            Assembly[] uniqueAssemblies,
            Type processorType)
        {
            // we have to do this because by default, generic type definitions (such as the Constrained Notification Handler) won't be registered
            var implementingTypes = container.GetTypesToRegister(
                processorType,
                uniqueAssemblies,
                new TypesToRegisterOptions
                {
                    IncludeGenericTypeDefinitions = true,
                    IncludeComposites = false,
                });

            container.Collection.Register(processorType, implementingTypes);
        }
    }
}
