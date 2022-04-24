using System.Diagnostics.CodeAnalysis;

namespace PbTools;

public class PbFileInfoEqualityComparer : IEqualityComparer<PbFileInfo>
{
    public bool Equals(PbFileInfo? x, PbFileInfo? y)
    {
        if (x == null || y == null)
        {
            return false;
        }

        return x.Extension == y.Extension && x.Length == y.Length;
    }

    public int GetHashCode([DisallowNull] PbFileInfo obj)
    {
        throw new NotImplementedException();
    }
}
