namespace TestApp
{
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
}
