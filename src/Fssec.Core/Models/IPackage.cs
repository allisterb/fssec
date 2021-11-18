namespace Fssec;

public interface IPackage
{
    string PackageManager { get; set; }

    string Name { get; set; }

    string Version { get; set; }

    string? Group { get; set; }

    string? Vendor { get; set; }
}
