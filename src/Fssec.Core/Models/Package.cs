namespace Fssec;
using Newtonsoft.Json;


[Serializable]
public class Package : IPackage
{
    [JsonProperty("pm")]
    public string PackageManager { get; set; }

    [JsonProperty("group")]
    public string? Group { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("version")]
    public string Version { get; set; }

    [JsonProperty("vendor")]
    public string? Vendor { get; set; }

    [JsonIgnore]
    public string? Architecture { get; set; }

    public Package(string package_manager, string application_name, string version, string? vendor = null, string? group = null, string? architecture = null)
    {
        this.PackageManager = package_manager;
        this.Name = application_name;
        this.Version = version;
        this.Vendor = vendor;
        this.Group = group;
        this.Architecture = architecture;
    }

    public string getPurl()
    {
        if (Group != null)
        {
            return "pkg:" + PackageManager + "/" + Group + "/" + Name + "@" + Version;
        }
        else
        {
            return "pkg:" + PackageManager + "/" + Name + "@" + Version;
        }
    }
}


