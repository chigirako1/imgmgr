using PictureManagerApp.src.Lib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PictureManagerApp.src.Model
{
    public class GroupItem : FileItem
    {
        private List<FileItem> Files = new();
        private Image image;

        public GroupItem(string path, string zipPath = "") : base(path, zipPath)
        {

        }

        public override Image GetImage()
        {
            var img = ImageModule.GetGroupThumbnailImage(800, 800, $"{Files.Count()}");
            ImageSize.Width = img.Width;
            ImageSize.Height = img.Height;
            return img;
        }

        public void AddFileItem(FileItem fi)
        {
            Files.Add(fi);
        }

        public int MemberCount()
        {
            return Files.Count;
        }

        public List<Image> GetGroupImages()
        {
            var Images = new List<Image>();
            foreach (var file in Files)
            {
                Images.Add(file.GetImage());
            }
            return Images;
        }

        public Image GetGroupImage(int width, int height)
        {
            if (this.image == null)
            {
                var imgs = GetGroupImages();//TODO: 数が多いときは減らす？
                this.image = ImageModule.CreateCompositedImage(imgs, width, height);
            }
            return this.image;
        }

        override internal void toggleMark()
        {
            foreach (var file in Files)
            {
                file.toggleMark();
            }
        }
    }
}
