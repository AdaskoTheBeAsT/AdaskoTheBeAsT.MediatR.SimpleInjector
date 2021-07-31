namespace AdaskoTheBeAsT.MediatR.SimpleInjector.Test
{
    public sealed partial class TypeExtensionsTest
    {
        private class Derived<T>
            : Base<T>,
                IBase<T>
        {
        }
    }
}
