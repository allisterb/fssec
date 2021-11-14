namespace Fssec;

public readonly struct ProcessInfo
{
    #region Constructors
    public ProcessInfo(string user, int pid, string start_time, string cmd_line)
    {
        this.User = user;
        this.Pid = pid;
        this.CommandLine = cmd_line;
        this.StartTime = start_time;
    }
    #endregion

    #region Properties
    public string User { get; }
    public int Pid { get; }
    public string CommandLine { get; }
    public string StartTime { get; }
    #endregion
}

