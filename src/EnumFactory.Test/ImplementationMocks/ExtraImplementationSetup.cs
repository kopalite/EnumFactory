using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnumFactory.Test.ImplementationMocks
{

    public enum ExtraImplSetup
    {
        Extra1,
        Extra2
    }

    public interface IExtraImplSetupService
    {
    }

    public class Extra1Service : IExtraImplSetupService
    {
    }

    public class Extra2Service : IExtraImplSetupService
    {
    }

    public class Extra3Service : IExtraImplSetupService
    {
    }
}
