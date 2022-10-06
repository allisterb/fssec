using System.Reflection;

using Microsoft.Extensions.Configuration;

namespace Fssec
{
    public abstract class Runtime
    {
        #region Constructors
        static Runtime()
        {
            if (IsKubernetesPod || !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("OPENSHIFT_BUILD_NAMESPACE")))
            {
                Configuration = new ConfigurationBuilder()
                    .AddEnvironmentVariables()
                    .Build();
            }
            else if ((EntryAssemblyName == "Fssec.CLI" || EntryAssemblyName == "Fssec.Web") && Environment.GetEnvironmentVariable("USERNAME") == "Allister")
            {
                Configuration = new ConfigurationBuilder()
                    .AddUserSecrets("bdb696c2-242b-482c-8e96-cd61bb87b67e")
                    .Build();
            }
            else
            {
                Configuration = new ConfigurationBuilder()
                .AddJsonFile("config.json", optional: true)
                .Build();
            }

            HttpClient.DefaultRequestHeaders.UserAgent.ParseAdd("fssec/0.1");
            Logger = new ConsoleLogger();
        }
        public Runtime(CancellationToken ct)
        {
            if (Logger == null)
            {
                throw new InvalidOperationException("A logger is not assigned.");
            }
            Ct = ct;
            Type = this.GetType();
        }
        public Runtime(): this(Cts.Token) {}

        #endregion

        #region Properties
        public static Assembly EntryAssembly { get; } = Assembly.GetEntryAssembly()!;

        public static string EntryAssemblyName { get; } = EntryAssembly.GetName().Name!;

        
        public static DirectoryInfo EntryAssemblyDirectory = new DirectoryInfo(EntryAssembly.Location);

        
        public static Version EntryAssemblyVersion = EntryAssembly.GetName().Version!;

        public static string RuntimeAssemblyLocation { get; } = Path.GetDirectoryName(Assembly.GetAssembly(typeof(Runtime))!.Location)!;

        public static Version RuntimeAssemblyVersion { get; } = Assembly.GetAssembly(typeof(Runtime))!.GetName().Version!;

        public static DirectoryInfo CurrentDirectory { get; } = new DirectoryInfo(Directory.GetCurrentDirectory());

        public static IConfigurationRoot Configuration { get; protected set; }

        public static string Config(string i) => Configuration[i];

        public static bool DebugEnabled { get; set; }

        public static bool InteractiveConsole { get; set; } = false;

        public static bool IsAzureFunction { get; set; }

        public static bool IsKubernetesPod { get; } = (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("KUBERNETES_PORT")));

        public static string PathSeparator { get; } = Environment.OSVersion.Platform == PlatformID.Win32NT ? "\\" : "/";

        public static string LineTerminator { get; } = Environment.OSVersion.Platform == PlatformID.Win32NT ? "\r\n" : "\n";

        public static Logger Logger { get; protected set; }

        public static CancellationTokenSource Cts { get; } = new CancellationTokenSource();

        public static CancellationToken Ct { get; protected set; } = Cts.Token;

        public static HttpClient HttpClient { get; } = new HttpClient();

        public static string YY = DateTime.Now.Year.ToString().Substring(2, 2);

        public bool Initialized { get; protected set; }

        public Type Type { get; }

        #endregion

        #region Methods
        public static void SetLogger(Logger logger)
        {
            Logger = logger;
        }

        public static void SetLoggerIfNone(Logger logger)
        {
            if (Logger == null)
            {
                Logger = logger;
            }
        }

        public static void SetDefaultLoggerIfNone()
        {
            if (Logger == null)
            {
                Logger = new ConsoleLogger();
            }
        }

        [DebuggerStepThrough]
        public static void Info(string messageTemplate, params object[] args) => Logger.Info(messageTemplate, args);

        [DebuggerStepThrough]
        public static void Debug(string messageTemplate, params object[] args) => Logger.Debug(messageTemplate, args);

        [DebuggerStepThrough]
        public static void Error(string messageTemplate, params object[] args) => Logger.Error(messageTemplate, args);

        [DebuggerStepThrough]
        public static void Error(Exception ex, string messageTemplate, params object[] args) => Logger.Error(ex, messageTemplate, args);

        [DebuggerStepThrough]
        public static void Warn(string messageTemplate, params object[] args) => Logger.Warn(messageTemplate, args);

        [DebuggerStepThrough]
        public static void Fatal(string messageTemplate, params object[] args) => Logger.Fatal(messageTemplate, args);

        [DebuggerStepThrough]
        public static Logger.Op Begin(string messageTemplate, params object[] args) => Logger.Begin(messageTemplate, args);

        [DebuggerStepThrough]
        public static void WarnIfFileExists(string filename)
        {
            if (File.Exists(filename)) Warn("File {0} exists, overwriting...", filename);
        }

        [DebuggerStepThrough]
        public void FailIfNotInitialized()
        {
            if (!Initialized)
            {
                throw new RuntimeNotInitializedException(this);
            }
        }

        [DebuggerStepThrough]
        public T FailIfNotInitialized<T>(Func<T> r) => Initialized ? r() : throw new RuntimeNotInitializedException(this);

        [DebuggerStepThrough]
        public static string FailIfFileNotFound(string filePath)
        {
            if (filePath.StartsWith("http://") || filePath.StartsWith("https://"))
            {
                return filePath;
            }
            else if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(filePath);
            }
            else return filePath;
        }

        public static void SetPropsFromDict<T>(T instance, Dictionary<string, object> p)
        {
            foreach (PropertyInfo prop in typeof(T).GetProperties())
            {
                if (p.ContainsKey(prop.Name) && prop.PropertyType == p[prop.Name].GetType())
                {
                    prop.SetValue(instance, p[prop.Name]);
                }
            }
        }

        public void ThrowIfNotInitialized()
        {
            if (!this.Initialized) throw new RuntimeNotInitializedException(this);
        }
        #endregion
    }
}
