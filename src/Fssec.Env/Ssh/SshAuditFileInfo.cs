﻿namespace Fssec;

using Alpheus.IO;

public class SshAuditFileInfo : AuditFileInfo
{
    #region Overriden properties
    public override string FullName { get; protected set; }

    public override string Name { get; protected set; }

    public override bool IsReadOnly
    {
        get
        {
            throw new NotImplementedException();
        }
    }
    public override bool Exists
    {
        get
        {
            return this.AuditEnvironment.FileExists(this.FullName);
        }
    }

    public override long Length
    {
        get
        {
            if (!_Length.HasValue)
            {
                string c = this.ReadAsText();
                if (!string.IsNullOrEmpty(c))
                {
                    this._Length = c.Length;
                }
                else
                {
                    EnvironmentCommandError(this.AuditEnvironment.Here(), "Could not read {0} as text.", this.FullName);
                }
            }
            return this._Length.HasValue ? this._Length.Value : -1;
        }
    }

    public override DateTime LastWriteTimeUtc
    {
        get
        {
            DateTime result;
            string cmd = "stat";
            string args = string.Format("-c '%y' {0}", this.FullName);
            string? d = EnvironmentExecute(cmd, args);
            if (!string.IsNullOrEmpty(d))
            {
                if (DateTime.TryParse(d, out result))
                {
                    return result;
                }
                else
                {
                    EnvironmentCommandError(this.AuditEnvironment.Here(), "Could not parse result of {0} as DateTime: {1}.", cmd + " " + args, d);
                    return new DateTime(1000, 1, 1);
                }
            }
            else
            {
                EnvironmentCommandError(this.AuditEnvironment.Here(), "Command {0} returned null.", cmd + " " + args);
                return new DateTime(1000, 1, 1);
            }
        }
    }

    public override IDirectoryInfo Directory
    {
        get
        {
            if (this._Directory is null)
            {
                string? o = this.EnvironmentExecute("dirname", this.FullName);
                if (!string.IsNullOrEmpty(o))
                {
                    this._Directory = new SshAuditDirectoryInfo(this.SshAuditEnvironment, o);
                    return this._Directory;
                }
                else
                {
                    EnvironmentCommandError(this.AuditEnvironment.Here(), "Could not get parent directory for {0}.", this.FullName);
                    throw new InvalidOperationException($"Could not get parent directory for {FullName}");
                }
            }
            else return this._Directory;
        }
    }

    public override string DirectoryName
    {
        get
        {
            if (this.Directory != null)
            {
                return this.Directory.Name;
            }
            else
            {
                EnvironmentCommandError(this.AuditEnvironment.Here(), "Could not get directory name for {0}.", this.FullName);
                return string.Empty;
            }
        }
    }
    #endregion

    #region Overriden methods
    public override bool PathExists(string file_path)
    {
        string? result = this.EnvironmentExecute("test", "-f " + this.CombinePaths(this.Directory.FullName, file_path) + " && echo \"Yes\" || echo \"No\"");
        if (!string.IsNullOrEmpty(result) && (result == "Yes" || result == "No"))
        {
            return result == "Yes" ? true : false;
        }
        else
        {
            EnvironmentCommandError(this.AuditEnvironment.Here(), "Could not test for existence of regular file {0}.", file_path);
            return false;
        }
    }

    public override string ReadAsText()
    {
        string? o = this.EnvironmentExecute("cat", this.FullName);

        if (!string.IsNullOrEmpty(o))
        {
            if (o == string.Format("cat: {0}: No such file or directory", this.FullName))
            {
                EnvironmentCommandError(this.AuditEnvironment.Here(), "Access denied reading {0}.", this.FullName);
                return string.Empty;
            }
            else
            {
                this.AuditEnvironment.Debug(this.AuditEnvironment.Here(), "Read {0} characters from file {1}.", o.Length, this.FullName);
                return o;
            }
        }
        else
        {
            EnvironmentCommandError(this.AuditEnvironment.Here(), "Could not read as text {0}.", this.FullName);
            return string.Empty;
        }
    }

    public override byte[] ReadAsBinary()
    {
        throw new NotImplementedException();
    }

    public override LocalAuditFileInfo GetAsLocalFile()
    {
        CallerInformation caller = this.AuditEnvironment.Here();
        List<string> components = this.Directory.FullName.Split(PathSeparator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList() ;
        components.RemoveAll(c => c.Contains(PathSeparator) || c.Contains(":")); //Remove any drive or device parts
        DirectoryInfo parent = this.AuditEnvironment.WorkDirectory ?? throw new InvalidOperationException("SSH audit environment work directory is null.");
        if (components.Count > 1)
        {
            foreach (string c in components.Take(components.Count - 1))
            {
                DirectoryInfo d = new DirectoryInfo(Path.Combine(parent.FullName, c));
                if (!d.Exists)
                {
                    d.Create();
                    this.AuditEnvironment.Debug(caller, "Created sub-directory {0} in work directory.", c);
                    parent = d;
                }
            }
        }
        FileInfo lf = this.SshAuditEnvironment.GetFileAsLocal(this.FullName, this.CombinePaths(parent.FullName, this.Name));
        return new LocalAuditFileInfo(this.AuditEnvironment.HostEnvironment ?? throw new InvalidOperationException("SSH environment host environment is null."), lf);
    }

    public override async Task<LocalAuditFileInfo> GetAsLocalFileAsync()
    {
        return await Task.Run(() => this.GetAsLocalFile());
    }
    #endregion

    #region Constructors
    public SshAuditFileInfo(SshAuditEnvironment env, string file_path) : base(env, file_path)
    {
        this.SshAuditEnvironment = env;
        this.FullName = file_path;
        this.Name = this.GetPathComponents().Last();
    }
    #endregion    

    #region Properties
    protected SshAuditEnvironment SshAuditEnvironment { get; set; }
    #endregion

    #region Fields
    private long? _Length;
    private IDirectoryInfo? _Directory;
    #endregion
}

