using PictureManagerApp.src.Lib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PictureManagerApp.src.Model
{
    public class ZipEntryItem : FileItem
    {
        public ZipEntryItem(string path, string zipPath) : base(path, zipPath)
        {

        }

        private new Image GetImage()
        {
            var img = MyFiles.GetImageFromZipFile(mZipPath, FilePath);
            ImageSize.Width = img.Width;
            ImageSize.Height = img.Height;
            return img;
        }

        override public string GetZipEntryname()
        {
            return this.FilePath;
        }

        override public string GetFilename()
        {
            return Path.GetFileName(mZipPath);
        }

    }
}
