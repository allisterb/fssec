using Alpheus.IO;

namespace Fssec;
public abstract class AuditFileInfo : AuditFileSystemInfo, IFileInfo
{
    #region Constructors
    public AuditFileInfo(AuditEnvironment env, string file_path) : base(env)
    {
        this.FullName = file_path;
        this.Name = this.GetPathComponents().Last();
    }
    #endregion

    #region Abstract properties
    public abstract string DirectoryName { get; }
    public abstract IDirectoryInfo Directory { get; }
    public abstract bool IsReadOnly { get; }
    public abstract long Length { get; }
    public abstract DateTime LastWriteTimeUtc { get; }
    #endregion

    #region Abstract methods
    public abstract string ReadAsText();
    public abstract byte[] ReadAsBinary();
    public abstract bool PathExists(string file_path);
    public abstract LocalAuditFileInfo GetAsLocalFile();
    public abstract Task<LocalAuditFileInfo> GetAsLocalFileAsync();
    #endregion

    #region Methods
    public IFileInfo Create(string file_path)
    {
        return this.AuditEnvironment.ConstructFile(this.CombinePaths(this.Directory.FullName, file_path));
    }
    #endregion
}
