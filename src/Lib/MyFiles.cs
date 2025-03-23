using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;
using System.IO.Compression;
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace PictureManagerApp.src.Lib
{
    static class MyFiles
    {
        public static string FormatFileSize(long bytes)
        {
            var unit = 1024;
            if (bytes < unit) { return $"{bytes} B"; }

            var exp = (int)(Math.Log(bytes) / Math.Log(unit));
            return $"{bytes / Math.Pow(unit, exp):F2} {("KMGTPE")[exp - 1]}B";
        }

        public static void moveToTrashDir(string path, string rootpath, string appendStr = "-0trash")
        {
            move(path, rootpath, appendStr);
        }

        public static void move(string path, string rootpath, string appendStr)
        {
            Log.trc("------");
            Log.log($"path={path}");

            System.IO.FileInfo fi = new(path);

            string p = Path.GetRelativePath(rootpath, path);
            Log.log($"p={p}");

            string moveToPath = Path.Combine(rootpath + appendStr, p);
            Log.log($"moveToPath={moveToPath}");

            string dirname = Path.GetDirectoryName(moveToPath);
            if (!File.Exists(dirname))
            {
                Directory.CreateDirectory(dirname);
            }

            try
            {
                fi.MoveTo(moveToPath);
            }
            catch (IOException e)
            {
                Log.err($"{e}");
            }
            Log.trc("------");
        }

        public static Image GetImageFromZipFile(string zippath, string filepath)
        {
            using (var archive = ZipFile.OpenRead(zippath))
            {
                var ent = archive.GetEntry(filepath);
                var img = Image.FromStream(ent.Open());

                return img;
            }
        }

        public static byte[] GetThumbnailByteArray(string zippath, int width, int height, int picidx=0)
        {
            var entries = GetZipEntryList(zippath);
            if (entries.Count < 2)
            {
                picidx = 0;
            }
            var img = MyFiles.GetImageFromZipFile(zippath, entries[picidx]);
            var thumbimg = ImageModule.GetThumbnailImage(img, width, height);
            var ba = ImageModule.ConvImageToByteArray(thumbimg);
            return ba;
        }

        public static List<string> GetZipEntryList(string zippath)
        {
            var encode = "utf-8";
            //encode = "sjis";
            //encode = "shift_jis";
            //using (var archive = ZipFile.OpenRead(path))
            using (ZipArchive archive = ZipFile.Open(zippath,
                ZipArchiveMode.Read,
                System.Text.Encoding.GetEncoding(encode)))
            {
                foreach (var e in archive.Entries)
                {
                    //filelist.Add(e.FullName);
                    //Console.WriteLine("名前       : {0}", e.Name);
                    Console.WriteLine("フルパス   : {0}", e.FullName);
                    //Console.WriteLine("サイズ     : {0}", e.Length);
                    //Console.WriteLine("圧縮サイズ : {0}", e.CompressedLength);
                    //Console.WriteLine("更新日時   : {0}", e.LastWriteTime);
                }

                var files = archive.Entries.//OrderBy(e => e.FullName).
                    Where(e =>
                        e.FullName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                        e.FullName.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                        e.FullName.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                        e.FullName.EndsWith(".gif", StringComparison.OrdinalIgnoreCase)
                );

                List<string> filelist = [];
                files.ToList().ForEach(f => filelist.Add(f.FullName));
                filelist.Sort(new NaturalStringComparer());

                return filelist;
            }
        }

        //static readonly HashAlgorithm hashProvider = new SHA1CryptoServiceProvider();古いらしい
        //static readonly HashAlgorithm hashProvider = HashAlgorithm.Create();古いらしい
        static readonly HashAlgorithm hashProvider = SHA256.Create();

        /// <summary>
        /// Returns the hash string for the file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ComputeFileHash(string filePath)
        {
            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var bs = hashProvider.ComputeHash(fs);
            return BitConverter.ToString(bs).ToLower().Replace("-", "");
        }

        public static void OpenGuiShell(string dirpath)
        {
            Log.trc($"'{dirpath}'");
            dirpath = dirpath.Replace("/", "\\");
            System.Diagnostics.Process.Start(
                "EXPLORER.EXE",
                dirpath);
        }

        public static void OpenGuiShellFile(string filepath)
        {
            Log.trc($"'{filepath}'");
            filepath = filepath.Replace("/", "\\");
            System.Diagnostics.Process.Start(
                "EXPLORER.EXE",
                @"/select," + filepath);
        }
    }

}
