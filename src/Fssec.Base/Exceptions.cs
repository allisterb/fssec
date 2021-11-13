using System;
using System.Collections.Generic;
using System.Text;

namespace Fssec;

public class ApiNotInitializedException : Exception
{
    public ApiNotInitializedException(Runtime api) : base($"The {api.GetType().Name} Api is not initialized.") {}
}
