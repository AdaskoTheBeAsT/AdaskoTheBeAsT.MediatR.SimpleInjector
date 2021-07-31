using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace TestApp
{
    public static class Runner
    {
        public static Task RunAsync(IMediator mediator, WrappingWriter writer, string projectName)
        {
            if (mediator is null)
            {
                throw new ArgumentNullException(nameof(mediator));
            }

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            return RunInternalAsync(mediator, writer, projectName);
        }

#pragma warning disable MA0051 // Method is too long
        internal static async Task RunInternalAsync(
            IMediator mediator,
            WrappingWriter writer,
            string projectName)
        {
            await writer.WriteLineAsync("===============").ConfigureAwait(false);
            await writer.WriteLineAsync(projectName).ConfigureAwait(false);
            await writer.WriteLineAsync("===============").ConfigureAwait(false);

            await writer.WriteLineAsync("Sending Ping...").ConfigureAwait(false);
            var pong = await mediator.Send(new Ping { Message = nameof(Ping) }).ConfigureAwait(false);
            await writer.WriteLineAsync("Received: " + pong.Message).ConfigureAwait(false);

            await writer.WriteLineAsync("Publishing Pinged...").ConfigureAwait(false);
            await mediator.Publish(new Pinged()).ConfigureAwait(false);

            await writer.WriteLineAsync("Publishing Ponged...").ConfigureAwait(false);
            var failedPong = false;
            try
            {
                await mediator.Publish(new Ponged()).ConfigureAwait(false);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                failedPong = true;
                await writer.WriteLineAsync(e.ToString()).ConfigureAwait(false);
            }

            var failedJing = false;
            await writer.WriteLineAsync("Sending Jing...").ConfigureAwait(false);
            try
            {
                await mediator.Send(new Jing { Message = nameof(Jing) }).ConfigureAwait(false);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                failedJing = true;
                await writer.WriteLineAsync(e.ToString()).ConfigureAwait(false);
            }

            await writer.WriteLineAsync("---------------").ConfigureAwait(false);
            var contents = writer.Contents;
            var order = new[]
            {
                contents.IndexOf("- Starting Up", StringComparison.OrdinalIgnoreCase),
                contents.IndexOf("-- Handling Request", StringComparison.OrdinalIgnoreCase),
                contents.IndexOf("--- Handled Ping", StringComparison.OrdinalIgnoreCase),
                contents.IndexOf("-- Finished Request", StringComparison.OrdinalIgnoreCase),
                contents.IndexOf("- All Done", StringComparison.OrdinalIgnoreCase),
                contents.IndexOf("- All Done with Ping", StringComparison.OrdinalIgnoreCase),
            };

            var results = new RunResults
            {
                RequestHandlers = contents.Contains("--- Handled Ping:", StringComparison.OrdinalIgnoreCase),
                VoidRequestsHandlers = contents.Contains("--- Handled Jing:", StringComparison.OrdinalIgnoreCase),
                PipelineBehaviors = contents.Contains("-- Handling Request", StringComparison.OrdinalIgnoreCase),
                RequestPreProcessors = contents.Contains("- Starting Up", StringComparison.OrdinalIgnoreCase),
                RequestPostProcessors = contents.Contains("- All Done", StringComparison.OrdinalIgnoreCase),
                ConstrainedGenericBehaviors =
                    contents.Contains("- All Done with Ping", StringComparison.OrdinalIgnoreCase) && !failedJing,
                OrderedPipelineBehaviors = order.SequenceEqual(order.OrderBy(i => i)),
                NotificationHandler = contents.Contains("Got pinged async", StringComparison.OrdinalIgnoreCase),
                MultipleNotificationHandlers =
                    contents.Contains("Got pinged async", StringComparison.OrdinalIgnoreCase) && contents.Contains(
                        "Got pinged also async",
                        StringComparison.OrdinalIgnoreCase),
                ConstrainedGenericNotificationHandler =
                    contents.Contains("Got pinged constrained async", StringComparison.OrdinalIgnoreCase) &&
                    !failedPong,
                CovariantNotificationHandler = contents.Contains("Got notified", StringComparison.OrdinalIgnoreCase),
            };

            await writer.WriteLineAsync($"Request Handler...................{(results.RequestHandlers ? "Y" : "N")}")
                .ConfigureAwait(false);
            await writer.WriteLineAsync($"Void Request Handler..............{(results.VoidRequestsHandlers ? "Y" : "N")}")
                .ConfigureAwait(false);
            await writer.WriteLineAsync($"Pipeline Behavior.................{(results.PipelineBehaviors ? "Y" : "N")}")
                .ConfigureAwait(false);
            await writer.WriteLineAsync($"Pre-Processor.....................{(results.RequestPreProcessors ? "Y" : "N")}")
                .ConfigureAwait(false);
            await writer.WriteLineAsync($"Post-Processor....................{(results.RequestPostProcessors ? "Y" : "N")}")
                .ConfigureAwait(false);
            await writer.WriteLineAsync($"Constrained Post-Processor........{(results.ConstrainedGenericBehaviors ? "Y" : "N")}")
                .ConfigureAwait(false);
            await writer.WriteLineAsync($"Ordered Behaviors.................{(results.OrderedPipelineBehaviors ? "Y" : "N")}")
                .ConfigureAwait(false);
            await writer.WriteLineAsync($"Notification Handler..............{(results.NotificationHandler ? "Y" : "N")}")
                .ConfigureAwait(false);
            await writer.WriteLineAsync($"Notification Handlers.............{(results.MultipleNotificationHandlers ? "Y" : "N")}")
                .ConfigureAwait(false);
            await writer.WriteLineAsync($"Constrained Notification Handler..{(results.ConstrainedGenericNotificationHandler ? "Y" : "N")}")
                .ConfigureAwait(false);
            await writer.WriteLineAsync($"Covariant Notification Handler....{(results.CovariantNotificationHandler ? "Y" : "N")}")
                .ConfigureAwait(false);
        }
#pragma warning restore MA0051 // Method is too long
    }
}
