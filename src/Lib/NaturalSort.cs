using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

namespace PictureManagerApp.src.Lib
{
    [SuppressUnmanagedCodeSecurity]
    internal static class SafeNativeMethods
    {
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        public static extern int StrCmpLogicalW(string psz1, string psz2);
    }

    public sealed class NaturalStringComparer : IComparer<string>
    {
        public int Compare(string a, string b)
        {
            return SafeNativeMethods.StrCmpLogicalW(a, b);
        }
    }

    public sealed class NaturalFileInfoNameComparer : IComparer<System.IO.FileInfo>
    {
        public int Compare(System.IO.FileInfo a, System.IO.FileInfo b)
        {
            return SafeNativeMethods.StrCmpLogicalW(a.Name, b.Name);
        }
    }
}
