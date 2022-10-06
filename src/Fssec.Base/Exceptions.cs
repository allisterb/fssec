namespace Fssec;

using System;
public class RuntimeNotInitializedException : Exception
{
    public RuntimeNotInitializedException(Runtime api) : base($"The {api.GetType().Name} Api is not initialized.") {}
}