using System;
using AwesomeAssertions;
using AwesomeAssertions.Execution;
using Xunit;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.AspNet.Test
{
    public class MediatRSimpleInjectorAspNetConfigurationTest
    {
        private readonly MediatRSimpleInjectorAspNetConfiguration _sut;

        public MediatRSimpleInjectorAspNetConfigurationTest()
        {
            _sut = new MediatRSimpleInjectorAspNetConfiguration();
        }

        [Fact]
        public void ShouldHaveNonEmptyHttpContextCreator()
        {
            Action action = () => _sut.HttpContextCreator();

            // Assert
            using (new AssertionScope())
            {
                _sut.HttpContextCreator.Should().NotBeNull();
                action.Should().Throw<ArgumentNullException>();
            }
        }
    }
}
