using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;
using System.IO.Compression;
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using System.Security.Cryptography;

namespace PictureManagerApp.src.Lib
{
    static class MyFiles
    {
        public static void moveToTrashDir(string path, string rootpath, string appendStr = "-0trash")
        {
            move(path, rootpath, appendStr);
        }

        public static void move(string path, string rootpath, string appendStr)
        {
            Log.trc("------");
            Log.log($"path={path}");

            FileInfo fi = new(path);

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

        public static byte[] GetThumbnailByteArray(string zippath, int width, int height)
        {
            //var img = new Bitmap(@"D:\pic\my-pic\com.example.imageviewer\test.jpg");

            var entries = GetZipEntryList(zippath);
            var img = MyFiles.GetImageFromZipFile(zippath, entries[0]);
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
    }
}
