using Xunit;

namespace Fssec.Tests;

using Fssec;
public class FSTests
{
    [Fact]
    public void Test1()
    {
        var env = new LocalEnvironment();
        Assert.NotEmpty(env.OSName);
    }
}
