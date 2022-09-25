using System;
using FluentAssertions;
using SimpleInjector;
using Xunit;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.Test;

public class ContainerExtensionTests
{
    [Fact]
    public void ShouldThrowExceptionWhenNullContainerPassed()
    {
        // Arrange
        const Container? container = null;
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CC0026 // Call Extension Method As Extension

        // ReSharper disable once InvokeAsExtensionMethod
        Action action = () => ContainerExtension.AddMediatR(container!, _ => { });
#pragma warning restore CC0026 // Call Extension Method As Extension
#pragma warning restore CS8604 // Possible null reference argument.

        // Act and Assert
        action.Should().Throw<ArgumentNullException>();
    }
}
