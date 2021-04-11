namespace EnumFactory.Test.ImplementationMocks
{
    public enum MissingImplSetup
    {
        Missing1,
        Missing2
    }

    public interface IMissingImplSetupService
    {
    }

    public class Missing1Service : IMissingImplSetupService
    {
    }

    public class Missing2WronglyNamedService : IMissingImplSetupService
    {
    }

}
