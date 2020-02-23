using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace TestApp
{
#pragma warning disable SA1402 // File may only contain a single type
    public static class Runner
    {
        public static Task Run(IMediator mediator, WrappingWriter writer, string projectName)
        {
            if (mediator is null)
            {
                throw new ArgumentNullException(nameof(mediator));
            }

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            return RunInternal(mediator, writer, projectName);
        }

        internal static async Task RunInternal(
            IMediator mediator,
            WrappingWriter writer,
            string projectName)
        {
            await writer.WriteLineAsync("===============");
            await writer.WriteLineAsync(projectName);
            await writer.WriteLineAsync("===============");

            await writer.WriteLineAsync("Sending Ping...");
            var pong = await mediator.Send(new Ping { Message = "Ping" });
            await writer.WriteLineAsync("Received: " + pong.Message);

            await writer.WriteLineAsync("Publishing Pinged...");
            await mediator.Publish(new Pinged());

            await writer.WriteLineAsync("Publishing Ponged...");
            var failedPong = false;
            try
            {
                await mediator.Publish(new Ponged());
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                failedPong = true;
                await writer.WriteLineAsync(e.ToString());
            }

            bool failedJing = false;
            await writer.WriteLineAsync("Sending Jing...");
            try
            {
                await mediator.Send(new Jing { Message = "Jing" });
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                failedJing = true;
                await writer.WriteLineAsync(e.ToString());
            }

            await writer.WriteLineAsync("---------------");
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

            await writer.WriteLineAsync($"Request Handler...................{(results.RequestHandlers ? "Y" : "N")}");
            await writer.WriteLineAsync($"Void Request Handler..............{(results.VoidRequestsHandlers ? "Y" : "N")}");
            await writer.WriteLineAsync($"Pipeline Behavior.................{(results.PipelineBehaviors ? "Y" : "N")}");
            await writer.WriteLineAsync($"Pre-Processor.....................{(results.RequestPreProcessors ? "Y" : "N")}");
            await writer.WriteLineAsync($"Post-Processor....................{(results.RequestPostProcessors ? "Y" : "N")}");
            await writer.WriteLineAsync($"Constrained Post-Processor........{(results.ConstrainedGenericBehaviors ? "Y" : "N")}");
            await writer.WriteLineAsync($"Ordered Behaviors.................{(results.OrderedPipelineBehaviors ? "Y" : "N")}");
            await writer.WriteLineAsync($"Notification Handler..............{(results.NotificationHandler ? "Y" : "N")}");
            await writer.WriteLineAsync($"Notification Handlers.............{(results.MultipleNotificationHandlers ? "Y" : "N")}");
            await writer.WriteLineAsync($"Constrained Notification Handler..{(results.ConstrainedGenericNotificationHandler ? "Y" : "N")}");
            await writer.WriteLineAsync($"Covariant Notification Handler....{(results.CovariantNotificationHandler ? "Y" : "N")}");
        }
    }

    public class RunResults
    {
        public bool RequestHandlers { get; set; }

        public bool VoidRequestsHandlers { get; set; }

        public bool PipelineBehaviors { get; set; }

        public bool RequestPreProcessors { get; set; }

        public bool RequestPostProcessors { get; set; }

        public bool OrderedPipelineBehaviors { get; set; }

        public bool ConstrainedGenericBehaviors { get; set; }

        public bool NotificationHandler { get; set; }

        public bool MultipleNotificationHandlers { get; set; }

        public bool CovariantNotificationHandler { get; set; }

        public bool ConstrainedGenericNotificationHandler { get; set; }
    }

    public class WrappingWriter : TextWriter
    {
        private readonly TextWriter _innerWriter;
        private readonly StringBuilder _stringWriter = new StringBuilder();

        public WrappingWriter(TextWriter innerWriter)
        {
            _innerWriter = innerWriter;
        }

        public override Encoding Encoding => _innerWriter.Encoding;

        public string Contents => _stringWriter.ToString();

        public override void Write(char value)
        {
            _stringWriter.Append(value);
            _innerWriter.Write(value);
        }

        public override Task WriteLineAsync(string? value)
        {
            _stringWriter.AppendLine(value);
            return _innerWriter.WriteLineAsync(value);
        }
    }
#pragma warning restore SA1402 // File may only contain a single type
}
