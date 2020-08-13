using System;
using FluentAssertions;
using SimpleInjector;
using Xunit;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.Test
{
    public class ContainerExtensionTests
    {
        [Fact]
        public void ShouldThrowExceptionWhenNullContainerPassed()
        {
            // Arrange
            var container = default(Container);
#pragma warning disable CS8604 // Possible null reference argument.
            Action action = () => ContainerExtension.AddMediatR(container, _ => { });
#pragma warning restore CS8604 // Possible null reference argument.

            // Act & Assert
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
