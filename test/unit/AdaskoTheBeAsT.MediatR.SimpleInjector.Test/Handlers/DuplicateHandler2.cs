#pragma warning disable S125 // Sections of code should not be commented out
    /*
     * SimpleInjector do not allow to register duplicates of given implementation
     * it can be registered by using container.Collection.Register but then it can be only
     * retrieved as IEnumerable
        public class DuplicateHandler2 : IRequestHandler<DuplicateTest, string>
        {
            public Task<string> Handle(DuplicateTest request, CancellationToken cancellationToken)
            {
                return Task.FromResult(nameof(DuplicateHandler2));
            }
        }
        */
#pragma warning restore S125 // Sections of code should not be commented out
