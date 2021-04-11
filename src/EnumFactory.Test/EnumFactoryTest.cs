using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;
using EnumFactory;
using EnumFactory.Test.ImplementationMocks;

namespace EnumFactory.Test
{
    public class EnumFactoryTest
    {
        private static IServiceProvider GetServiceProvider(Action<ServiceCollection> registerServices)
        {
            var services = new ServiceCollection();
            registerServices(services);
            return services.BuildServiceProvider();
        }


        [Fact]
        public void CorrectInterfaceScopedSetupShouldSucceed()
        {
            //Arrange
            var provider = GetServiceProvider(x => x.AddEnumFactory<CorrectInterfaceSetup, ICorrectInterfaceSetupService>(Lifecycle.Scoped));

            //Act 
            var factory = provider.GetService<IEnumFactory<CorrectInterfaceSetup, ICorrectInterfaceSetupService>>();
            var c1 = factory.GetService(CorrectInterfaceSetup.Correct1);
            var c2 = factory.GetService(CorrectInterfaceSetup.Correct2);

            //Assert
            Assert.IsType<Correct1Service>(c1);
            Assert.IsType<Correct2Service>(c2);
        }

        [Fact]
        public void CorrectInterfaceTransientSetupShouldSucceed()
        {
            //Arrange
            var provider = GetServiceProvider(x => x.AddEnumFactory<CorrectInterfaceSetup, ICorrectInterfaceSetupService>(Lifecycle.Transient));

            //Act 
            var factory = provider.GetService<IEnumFactory<CorrectInterfaceSetup, ICorrectInterfaceSetupService>>();
            var c1 = factory.GetService(CorrectInterfaceSetup.Correct1);
            var c2 = factory.GetService(CorrectInterfaceSetup.Correct2);

            //Assert
            Assert.IsType<Correct1Service>(c1);
            Assert.IsType<Correct2Service>(c2);
        }

        [Fact]
        public void CorrectInterfaceSingletonSetupShouldSucceed()
        {
            //Arrange
            var provider = GetServiceProvider(x => x.AddEnumFactory<CorrectInterfaceSetup, ICorrectInterfaceSetupService>(Lifecycle.Singleton));

            //Act 
            var factory = provider.GetService<IEnumFactory<CorrectInterfaceSetup, ICorrectInterfaceSetupService>>();
            var c1 = factory.GetService(CorrectInterfaceSetup.Correct1);
            var c2 = factory.GetService(CorrectInterfaceSetup.Correct2);

            //Assert
            Assert.IsType<Correct1Service>(c1);
            Assert.IsType<Correct2Service>(c2);
        }

        [Fact]
        public void CorrectInterfaceSetupSingletonShouldSucceed()
        {
            //Arrange
            var provider = GetServiceProvider(x => x.AddEnumFactory<CorrectInterfaceSetup, ICorrectInterfaceSetupService>());

            //Act 
            var factory = provider.GetService<IEnumFactory<CorrectInterfaceSetup, ICorrectInterfaceSetupService>>();
            var c1 = factory.GetService(CorrectInterfaceSetup.Correct1);
            var c2 = factory.GetService(CorrectInterfaceSetup.Correct2);

            //Assert
            Assert.IsType<Correct1Service>(c1);
            Assert.IsType<Correct2Service>(c2);
        }

        [Fact]
        public void CorrectBaseClassSetupShouldSucceed()
        {
            //Arrange
            var provider = GetServiceProvider(x => x.AddEnumFactory<CorrectBaseClassSetup, CorrectBaseClassSetupService>());

            //Act 
            var factory = provider.GetService<IEnumFactory<CorrectBaseClassSetup, CorrectBaseClassSetupService>>();
            var b1 = factory.GetService(CorrectBaseClassSetup.Correct1B);
            var bc2 = factory.GetService(CorrectBaseClassSetup.Correct2B);

            //Assert
            Assert.IsType<Correct1BService>(b1);
            Assert.IsType<Correct2BService>(bc2);
        }

        [Fact]
        public void MissingImplementationSetupShouldFail()
        {
            //Arrange, Act, Assert
            Assert.Throws<Exception>(() => GetServiceProvider(x => x.AddEnumFactory<MissingImplSetup, IMissingImplSetupService>()));
        }

        [Fact]
        public void ExtraImplementationSetupShouldFail()
        {
            //Arrange, Act, Assert
            Assert.Throws<Exception>(() => GetServiceProvider(x => x.AddEnumFactory<ExtraImplSetup, IExtraImplSetupService>()));
        }
    }
}
