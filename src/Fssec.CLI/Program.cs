﻿namespace Fssec.CLI;

using CommandLine;
using CommandLine.Text;
using Spectre.Console;

#region Enums
public enum ExitResult
{
    SUCCESS = 0,
    UNHANDLED_EXCEPTION = 1,
    INVALID_OPTIONS = 2,
    NOT_FOUND = 4,
    SERVER_ERROR = 5,
    ERROR_IN_RESULTS = 6,
    UNKNOWN_ERROR = 7
}
#endregion

class Program : Runtime
{
    #region Constructor
    static Program()
    {
        AppDomain.CurrentDomain.UnhandledException += Program_UnhandledException;
        foreach (var t in OptionTypes)
        {
            OptionTypesMap.Add(t.Name, t);
        }

    }
    #endregion

    #region Entry point
    static void Main(string[] args)
    {
        PrintLogo();
        ParserResult<object> result = new Parser().ParseArguments<Options, ApiOptions, PingOptions, EndpointsOptions, SchemaOptions, VerticesOptions, BuiltinOptions>(args);
        result.WithParsed<ApiOptions>(o =>
        {

        });
    }
    #endregion

    #region Properties
    private static FigletFont Font { get; } = FigletFont.Load("chunky.flf");

    static Type[] OptionTypes = { typeof(Options), typeof(ApiOptions), typeof(PingOptions), typeof(EndpointsOptions),
            typeof(SchemaOptions), typeof(VerticesOptions)};
    static Dictionary<string, Type> OptionTypesMap { get; } = new Dictionary<string, Type>();
    #endregion

    #region Methods
    static void PrintLogo()
    {
        Con.Write(new FigletText(Font, "Fssec").LeftAligned().Color(Color.Blue));
        Con.Write(new Text($"v{AssemblyVersion.ToString(3)}").LeftAligned());
    }

    public static void Exit(ExitResult result)
    {

        if (Cts != null && !Cts.Token.CanBeCanceled)
        {
            Cts.Cancel();
            Cts.Dispose();
        }
        Environment.Exit((int)result);
    }

    static HelpText GetAutoBuiltHelpText(ParserResult<object> result)
    {
        return HelpText.AutoBuild(result, h =>
        {
            h.AddOptions(result);
            return h;
        },
        e =>
        {
            return e;
        });
    }
    #endregion

    #region Event Handlers
    private static void Program_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        Error((Exception)e.ExceptionObject, "Unhandled runtime error occurred. Fssec CLI will now shutdown.");
        Exit(ExitResult.UNHANDLED_EXCEPTION);
    }

    private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
    {
        Info("Ctrl-C pressed. Exiting.");
        Cts.Cancel();
        Exit(ExitResult.SUCCESS);
    }
    #endregion
}
