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
        public void CorrectDISupportSetupShouldSucceed()
        {
            //Arrange
            var provider = GetServiceProvider(x => 
            {
                x.AddEnumFactory<DISupportSetup, IDISupportSetupService>();
                x.AddScoped<Dependency>();
            });

            //Act 
            var factory = provider.GetService<IEnumFactory<DISupportSetup, IDISupportSetupService>>();
            var d1 = factory.GetService(DISupportSetup.DISupport1);
            var d2 = factory.GetService(DISupportSetup.DISupport2);

            //Assert
            Assert.IsType<DISupport1Service>(d1);
            Assert.IsType<DISupport2Service>(d2);
        }

        [Fact]
        public void CorrectVariationsSetupShouldSucceed()
        {
            //Arrange
            var provider = GetServiceProvider(x => x.AddEnumFactory<CorrectVariationsSetup, CorrectVariationsSetupService>());

            //Act 
            var factory = provider.GetService<IEnumFactory<CorrectVariationsSetup, CorrectVariationsSetupService>>();
            var v1 = factory.GetService(CorrectVariationsSetup.Correct1V);
            var v2 = factory.GetService(CorrectVariationsSetup.Correct2V);

            //Assert
            Assert.IsType<Correct1VSuffix1>(v1);
            Assert.IsType<Correct2VSuffix2>(v2);
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
