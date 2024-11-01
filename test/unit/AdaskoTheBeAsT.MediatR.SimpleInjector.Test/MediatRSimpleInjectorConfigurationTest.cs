using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using MediatR.NotificationPublishers;
using MediatR.Pipeline;
using Moq;
using SimpleInjector;
using Xunit;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.Test;

public sealed class MediatRSimpleInjectorConfigurationTest
{
    private readonly MediatRSimpleInjectorConfiguration _sut;

    public MediatRSimpleInjectorConfigurationTest()
    {
        _sut = new MediatRSimpleInjectorConfiguration();
    }

    [Fact]
    public void ShouldContainDefaultSettingsAfterCreation()
    {
        // Arrange
        var config = new MediatRSimpleInjectorConfiguration();

        // Assert
        using (new AssertionScope())
        {
            config.Lifestyle.Should().Be(Lifestyle.Singleton);
            config.MediatorImplementationType.Should().Be(typeof(Mediator));
            config.MediatorInstanceCreator.Should().NotBeNull();
            config.MediatorInstanceCreator().Should().BeNull();
            config.RequestPreProcessorBehaviorEnabled.Should().BeFalse();
            config.RequestPostProcessorBehaviorEnabled.Should().BeFalse();
            config.RequestExceptionProcessorBehaviorEnabled.Should().BeFalse();
            config.RequestExceptionActionProcessorBehaviorEnabled.Should().BeFalse();
            config.AssembliesToScan.Should().BeEmpty();
            config.PipelineBehaviorTypes.Should().BeEmpty();
            config.NotificationPublisherType.Should().Be(typeof(ForeachAwaitPublisher));
            config.RequestPreProcessorTypes.Should().BeEmpty();
            config.RequestPostProcessorTypes.Should().BeEmpty();
            config.RequestExceptionHandlerTypes.Should().BeEmpty();
            config.RequestExceptionActionTypes.Should().BeEmpty();
        }
    }

    [Fact]
    public void AsSingletonShouldSetSingleton()
    {
        // Act
        var result = _sut.AsSingleton();

        // Assert
        result.Lifestyle.Should().Be(Lifestyle.Singleton);
    }

    [Fact]
    public void AsScopedShouldSetScoped()
    {
        // Act
        var result = _sut.AsScoped();

        // Assert
        result.Lifestyle.Should().Be(Lifestyle.Scoped);
    }

    [Fact]
    public void AsTransientShouldSetTransient()
    {
        // Act
        var result = _sut.AsTransient();

        // Assert
        result.Lifestyle.Should().Be(Lifestyle.Transient);
    }

    [Fact]
    public void UsingMediatorTypeShouldSetCorrectType()
    {
        // Act
        var result = _sut.Using<FakeMediator>();

        // Assert
        result.MediatorImplementationType.Should().Be<FakeMediator>();
    }

    [Fact]
    public void UsingMediatorInstanceCreatorShouldThrowExceptionWhenNullPassed()
    {
        // Arrange
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Action action = () => _sut.Using(instanceCreator: null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        // Act and Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void UsingMediatorInstanceCreatorShouldSetCorrectInstance()
    {
        // Arrange
        var instance = new Mock<IMediator>(MockBehavior.Strict).Object;

        // Act
        var result = _sut.Using(() => instance);

        // Assert
        using (new AssertionScope())
        {
            result.MediatorInstanceCreator.Should().NotBeNull();
            result.MediatorInstanceCreator.Should().NotThrow();
            var resultInstance = result.MediatorInstanceCreator();
            resultInstance.Should().Be(instance);
        }
    }

    [Fact]
    public void WithAssembliesToScanShouldSetAssembliesCorrectlyWhenPassedParams()
    {
        // Arrange
        var assemblyToScan = typeof(MediatRSimpleInjectorConfigurationTest).GetTypeInfo().Assembly;

        // Act
        var result = _sut.WithAssembliesToScan(
            assemblyToScan);

        // Assert
        using (new AssertionScope())
        {
            result.AssembliesToScan.Should().ContainSingle();
            result.AssembliesToScan.First().Should().BeSameAs(assemblyToScan);
        }
    }

    [Fact]
    public void WithAssembliesToScanShouldSetAssembliesCorrectlyWhenPassedIEnumerable()
    {
        // Arrange
        var assemblyToScan = typeof(MediatRSimpleInjectorConfigurationTest).GetTypeInfo().Assembly;

        // Act
#pragma warning disable S3878
        var result = _sut.WithAssembliesToScan(
            new[] { assemblyToScan });
#pragma warning restore S3878

        // Assert
        using (new AssertionScope())
        {
            result.AssembliesToScan.Should().ContainSingle();
            result.AssembliesToScan.First().Should().BeSameAs(assemblyToScan);
        }
    }

    [Fact]
    public void WithHandlerAssemblyMarkerTypesShouldSetAssembliesCorrectlyWhenPassedParams()
    {
        // Arrange
        var handlerAssemblyMarkerType = typeof(MediatRSimpleInjectorConfigurationTest);

        // Act
        var result = _sut.WithHandlerAssemblyMarkerTypes(
            handlerAssemblyMarkerType);

        // Assert
        // Assert
        using (new AssertionScope())
        {
            result.AssembliesToScan.Should().ContainSingle();
            result.AssembliesToScan.First().Should().BeSameAs(handlerAssemblyMarkerType.GetTypeInfo().Assembly);
        }
    }

    [Fact]
    public void WithHandlerAssemblyMarkerTypesShouldSetAssembliesCorrectlyWhenPassedIEnumerable()
    {
        // Arrange
        var handlerAssemblyMarkerType = typeof(MediatRSimpleInjectorConfigurationTest);

        // Act
#pragma warning disable S3878
        var result = _sut.WithHandlerAssemblyMarkerTypes(
            new[] { handlerAssemblyMarkerType });
#pragma warning restore S3878

        // Assert
        // Assert
        using (new AssertionScope())
        {
            result.AssembliesToScan.Should().ContainSingle();
            result.AssembliesToScan.First().Should().BeSameAs(handlerAssemblyMarkerType.GetTypeInfo().Assembly);
        }
    }

    [Theory]
    [InlineData(false, false, false, false)]
    [InlineData(false, false, false, true)]
    [InlineData(false, false, true, false)]
    [InlineData(false, false, true, true)]
    [InlineData(false, true, false, false)]
    [InlineData(false, true, false, true)]
    [InlineData(false, true, true, false)]
    [InlineData(false, true, true, true)]
    [InlineData(true, false, false, false)]
    [InlineData(true, false, false, true)]
    [InlineData(true, false, true, false)]
    [InlineData(true, false, true, true)]
    [InlineData(true, true, false, false)]
    [InlineData(true, true, false, true)]
    [InlineData(true, true, true, false)]
    [InlineData(true, true, true, true)]
    public void UsingBuiltinPipelineProcessorBehaviorsShouldSetFlagsIndividually(
        bool requestPreProcessorBehaviorEnabled,
        bool requestPostProcessorBehaviorEnabled,
        bool requestExceptionProcessorBehaviorEnabled,
        bool requestExceptionActionProcessorBehaviorEnabled)
    {
        // Act
        var result = _sut.UsingBuiltinPipelineProcessorBehaviors(
            requestPreProcessorBehaviorEnabled,
            requestPostProcessorBehaviorEnabled,
            requestExceptionProcessorBehaviorEnabled,
            requestExceptionActionProcessorBehaviorEnabled);

        // Assert
        using (new AssertionScope())
        {
            result.RequestPreProcessorBehaviorEnabled.Should().Be(requestPreProcessorBehaviorEnabled);
            result.RequestPostProcessorBehaviorEnabled.Should().Be(requestPostProcessorBehaviorEnabled);
            result.RequestExceptionProcessorBehaviorEnabled.Should().Be(requestExceptionProcessorBehaviorEnabled);
            result.RequestExceptionActionProcessorBehaviorEnabled.Should()
                .Be(requestExceptionActionProcessorBehaviorEnabled);
        }
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void UsingBuiltinPipelineProcessorBehaviorsShouldSetFlagsCollectively(bool processorBehaviorEnabled)
    {
        // Act
        var result = _sut.UsingBuiltinPipelineProcessorBehaviors(
            processorBehaviorEnabled);

        // Assert
        using (new AssertionScope())
        {
            result.RequestPreProcessorBehaviorEnabled.Should().Be(processorBehaviorEnabled);
            result.RequestPostProcessorBehaviorEnabled.Should().Be(processorBehaviorEnabled);
            result.RequestExceptionProcessorBehaviorEnabled.Should().Be(processorBehaviorEnabled);
            result.RequestExceptionActionProcessorBehaviorEnabled.Should()
                .Be(processorBehaviorEnabled);
            result.StreamPipelineBehaviorEnabled.Should()
                .Be(processorBehaviorEnabled);
        }
    }

    [Fact]
    public void UsingPipelineProcessorBehaviorsShouldThrowExceptionWhenInvalidTypePassedInParams()
    {
        // Arrange
        Action action = () => _sut.UsingPipelineProcessorBehaviors(typeof(object));

        // Act and Assert
        action.Should().Throw<InvalidPipelineBehaviorTypeException>();
    }

    [Fact]
    public void UsingPipelineProcessorBehaviorsShouldSetBehaviorsCorrectlyWhenPassedParams()
    {
        // Arrange
        var pipelineType = typeof(FakePipelineBehavior<,>);

        // Act
        var result = _sut.UsingPipelineProcessorBehaviors(pipelineType);

        // Assert
        using (new AssertionScope())
        {
            result.PipelineBehaviorTypes.Should().NotBeEmpty();
            result.PipelineBehaviorTypes.First().Should().Be(pipelineType);
        }
    }

    [Fact]
    public void UsingPipelineProcessorBehaviorsShouldThrowExceptionWhenInvalidTypePassedInIEnumerable()
    {
        // Arrange
        Action action = () => _sut.UsingPipelineProcessorBehaviors(new List<Type> { typeof(object) });

        // Act and Assert
        action.Should().Throw<InvalidPipelineBehaviorTypeException>();
    }

    [Fact]
    public void UsingPipelineProcessorBehaviorsShouldSetBehaviorsCorrectlyWhenPassedIEnumerable()
    {
        // Arrange
        var pipelineType = typeof(FakePipelineBehavior<,>);

        // Act
        var result = _sut.UsingPipelineProcessorBehaviors(new List<Type> { pipelineType });

        // Assert
        using (new AssertionScope())
        {
            result.PipelineBehaviorTypes.Should().NotBeEmpty();
            result.PipelineBehaviorTypes.First().Should().Be(pipelineType);
        }
    }

    [Fact]
    public void UsingStreamPipelineBehaviorsShouldThrowExceptionWhenInvalidTypePassedInParams()
    {
        // Arrange
        Action action = () => _sut.UsingStreamPipelineBehaviors(typeof(object));

        // Act and Assert
        action.Should().Throw<InvalidStreamPipelineBehaviorTypeException>();
    }

    [Fact]
    public void UsingStreamPipelineBehaviorsShouldSetBehaviorsCorrectlyWhenPassedParams()
    {
        // Arrange
        var pipelineType = typeof(FakeStreamPipelineBehavior<,>);

        // Act
        var result = _sut.UsingStreamPipelineBehaviors(pipelineType);

        // Assert
        using (new AssertionScope())
        {
            result.StreamPipelineBehaviorTypes.Should().NotBeEmpty();
            result.StreamPipelineBehaviorTypes.First().Should().Be(pipelineType);
        }
    }

    [Fact]
    public void UsingStreamPipelineBehaviorsShouldThrowExceptionWhenInvalidTypePassedInIEnumerable()
    {
        // Arrange
        Action action = () => _sut.UsingStreamPipelineBehaviors(new List<Type> { typeof(object) });

        // Act and Assert
        action.Should().Throw<InvalidStreamPipelineBehaviorTypeException>();
    }

    [Fact]
    public void UsingStreamPipelineBehaviorsShouldSetBehaviorsCorrectlyWhenPassedIEnumerable()
    {
        // Arrange
        var pipelineType = typeof(FakeStreamPipelineBehavior<,>);

        // Act
        var result = _sut.UsingStreamPipelineBehaviors(new List<Type> { pipelineType });

        // Assert
        using (new AssertionScope())
        {
            result.StreamPipelineBehaviorTypes.Should().NotBeEmpty();
            result.StreamPipelineBehaviorTypes.First().Should().Be(pipelineType);
        }
    }

    [Fact]
    public void WithNotificationPublisherForeachAwaitShouldSetProperNotificationPublisher()
    {
        // Act
        var result = _sut.WithNotificationPublisherForeachAwait();

        // Assert
        result.NotificationPublisherType.Should().Be(typeof(ForeachAwaitPublisher));
    }

    [Fact]
    public void WithNotificationPublisherTaskWhenAllShouldSetProperNotificationPublisher()
    {
        // Act
        var result = _sut.WithNotificationPublisherTaskWhenAll();

        // Assert
        result.NotificationPublisherType.Should().Be(typeof(TaskWhenAllPublisher));
    }

    [Fact]
    public void WithNotificationPublisherCustomShouldSetProperNotificationPublisher()
    {
        // Act
        var result =
            _sut.WithNotificationPublisherCustom<FakeNotificationPublisher>();

        // Assert
        result.NotificationPublisherType.Should().Be(typeof(FakeNotificationPublisher));
    }

    [Fact]
    public void WithRequestPreProcessorTypesShouldThrowExceptionWhenInvalidTypePassedInParams()
    {
        // Arrange
        Action action = () => _sut.WithRequestPreProcessorTypes(typeof(object));

        // Act and Assert
        action.Should().Throw<InvalidRequestPreProcessorTypeException>();
    }

    [Fact]
    public void WithRequestPreProcessorTypesShouldSetProcessorCorrectlyWhenPassedParams()
    {
        // Arrange
        var pipelineType = typeof(FakeRequestPreProcessor<>);

        // Act
        var result = _sut.WithRequestPreProcessorTypes(pipelineType);

        // Assert
        using (new AssertionScope())
        {
            result.RequestPreProcessorTypes.Should().NotBeEmpty();
            result.RequestPreProcessorTypes.First().Should().Be(pipelineType);
        }
    }

    [Fact]
    public void WithRequestPreProcessorTypesShouldThrowExceptionWhenInvalidTypePassedEnumerable()
    {
        // Arrange
        Action action = () => _sut.WithRequestPreProcessorTypes(new List<Type> { typeof(object) });

        // Act and Assert
        action.Should().Throw<InvalidRequestPreProcessorTypeException>();
    }

    [Fact]
    public void WithRequestPreProcessorTypesShouldSetProcessorCorrectlyWhenPassedEnumerable()
    {
        // Arrange
        var pipelineType = typeof(FakeRequestPreProcessor<>);

        // Act
        var result = _sut.WithRequestPreProcessorTypes(new List<Type> { pipelineType });

        // Assert
        using (new AssertionScope())
        {
            result.RequestPreProcessorTypes.Should().NotBeEmpty();
            result.RequestPreProcessorTypes.First().Should().Be(pipelineType);
        }
    }

    [Fact]
    public void WithRequestPostProcessorTypesShouldThrowExceptionWhenInvalidTypePassedInParams()
    {
        // Arrange
        Action action = () => _sut.WithRequestPostProcessorTypes(typeof(object));

        // Act and Assert
        action.Should().Throw<InvalidRequestPostProcessorTypeException>();
    }

    [Fact]
    public void WithRequestPostProcessorTypesShouldSetProcessorCorrectlyWhenPassedParams()
    {
        // Arrange
        var pipelineType = typeof(FakeRequestPostProcessor<,>);

        // Act
        var result = _sut.WithRequestPostProcessorTypes(pipelineType);

        // Assert
        using (new AssertionScope())
        {
            result.RequestPostProcessorTypes.Should().NotBeEmpty();
            result.RequestPostProcessorTypes.First().Should().Be(pipelineType);
        }
    }

    [Fact]
    public void WithRequestPostProcessorTypesShouldThrowExceptionWhenInvalidTypePassedEnumerable()
    {
        // Arrange
        Action action = () => _sut.WithRequestPostProcessorTypes(new List<Type> { typeof(object) });

        // Act and Assert
        action.Should().Throw<InvalidRequestPostProcessorTypeException>();
    }

    [Fact]
    public void WithRequestPostProcessorTypesShouldSetProcessorCorrectlyWhenPassedEnumerable()
    {
        // Arrange
        var pipelineType = typeof(FakeRequestPostProcessor<,>);

        // Act
        var result = _sut.WithRequestPostProcessorTypes(new List<Type> { pipelineType });

        // Assert
        using (new AssertionScope())
        {
            result.RequestPostProcessorTypes.Should().NotBeEmpty();
            result.RequestPostProcessorTypes.First().Should().Be(pipelineType);
        }
    }

    [Fact]
    public void WithRequestExceptionHandlerTypesShouldThrowExceptionWhenInvalidTypePassedInParams()
    {
        // Arrange
        Action action = () => _sut.WithRequestExceptionHandlerTypes(typeof(object));

        // Act and Assert
        action.Should().Throw<InvalidRequestExceptionHandlerTypeException>();
    }

    [Fact]
    public void WithRequestExceptionHandlerTypesShouldSetProcessorCorrectlyWhenPassedParams()
    {
        // Arrange
        var pipelineType = typeof(FakeRequestExceptionHandler<,,>);

        // Act
        var result = _sut.WithRequestExceptionHandlerTypes(pipelineType);

        // Assert
        using (new AssertionScope())
        {
            result.RequestExceptionHandlerTypes.Should().NotBeEmpty();
            result.RequestExceptionHandlerTypes.First().Should().Be(pipelineType);
        }
    }

    [Fact]
    public void WithRequestExceptionHandlerTypesShouldThrowExceptionWhenInvalidTypePassedEnumerable()
    {
        // Arrange
        Action action = () => _sut.WithRequestExceptionHandlerTypes(new List<Type> { typeof(object) });

        // Act and Assert
        action.Should().Throw<InvalidRequestExceptionHandlerTypeException>();
    }

    [Fact]
    public void WithRequestExceptionHandlerTypesShouldSetProcessorCorrectlyWhenPassedEnumerable()
    {
        // Arrange
        var pipelineType = typeof(FakeRequestExceptionHandler<,,>);

        // Act
        var result = _sut.WithRequestExceptionHandlerTypes(new List<Type> { pipelineType });

        // Assert
        using (new AssertionScope())
        {
            result.RequestExceptionHandlerTypes.Should().NotBeEmpty();
            result.RequestExceptionHandlerTypes.First().Should().Be(pipelineType);
        }
    }

    [Fact]
    public void WithRequestExceptionActionTypesShouldThrowExceptionWhenInvalidTypePassedInParams()
    {
        // Arrange
        Action action = () => _sut.WithRequestExceptionActionTypes(typeof(object));

        // Act and Assert
        action.Should().Throw<InvalidRequestExceptionActionTypeException>();
    }

    [Fact]
    public void WithRequestExceptionActionTypesShouldSetProcessorCorrectlyWhenPassedParams()
    {
        // Arrange
        var pipelineType = typeof(FakeRequestExceptionAction<,>);

        // Act
        var result = _sut.WithRequestExceptionActionTypes(pipelineType);

        // Assert
        using (new AssertionScope())
        {
            result.RequestExceptionActionTypes.Should().NotBeEmpty();
            result.RequestExceptionActionTypes.First().Should().Be(pipelineType);
        }
    }

    [Fact]
    public void WithRequestExceptionActionTypesShouldThrowExceptionWhenInvalidTypePassedEnumerable()
    {
        // Arrange
        Action action = () => _sut.WithRequestExceptionActionTypes(new List<Type> { typeof(object) });

        // Act and Assert
        action.Should().Throw<InvalidRequestExceptionActionTypeException>();
    }

    [Fact]
    public void WithRequestExceptionActionTypesShouldSetProcessorCorrectlyWhenPassedEnumerable()
    {
        // Arrange
        var pipelineType = typeof(FakeRequestExceptionAction<,>);

        // Act
        var result = _sut.WithRequestExceptionActionTypes(new List<Type> { pipelineType });

        // Assert
        using (new AssertionScope())
        {
            result.RequestExceptionActionTypes.Should().NotBeEmpty();
            result.RequestExceptionActionTypes.First().Should().Be(pipelineType);
        }
    }

#pragma warning disable CA1812
    private sealed class FakeMediator
        : IMediator
    {
        public Task<TResponse> Send<TResponse>(
            IRequest<TResponse> request,
            CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public Task Send<TRequest>(
            TRequest request,
            CancellationToken cancellationToken = default)
            where TRequest : IRequest
        {
            throw new NotSupportedException();
        }

        public Task<object?> Send(
            object request,
            CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public Task Publish(
            object notification,
            CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public Task Publish<TNotification>(
            TNotification notification,
            CancellationToken cancellationToken = default)
            where TNotification : INotification
        {
            throw new NotSupportedException();
        }

        public IAsyncEnumerable<TResponse> CreateStream<TResponse>(
            IStreamRequest<TResponse> request,
            CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public IAsyncEnumerable<object?> CreateStream(
            object request,
            CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }
    }

    private sealed class FakePipelineBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        public Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }
    }

    private sealed class FakeStreamPipelineBehavior<TRequest, TResponse>
        : IStreamPipelineBehavior<TRequest, TResponse>
        where TRequest : IStreamRequest<TResponse>
    {
        public IAsyncEnumerable<TResponse> Handle(
            TRequest request,
            StreamHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }
    }

    private sealed class FakeNotificationPublisher
        : INotificationPublisher
    {
        public Task Publish(
            IEnumerable<NotificationHandlerExecutor> handlerExecutors,
            INotification notification,
            CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }
    }

    private sealed class FakeRequestPreProcessor<TRequest>
        : IRequestPreProcessor<TRequest>
        where TRequest : notnull
    {
        public Task Process(
            TRequest request,
            CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    private sealed class FakeRequestPostProcessor<TRequest, TResponse>
        : IRequestPostProcessor<TRequest, TResponse>
        where TRequest : notnull
    {
        public Task Process(
            TRequest request,
            TResponse response,
            CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    private sealed class FakeRequestExceptionHandler<TRequest, TResponse, TException>
        : IRequestExceptionHandler<TRequest, TResponse, TException>
        where TRequest : notnull
        where TException : Exception
    {
        public Task Handle(
            TRequest request,
            TException exception,
            RequestExceptionHandlerState<TResponse> state,
            CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    private sealed class FakeRequestExceptionAction<TRequest, TException>
        : IRequestExceptionAction<TRequest, TException>
        where TRequest : notnull
        where TException : Exception
    {
        public Task Execute(
            TRequest request,
            TException exception,
            CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
#pragma warning restore CA1812
}
