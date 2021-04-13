namespace EnumFactory.Test.ImplementationMocks
{
    public enum CorrectBaseClassSetup
    {
        Correct1B,
        Correct2B
    }

    public abstract class CorrectBaseClassSetupService
    {
    }

    public class Correct1BService : CorrectBaseClassSetupService
    {
    }

    public class Correct2BService : CorrectBaseClassSetupService
    {
    }
}
