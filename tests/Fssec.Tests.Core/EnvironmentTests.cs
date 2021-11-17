using System;

using Microsoft.Extensions.Configuration;
using Xunit;

namespace Fssec.Tests;

using Fssec;
public class SshEnvironmentTests
{
    protected static IConfigurationRoot Config = 
            new ConfigurationBuilder()
            .AddUserSecrets("bdb696c2-242b-482c-8e96-cd61bb87b67e")
            .Build();

    protected EventHandler<EnvironmentEventArgs> nullHandler = new EventHandler<EnvironmentEventArgs>((s, e) => {});
    protected LocalEnvironment hostEnv = new LocalEnvironment();
    protected string sshHost = Config["SSH_TEST_HOST"];
    protected string sshUser = Config["SSH_TEST_USER"];
    protected string sshPass = Config["SSH_TEST_PASS"];
    protected int sshPort = int.Parse(Config["SSH_TEST_PORT"]);

    [Fact]
    public void CanConstructSshEnvironment()
    {
        var env = new SshAuditEnvironment(nullHandler, sshHost, sshPort, sshUser, hostEnv.ToSecureString(sshPass), null, hostEnv);
        Assert.True(env.Initialized);
    }
}
