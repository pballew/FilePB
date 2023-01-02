namespace PbTools;

public class PbFileUniqueId : IEquatable<PbFileUniqueId>
{
    public string? Extension { get; set; }
    public long Length { get; set; }

    public PbFileUniqueId(string filePath, long length)
    {
        Extension = Path.GetExtension(filePath);
        Length = length;
    }

    public bool Equals(PbFileUniqueId? other)
    {
        if (other == null) return false;
        if (Extension != other.Extension) return false;
        if (Length != other.Length) return false;

        return true;
    }
}

public class PbFileInfo
{
    public string? FilePath { get; set; }
    public string? FileName { get; set; }
    public PbFileUniqueId FileUniqueId { get; }

    public PbFileInfo(string filePath)
    {
        using (var stream = File.OpenRead(filePath))
        {
            FilePath = filePath;
            FileName = Path.GetFileName(filePath);
            FileUniqueId = new PbFileUniqueId(filePath, stream.Length);
        }
    }
}

public class PbDuplicate
{
    public PbFileUniqueId FileUniqueId { get; }
    public List<PbFileInfo> FileInfos { get; } = new List<PbFileInfo>();

    public PbDuplicate(PbFileUniqueId fileUniqueId)
    {
        FileUniqueId = fileUniqueId;
    }
}
