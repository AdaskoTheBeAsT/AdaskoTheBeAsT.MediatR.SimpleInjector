namespace AdaskoTheBeAsT.MediatR.SimpleInjector.Test;

public sealed partial class TypeExtensionsTest
{
    private interface IDerived<T>
        : IBase<T>;
}
