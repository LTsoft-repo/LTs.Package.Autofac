using Autofac;
using JetBrains.Annotations;
using LTs.Autofac.Extensions;

namespace LTs.Autofac.test.Extensions;

public class AutofacRegisterExtensionsTests
{
    #region RegisterType
    [ Fact ]
    public void RegisterType_WithCorrectParameters_RegistersTheType()
    {
        // Arrange
        var builder = new ContainerBuilder();

        // Act
        builder.RegisterType<IService, ServiceImplementation>();

        // Assert
        var container = builder.Build();
        container.Should().NotBeNull();

        var service = container.Resolve<IService>();
        service.Should().NotBeNull();
        service.Should().BeOfType<ServiceImplementation>();

        var services = container.Resolve<IEnumerable<IService>>();

        services.Should().ContainSingle()
                .Which.Should().BeOfType<ServiceImplementation>();
    }
    #endregion

    #region RegisterGeneric
    [ Fact ]
    public void RegisterGeneric_WithCorrectParameters_ReplacesTheType()
    {
        // Arrange
        var builder = new ContainerBuilder();

        // Act
        builder.RegisterGeneric<ITypedService<object>, TypedServiceImplementation<object>>();

        // Assert
        var container = builder.Build();
        container.Should().NotBeNull();

        var service = container.Resolve<ITypedService<string>>();
        service.Should().NotBeNull();
        service.Should().BeOfType<TypedServiceImplementation<string>>();
    }

    [ Fact ]
    public void RegisterGeneric_WithNonGeneric_Throws()
    {
        // Arrange
        var builder = new ContainerBuilder();

        // Act
        var act = () => builder.RegisterGeneric<IService, TypedServiceImplementation<object>>();

        // Assert
        act.Should().Throw<ArgumentException>()
           .WithMessage( "TInterface is not a generic type." );
    }
    #endregion
}

// ReSharper disable once RedundantTypeDeclarationBody
public interface IService { }

[ UsedImplicitly ]
// ReSharper disable once RedundantTypeDeclarationBody
public interface ITypedService<T> : IService { }

[ UsedImplicitly ]
// ReSharper disable once RedundantTypeDeclarationBody
public class ServiceImplementation : IService { }

[ UsedImplicitly ]
// ReSharper disable once RedundantTypeDeclarationBody
public class TypedServiceImplementation<T> : ITypedService<T> { }