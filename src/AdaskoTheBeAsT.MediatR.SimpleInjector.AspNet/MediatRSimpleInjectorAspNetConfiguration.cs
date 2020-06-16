using System;
using System.Web;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.AspNet
{
    public class MediatRSimpleInjectorAspNetConfiguration
        : MediatRSimpleInjectorConfiguration
    {
        public MediatRSimpleInjectorAspNetConfiguration()
        {
            this.AsScoped();
            HttpContextCreator = () => new HttpContextWrapper(HttpContext.Current);
        }

        public Func<HttpContextBase> HttpContextCreator { get; private set; }

        /// <summary>
        /// Register custom HttpContextBase instance creator
        /// instead of default one <see cref="HttpContextBase"/>.
        /// </summary>
        /// <param name="instanceCreator">Custom <see cref="HttpContextBase"/> instance creator function.</param>
        /// <returns><see cref="MediatRSimpleInjectorAspNetConfiguration"/>
        /// with custom <see cref="HttpContextBase"/> instance creator function.</returns>
        public MediatRSimpleInjectorAspNetConfiguration UsingHttpContextCreator(Func<HttpContextBase> instanceCreator)
        {
            HttpContextCreator =
                instanceCreator
                ?? throw new ArgumentNullException(nameof(instanceCreator));
            return this;
        }
    }
}
