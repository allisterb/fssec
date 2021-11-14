global using System;
global using System.Collections.Generic;
global using System.Diagnostics;
global using System.IO;
global using System.Runtime.CompilerServices;
global using System.Runtime.InteropServices;
global using System.Linq;
global using System.Text;
global using System.Text.RegularExpressions;
global using System.Threading;
global using System.Threading.Tasks;

global using static Fssec.CollectionUtils;
#region Enums
public enum EventMessageType
{
    SUCCESS = 0,
    ERROR = 1,
    INFO = 2,
    WARNING = 3,
    STATUS = 4,
    PROGRESS = 5,
    DEBUG = 6,
}
#endregion

#region Structs
public struct CallerInformation
{
    public string Name;
    public string File;
    public int LineNumber;

    public CallerInformation(string name, string file, int line_number)
    {
        this.Name = name;
        this.File = file;
        this.LineNumber = line_number;
    }
}

public struct OperationProgress
{
    public string Operation;
    public int Total;
    public int Complete;
    public TimeSpan? Time;

    public OperationProgress(string op, int total, int complete, TimeSpan? time)
    {
        this.Operation = op;
        this.Total = total;
        this.Complete = complete;
        this.Time = time;
    }
}
#endregion
