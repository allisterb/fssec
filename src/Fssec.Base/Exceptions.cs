namespace Fssec;

public class ApiNotInitializedException : Exception
{
    public ApiNotInitializedException(Runtime api) : base($"The {api.GetType().Name} Api is not initialized.") {}
}