namespace Fssec;

public class ApiNotInitializedException : Exception
{
    public ApiNotInitializedException(Runtime api) : base($"The {api.GetType().Name} Api is not initialized.") {}
}

public class EnvironmentExecException : IOException
{
    public EnvironmentExecException(string msg, Exception? inner = null) : base(msg, inner) { }
}

public class FileIOException : IOException
{
    public FileIOException(string msg, Exception? inner = null) : base(msg, inner) { }
}
