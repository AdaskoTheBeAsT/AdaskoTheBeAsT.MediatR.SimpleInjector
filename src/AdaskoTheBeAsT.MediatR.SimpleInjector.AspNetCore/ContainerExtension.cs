using System;
using System.Collections.Generic;
using System.Reflection;
using MediatR;
using MediatR.Pipeline;
using SimpleInjector;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.AspNetCore
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
        public static Container AddMediatRAspNetCore(
            this Container container,
            params Assembly[] assemblies)
        {
            return AddMediatRAspNetCore(container, config => config.WithAssembliesToScan(assemblies));
        }

        /// <summary>
        /// Registers handlers and mediator types from the specified assemblies.
        /// </summary>
        /// <param name="container"><see cref="Container"/>.</param>
        /// <param name="assemblies">Assemblies to scan.</param>
        /// <returns><see cref="Container"/>.</returns>
        public static Container AddMediatRAspNetCore(
            this Container container,
            IEnumerable<Assembly> assemblies)
        {
            return AddMediatRAspNetCore(container, config => config.WithAssembliesToScan(assemblies));
        }

        /// <summary>
        /// Registers handlers and mediator types from the assemblies that contain the specified types.
        /// </summary>
        /// <param name="container"><see cref="Container"/>.</param>
        /// <param name="handlerAssemblyMarkerTypes">Types used to mark assemblies to scan.</param>.
        /// <returns><see cref="Container"/>.</returns>
        public static Container AddMediatRAspNetCore(
            this Container container,
            params Type[] handlerAssemblyMarkerTypes)
        {
            return AddMediatRAspNetCore(
                container,
                config => config.WithHandlerAssemblyMarkerTypes(handlerAssemblyMarkerTypes));
        }

        /// <summary>
        /// Registers handlers and mediator types from the assemblies that contain the specified types.
        /// </summary>
        /// <param name="container"><see cref="Container"/>.</param>
        /// <param name="handlerAssemblyMarkerTypes">Types used to mark assemblies to scan.</param>
        /// <returns><see cref="Container"/>.</returns>
        public static Container AddMediatRAspNetCore(
            this Container container,
            IEnumerable<Type> handlerAssemblyMarkerTypes)
        {
            return AddMediatRAspNetCore(
                container,
                config => config.WithHandlerAssemblyMarkerTypes(handlerAssemblyMarkerTypes));
        }

        /// <summary>
        /// Registers handlers and mediator types from the specified assemblies.
        /// </summary>
        /// <param name="container"><see cref="Container"/>.</param>
        /// <param name="configuration">The action used to configure the options.</param>
        /// <returns><see cref="Container"/>.</returns>
        public static Container AddMediatRAspNetCore(
            this Container container,
            Action<MediatRSimpleInjectorConfiguration>? configuration)
        {
            var serviceConfig = new MediatRSimpleInjectorConfiguration();
            configuration?.Invoke(serviceConfig);

            var containerRef = container.SetupContainer(serviceConfig);
            containerRef.RegisterDecorator<IMediator, HttpRequestAbortedCancellationTokenMediatorDecorator>(serviceConfig.Lifestyle);
            return containerRef;
        }
    }
}
