namespace EnumFactory.Test.ImplementationMocks
{
    public enum CorrectInterfaceSetup
    {
        Correct1,
        Correct2
    }

    public interface ICorrectInterfaceSetupService
    {
    }

    public class Correct1Service : ICorrectInterfaceSetupService
    {    
    }

    public class Correct2Service : ICorrectInterfaceSetupService
    {
    }
}
