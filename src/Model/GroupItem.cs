using PictureManagerApp.src.Lib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PictureManagerApp.src.Model
{
    public class GroupItem : FileItem
    {
        private List<FileItem> files = new();

        public GroupItem(string path, string zipPath = "") : base(path, zipPath)
        {

        }

        public override Image GetImage()
        {
            var img = ImageModule.GetGroupThumbnailImage(640, 640);
            ImageSize.Width = img.Width;
            ImageSize.Height = img.Height;
            return img;
        }

        public void AddFileItem(FileItem fi)
        {
            files.Add(fi);
        }

        public int MemberCount()
        {
            return files.Count;
        }
    }
}
