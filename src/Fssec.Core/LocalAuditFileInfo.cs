namespace Fssec;

using Alpheus.IO;

public class LocalAuditFileInfo : AuditFileInfo
{
    #region Overriden properties
    public override IDirectoryInfo Directory => new LocalDirectoryInfo(this.File.Directory);
    
    public override string DirectoryName => this.File.DirectoryName ?? throw new Exception($"Directory name for file {File.Name} is null.");
    
    public override bool Exists
    {
        get
        {
            return this.File.Exists;
        }
    }

    public override long Length
    {
        get
        {
            return this.File.Length;
        }
    }

    public override bool IsReadOnly
    {
        get
        {
            return this.File.IsReadOnly;
        }
    }

    public override string FullName { get; protected set; }
      

    public override string Name { get; protected set; }
    

    public override DateTime LastWriteTimeUtc
    {
        get
        {
            return this.File.LastAccessTimeUtc;
        }
    }

    public override bool PathExists(string file_path)
    {
        return System.IO.File.Exists(file_path);
    }
    #endregion

    #region Overriden methods
    public override string ReadAsText()
    {
        using (StreamReader s = new StreamReader(this.File.OpenRead()))
        {
            return s.ReadToEnd();
        }
    }

    public override byte[] ReadAsBinary()
    {
        using (FileStream s = this.File.Open(FileMode.Open, FileAccess.Read))
        {
            byte[] buffer = new byte[this.File.Length];
            s.Read(buffer, 0, buffer.Length);
            return buffer;
        }
    }

    public override LocalAuditFileInfo GetAsLocalFile()
    {
        return this;
    }

    public override Task<LocalAuditFileInfo> GetAsLocalFileAsync()
    {
        return Task.FromResult<LocalAuditFileInfo>(this);
    }
    #endregion

    #region Constructors
    public LocalAuditFileInfo(LocalEnvironment env, string file_path) : base(env, file_path)
    {
        this.LocalAuditEnvironment = env;
        this.File = new FileInfo(file_path);
        this.Name = this.File.Name;
        this.FullName = this.File.FullName;
    }

    public LocalAuditFileInfo(LocalEnvironment env, FileInfo f) : base(env, f.FullName)
    {
        this.LocalAuditEnvironment = env;
        this.File = f;
        this.Name = this.File.Name;
        this.FullName = this.File.FullName;
    }
    #endregion

    #region Properties
    public FileInfo SysFile
    {
        get
        {
            return this.File;
        }
    }
    #endregion

    #region Fields
    private FileInfo File;
    private LocalEnvironment LocalAuditEnvironment { get; set; }
    #endregion
}

