namespace Fssec;

public struct EnvironmentEventArgs
{
    #region Properties
    public EventMessageType MessageType { get; }
    public Thread CurrentThread { get; }
    public string Message { get; }
    public DateTime DateTime { get; } = DateTime.UtcNow;
    public CallerInformation? Caller { get; }
    public Exception? Exception { get; }
    public OperationProgress? Progress { get; }
    public string? EnvironmentLocation { get; internal set;}
    #endregion

    public EnvironmentEventArgs(EventMessageType message_type, string message)
    {
        this.CurrentThread = Thread.CurrentThread;
        this.MessageType = message_type;
        this.Message = message;
        this.Caller = null;
        this.Exception = null;
        this.Progress = null;
        this.EnvironmentLocation = null;
    }

    public EnvironmentEventArgs(EventMessageType message_type, string message_format, object[] m)
    {
        this.CurrentThread = Thread.CurrentThread;
        this.MessageType = message_type;
        this.Message = string.Format(message_format, m);
        this.Caller = null;
        this.Exception = null;
        this.Progress = null;
        this.EnvironmentLocation = null;
    }

    public EnvironmentEventArgs(CallerInformation caller, EventMessageType message_type, string message_format, object[] m)
    {
        this.CurrentThread = Thread.CurrentThread;
        this.Caller = caller;
        this.MessageType = message_type;
        this.Message = string.Format(message_format, m);
        this.Exception = null;
        this.Progress = null;
        this.EnvironmentLocation = null;
    }

    public EnvironmentEventArgs(CallerInformation caller, Exception e)
    {
        this.CurrentThread = Thread.CurrentThread;
        this.Caller = caller;
        this.MessageType = EventMessageType.ERROR;
        this.Message = string.Format("Exception occurred.");
        this.Exception = e;
        this.Progress = null;
        this.EnvironmentLocation = null;
    }

    public EnvironmentEventArgs(Exception e)
    {
        this.CurrentThread = Thread.CurrentThread;
        this.MessageType = EventMessageType.ERROR;
        this.Message = string.Format("Exception occurred.");
        this.Exception = e;
        this.Caller = null;
        this.Progress = null;
        this.EnvironmentLocation = null;
    }

    public EnvironmentEventArgs(OperationProgress p)
    {
        this.CurrentThread = Thread.CurrentThread;
        this.MessageType = EventMessageType.PROGRESS;
        this.Progress = p;
        this.Message = string.Format("{0} {1} of {2}", p.Operation, p.Complete, p.Total);
        if (p.Time.HasValue)
        {
            this.Message += string.Format(" in {0} ms.", p.Time.Value.Milliseconds);
        }
        this.Caller = null;
        this.Exception = null;
        this.Progress = null;
        this.EnvironmentLocation = null;
    }
}

