using Alpheus.IO;

namespace Fssec;

public abstract class AuditDirectoryInfo : AuditFileSystemInfo, IDirectoryInfo
{
    #region Constructors
    public AuditDirectoryInfo(AuditEnvironment env, string dir_path) : base(env)
    {
        this.FullName = dir_path;
        this.Name = this.GetPathComponents().Last();
    }
    #endregion

    #region Abstract properties
    public abstract IDirectoryInfo Root { get; }
    public abstract IDirectoryInfo Parent { get; }
    #endregion

    #region Abstract methods
    public abstract IDirectoryInfo[] GetDirectories();
    public abstract IDirectoryInfo[] GetDirectories(string search_path);
    public abstract IDirectoryInfo[] GetDirectories(string search_pattern, SearchOption search_option);
    public abstract IFileInfo[] GetFiles();
    public abstract IFileInfo[] GetFiles(string searchPattern);
    public abstract AuditFileInfo? GetFile(string file_path);
    public abstract Dictionary<AuditFileInfo, string> ReadFilesAsText(string searchPattern);
    public abstract Task<Dictionary<AuditFileInfo, string>> ReadFilesAsTextAsync(string searchPattern);
    public abstract Dictionary<AuditFileInfo, string> ReadFilesAsText(IEnumerable<AuditFileInfo> files);
    public abstract Task<Dictionary<AuditFileInfo, string>> ReadFilesAsTextAsync(IEnumerable<AuditFileInfo> files);
    //public abstract Dictionary<AuditFileInfo, byte[]> ReadFilesAsBinary(string searchPattern);
    //public abstract Dictionary<AuditFileInfo, byte[]> ReadFilesAsBinary(IEnumerable<AuditFileInfo> files);
    public abstract IFileInfo[] GetFiles(string searchPattern, SearchOption searchOption);
    public abstract LocalAuditDirectoryInfo GetAsLocalDirectory();
    public abstract Task<LocalAuditDirectoryInfo> GetAsLocalDirectoryAsync();
    #endregion

    #region Methods
    public IDirectoryInfo Create(string dir_path)
    {
        return this.AuditEnvironment.ConstructDirectory(dir_path);
    }
    #endregion
}

