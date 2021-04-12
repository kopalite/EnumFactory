using System;

namespace EnumFactory.Test.ImplementationMocks
{
    public enum DISupportSetup
    {
        DISupport1,
        DISupport2
    }

    public interface IDISupportSetupService
    {
    }

    public class DISupport1Service : IDISupportSetupService
    {
        public DISupport1Service(Dependency dep)
        {
            if (dep == null)
            {
                throw new Exception("Variant classes should support DI");
            }
        }
    }

    public class DISupport2Service : IDISupportSetupService
    {
    }

    public class Dependency
    {
        
    }
}
