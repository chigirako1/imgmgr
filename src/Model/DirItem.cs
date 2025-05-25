using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PictureManagerApp.src.Lib;

namespace PictureManagerApp.src.Model
{
    public struct FileInfo
    {
        public string Name;
        public int Rating;
        public int PageTotal;
    }

    public class DirItem
    {
        public enum IMPORTANCE_TYPE
        {
            IMPORTANCE_NORMAL,
            IMPORTANCE_LOW,
            IMPORTANCE_HIGH,

            IMPORTANCE_MAX,
        }

        private string Path;
        private int TotalPageNo;
        private int PageNo;
        private IMPORTANCE_TYPE Importance;
        //private Image Thumbnail;

        public DirItem(string path)
        {
            Path = path;
            TotalPageNo = 0;
            PageNo = 0;
            Importance = IMPORTANCE_TYPE.IMPORTANCE_NORMAL;

            Log.trc($"{TotalPageNo}{PageNo}{Importance}");
        }

        public void Update()
        {
            TotalPageNo = 0;
            PageNo = 0;
            //Thumbnail;
        }
        public static FileInfo GetInfoFromFilename(string filename)
        {
            FileInfo ai = new();

            var pattern = @"\w+-\w+\d+-(.*)\[(\d+)\].\w+";
            var mc = System.Text.RegularExpressions.Regex.Matches(filename, pattern);
            foreach (System.Text.RegularExpressions.Match m in mc)
            {
                var info = m.Groups[1].Value;
                ai.PageTotal = Int32.Parse(m.Groups[2].Value);

                pattern = @"\[(\d+)\]\s+\[\w+\]\s+(.*)";
                mc = System.Text.RegularExpressions.Regex.Matches(info, pattern);
                if (mc.Count > 0)
                {
                    foreach (System.Text.RegularExpressions.Match m2 in mc)
                    {
                        ai.Rating = Int32.Parse(m2.Groups[1].Value);
                        ai.Name = m2.Groups[2].Value;
                        break;
                    }
                    break;
                }

                pattern = @"(.*)";
                mc = System.Text.RegularExpressions.Regex.Matches(info, pattern);
                if (mc.Count > 0)
                {
                    foreach (System.Text.RegularExpressions.Match m3 in mc)
                    {
                        ai.Rating = 0;
                        ai.Name = m3.Groups[1].Value;
                        break;
                    }
                    break;
                }
            }
            return ai;
        }
    }
}
