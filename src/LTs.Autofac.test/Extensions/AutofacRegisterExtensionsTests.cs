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

    //#region ReplaceTypeWithWrapper
    //[ Fact ]
    //public void ReplaceTypeWithWrapper_WithCorrectParameters_ReplacesTheType()
    //{
    //    // Arrange
    //    var builder = new ContainerBuilder();
    //    builder.RegisterGeneric( typeof( TypedServiceImplementation<> ) ).As( typeof( ITypedService<> ) );

    //    // Act
    //    builder.RegisterType<IService, ServiceImplementation>();
    //    builder.ReplaceTypeWithWrapper<IService, ITypedService<object>>();

    //    // Assert
    //    var container = builder.Build();
    //    container.Should().NotBeNull();

    //    var service = container.Resolve<IService>();
    //    service.Should().NotBeNull();
    //    service.Should().BeAssignableTo<ITypedService<IService>>();
    //    service.Should().BeOfType<TypedServiceImplementation<ServiceImplementation>>();
    //}

    //[ Fact ]
    //public void ReplaceTypeWithWrapper_WithCorrectParameters2_ReplacesTheType()
    //{
    //    // Arrange
    //    var builder = new ContainerBuilder();
    //    builder.RegisterGeneric( typeof( TypedServiceImplementation<> ) ).As( typeof( ITypedService<> ) );

    //    // Act
    //    builder.Register<IService>( c => c.Resolve<ServiceImplementation>() );
    //    builder.ReplaceTypeWithWrapper<IService, ITypedService<object>>();

    //    // Assert
    //    var container = builder.Build();
    //    container.Should().NotBeNull();

    //    var service = container.Resolve<IService>();
    //    service.Should().NotBeNull();
    //    service.Should().BeOfType<TypedServiceImplementation<ServiceImplementation>>();
    //}
    //#endregion

    //#region ReplaceTypeWithGenericType
    //[ Fact ]
    //public void ReplaceTypeWithGenericType_WithCorrectParameters_ReplacesTheType()
    //{
    //    // Arrange
    //    var builder = new ContainerBuilder();
    //    builder.RegisterGeneric( typeof( TypedServiceImplementation<> ) ).As( typeof( ITypedService<> ) );
    //    builder.RegisterGeneric( typeof( TypedServiceImplementation2<> ) ).As( typeof( ITypedService2<> ) );

    //    // Act
    //    builder.Register<IService>( c => c.Resolve<ITypedService<ServiceImplementation>>() );
    //    builder.RegisterType<ServiceImplementation>();
    //    //builder.Register<IService>( c => c.Resolve<ServiceImplementation>() );
    //    builder.ReplaceTypeWithGenericType<IService, ITypedService2<object>>();

    //    // Assert
    //    var container = builder.Build();
    //    container.Should().NotBeNull();

    //    var service = container.Resolve<IService>();
    //    service.Should().NotBeNull();
    //    service.Should().BeOfType<TypedServiceImplementation2<IService>>();
    //}
    //#endregion
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