using System.IO;

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

            FileInfo fi = new FileInfo(path);

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
    }
}
