using System;
using FluentAssertions;
using Xunit;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.Test
{
    /// <summary>
    /// Based on https://gist.github.com/klmr/4174727.
    /// </summary>
    public sealed class TypeExtensionsTest
    {
        [Fact]
        public void ShouldGenericClassBeAssignableToGenericBaseClass()
        {
            // Act
            var result = typeof(Derived<>).IsAssignableToGenericType(typeof(Base<>));

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void ShouldConcreteClassBeAssignableToGenericBaseClass()
        {
            // Act
            var result = typeof(Derived<object>).IsAssignableToGenericType(typeof(Base<>));

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void ShouldGenericClassBeAssignableToGenericInterface()
        {
            // Act
            var result = typeof(Derived<>).IsAssignableToGenericType(typeof(IBase<>));

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void ShouldConcreteClassBeAssignableToGenericInterface()
        {
            // Act
            var result = typeof(Derived<object>).IsAssignableToGenericType(typeof(IBase<>));

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void ShouldGenericClassBeAssignableToGenericSelf()
        {
            // Act
            var result = typeof(Derived<>).IsAssignableToGenericType(typeof(Derived<>));

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void ShouldConcreteClassBeAssignableToGenericSelf()
        {
            // Act
            var result = typeof(Derived<object>).IsAssignableToGenericType(typeof(Derived<>));

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void ShouldGenericClassBeAssignableToGenericTransitiveClass()
        {
            // Act
            var result = typeof(Derived2<>).IsAssignableToGenericType(typeof(Base<>));

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void ShouldConcreteClassBeAssignableToGenericTransitiveClass()
        {
            // Act
            var result = typeof(Derived2<object>).IsAssignableToGenericType(typeof(Base<>));

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void ShouldGenericInterfaceBeAssignableToGenericTransitiveInterface()
        {
            // Act
            var result = typeof(DerivedI<>).IsAssignableToGenericType(typeof(IBase<>));

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void ShouldConcreteInterfaceBeAssignableToGenericTransitiveInterface()
        {
            // Act
            var result = typeof(DerivedI<object>).IsAssignableToGenericType(typeof(IBase<>));

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void ShouldGenericClassBeAssignableToGenericTransitiveInterface()
        {
            // Act
            var result = typeof(Derived2<>).IsAssignableToGenericType(typeof(IBase<>));

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void ShouldConcreteClassBeAssignableToGenericTransitiveInterface()
        {
            // Act
            var result = typeof(Derived2<object>).IsAssignableToGenericType(typeof(IBase<>));

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void ShouldNullableTypeBeAssignableToNullable()
        {
            // Act
            var result = typeof(int?).IsAssignableToGenericType(typeof(Nullable<>));

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void ShouldNotGenericClassBeAssignableToObject()
        {
            // Act
            var result = typeof(Derived<>).IsAssignableToGenericType(typeof(object));

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void ShouldNotConcreteClassBeAssignableToObject()
        {
            // Act
            var result = typeof(Derived<int>).IsAssignableToGenericType(typeof(object));

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void ShouldNotGenericClassBeAssignableToNonBaseGenericClass()
        {
            // Act
            var result = typeof(Derived2<>).IsAssignableToGenericType(typeof(DerivedI<>));

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void ShouldNotConcreteClassBeAssignableToNonBaseGenericClass()
        {
            // Act
            var result = typeof(Derived2<int>).IsAssignableToGenericType(typeof(DerivedI<>));

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void ShouldNotGenericClassBeAssignableToNonBaseGenericInterface()
        {
            // Act
            var result = typeof(Derived<>).IsAssignableToGenericType(typeof(IDerived<>));

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void ShouldNotConcreteClassBeAssignableToNonBaseGenericInterface()
        {
            // Act
            var result = typeof(Derived<int>).IsAssignableToGenericType(typeof(IDerived<>));

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void ShouldNotGenericClassBeAssignableToConcreteClass()
        {
            // Act
            var result = typeof(Derived<>).IsAssignableToGenericType(typeof(Base<object>));

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void ShouldNotConcreteClassBeAssignableToConcreteClass()
        {
            // Act
            var result = typeof(Derived<int>).IsAssignableToGenericType(typeof(Base<object>));

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void ShouldNotGenericClassBeAssignableToConcreteInterface()
        {
            // Act
            var result = typeof(Derived<>).IsAssignableToGenericType(typeof(IBase<object>));

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void ShouldNotConcreteClassBeAssignableToConcreteInterface()
        {
            // Act
            var result = typeof(Derived<int>).IsAssignableToGenericType(typeof(IBase<object>));

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void ShouldNotConcreteNonNullableToNullable()
        {
            // Act
            var result = typeof(int).IsAssignableToGenericType(typeof(Nullable<>));

            // Assert
            result.Should().BeFalse();
        }

#pragma warning disable CA1812
#pragma warning disable S2326 // Unused type parameters should be removed
#pragma warning disable SA1201 // Elements should appear in the correct order
        private class Base<T>
        {
        }

        // ReSharper disable once UnusedTypeParameter
        private interface IBase<T>
        {
        }

        private interface IDerived<T>
            : IBase<T>
        {
        }

        private class Derived<T>
            : Base<T>,
                IBase<T>
        {
        }

        private class Derived2<T>
            : Derived<T>
        {
        }

        private class DerivedI<T>
            : IDerived<T>
        {
        }
#pragma warning restore SA1201 // Elements should appear in the correct order
#pragma warning restore S2326 // Unused type parameters should be removed
#pragma warning restore CA1812
    }
}
