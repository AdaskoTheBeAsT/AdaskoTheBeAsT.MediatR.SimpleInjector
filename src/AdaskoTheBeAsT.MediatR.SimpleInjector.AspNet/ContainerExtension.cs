using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using MediatR;
using MediatR.Pipeline;
using SimpleInjector;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.AspNet
{
#pragma warning disable RCS1247 // Fix documentation comment tag.
    /// <summary>
    /// Extensions to scan for MediatR handlers and registers them in SimpleInjector.
    /// - Scans for any handler interface implementations and registers them as <see cref="Lifestyle.Transient"/>
    /// - Scans for any
    ///   <see cref="IRequestPreProcessor{TRequest}"/>
    ///   <see cref="IRequestPostProcessor{TRequest,TResponse}"/>
    ///   <see cref="IRequestExceptionHandler{TRequest,TResponse,TException}"/>
    ///   <see cref="IRequestExceptionAction{TRequest,TException}"/>
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
    /// <code>container.Register&lt;Type, Type&gt;();</code>
    /// </summary>
    public static class ContainerExtension
#pragma warning restore RCS1247 // Fix documentation comment tag.
    {
        /// <summary>
        /// Registers handlers and mediator types from the specified assemblies.
        /// </summary>
        /// <param name="container"><see cref="Container"/>.</param>
        /// <param name="httpContextFunc"><see cref="HttpContextBase"/> creator function.</param>
        /// <param name="assemblies">Assemblies to scan.</param>
        /// <returns><see cref="Container"/>.</returns>
        public static Container AddMediatRAspNet(
            this Container container,
            Func<HttpContextBase>? httpContextFunc,
            params Assembly[] assemblies)
        {
            return AddMediatRAspNet(
                container,
                config =>
            {
                config.WithAssembliesToScan(assemblies);
                if (httpContextFunc != null)
                {
                    config.UsingHttpContextCreator(httpContextFunc);
                }
            });
        }

        /// <summary>
        /// Registers handlers and mediator types from the specified assemblies.
        /// </summary>
        /// <param name="container"><see cref="Container"/>.</param>
        /// <param name="httpContextFunc"><see cref="HttpContextBase"/> creator function.</param>
        /// <param name="assemblies">Assemblies to scan.</param>
        /// <returns><see cref="Container"/>.</returns>
        public static Container AddMediatRAspNet(
            this Container container,
            Func<HttpContextBase>? httpContextFunc,
            IEnumerable<Assembly> assemblies)
        {
            return AddMediatRAspNet(
                container,
                config =>
            {
                config.WithAssembliesToScan(assemblies);
                if (httpContextFunc != null)
                {
                    config.UsingHttpContextCreator(httpContextFunc);
                }
            });
        }

        /// <summary>
        /// Registers handlers and mediator types from the assemblies that contain the specified types.
        /// </summary>
        /// <param name="container"><see cref="Container"/>.</param>
        /// <param name="httpContextFunc"><see cref="HttpContextBase"/> creator function.</param>
        /// <param name="handlerAssemblyMarkerTypes">Types used to mark assemblies to scan.</param>.
        /// <returns><see cref="Container"/>.</returns>
        public static Container AddMediatRAspNet(
            this Container container,
            Func<HttpContextBase>? httpContextFunc,
            params Type[] handlerAssemblyMarkerTypes)
        {
            return AddMediatRAspNet(
                container,
                config =>
                {
                    config.WithHandlerAssemblyMarkerTypes(handlerAssemblyMarkerTypes);
                    if (httpContextFunc != null)
                    {
                        config.UsingHttpContextCreator(httpContextFunc);
                    }
                });
        }

        /// <summary>
        /// Registers handlers and mediator types from the assemblies that contain the specified types.
        /// </summary>
        /// <param name="container"><see cref="Container"/>.</param>
        /// <param name="httpContextFunc"><see cref="HttpContextBase"/> creator function.</param>
        /// <param name="handlerAssemblyMarkerTypes">Types used to mark assemblies to scan.</param>
        /// <returns><see cref="Container"/>.</returns>
        public static Container AddMediatRAspNet(
            this Container container,
            Func<HttpContextBase>? httpContextFunc,
            IEnumerable<Type> handlerAssemblyMarkerTypes)
        {
            return AddMediatRAspNet(
                container,
                config =>
                {
                    config.WithHandlerAssemblyMarkerTypes(handlerAssemblyMarkerTypes);
                    if (httpContextFunc != null)
                    {
                        config.UsingHttpContextCreator(httpContextFunc);
                    }
                });
        }

        /// <summary>
        /// Registers handlers and mediator types from the specified assemblies.
        /// </summary>
        /// <param name="container"><see cref="Container"/>.</param>
        /// <param name="configuration">The action used to configure the options.</param>
        /// <returns><see cref="Container"/>.</returns>
        public static Container AddMediatRAspNet(
            this Container container,
            Action<MediatRSimpleInjectorAspNetConfiguration>? configuration)
        {
            var serviceConfig = new MediatRSimpleInjectorAspNetConfiguration();
            configuration?.Invoke(serviceConfig);
            if (serviceConfig.Lifestyle == Lifestyle.Singleton)
            {
                throw new InvalidAspNetLifetimeException(
                    "AspNet Mediator cannot be singleton as HttpContextWrapper needs to be registered as scoped");
            }

            var containerRef = container.SetupContainer(serviceConfig);
            containerRef.RegisterDecorator<IMediator, HttpResponseClientDisconnectedTokenMediatorDecorator>(
                serviceConfig.Lifestyle);

            containerRef.Register(
                () =>
                {
                    var instance = serviceConfig.HttpContextCreator();
                    return instance ?? throw new HttpContextCreatorReturnsNullException("HttpContext creator returned null");
                },
                Lifestyle.Scoped);
            return containerRef;
        }
    }
}
