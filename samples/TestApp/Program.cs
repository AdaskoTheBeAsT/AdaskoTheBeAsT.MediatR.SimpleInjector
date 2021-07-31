using System;
using System.IO;
using System.Threading.Tasks;
using AdaskoTheBeAsT.MediatR.SimpleInjector;
using MediatR;
using MediatR.Pipeline;
using SimpleInjector;

namespace TestApp
{
    public static class Program
    {
        private static Container? container;

        public static Task Main()
        {
            using var writer = new WrappingWriter(Console.Out);
            var mediator = BuildMediator(writer);
            return Runner.RunAsync(mediator, writer, "SimpleInjector");
        }

        private static IMediator BuildMediator(WrappingWriter writer)
        {
            container = new Container();
            container.RegisterInstance(typeof(TextWriter), writer);
            container.Register(typeof(IRequestPostProcessor<,>), typeof(ConstrainedRequestPostProcessor<,>));
            container.Register(typeof(INotificationHandler<>), typeof(ConstrainedPingedHandler<>));

            container.AddMediatR(
                config =>
                {
                    config.WithHandlerAssemblyMarkerTypes(typeof(Ping));
                    config.UsingBuiltinPipelineProcessorBehaviors(true);
                    config.UsingPipelineProcessorBehaviors(typeof(GenericPipelineBehavior<,>));
                });

            foreach (var service in container.GetCurrentRegistrations())
            {
                Console.WriteLine(service.ServiceType + " - " + service.Registration.ImplementationType);
            }

            return container.GetInstance<IMediator>();
        }
    }
}
