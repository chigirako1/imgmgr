using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace PictureManagerApp.src.Lib
{
    abstract class TsvRow
    {
    }

    class PicEvalRow : TsvRow
    {
            public enum COL_NAME
        {
            COL_FILE_NAME,
            COL_ENTRY_NAME,
            COL_METHOD,
            COL_TIMESTAMP,

            COL_MAX
        }

        public enum METHOD
        {
            METHOD_NONE,
            METHOD_DEL,
            METHOD_FAV,

            METHOD_MAX
        }

        public string FileName { get; private set; }
        public string EntryName { get; private set; }
        public METHOD Method { get; private set; }
        public DateTime RegDatetime { get; private set; }
        public bool Done { get; private set; }

        public PicEvalRow(string line)
        {
            var values = line.Split('\t');
            FileName = values[0];
            EntryName = values[1];
            switch (values[2])
            {
                case "DEL":
                    Method = METHOD.METHOD_DEL;
                    break;
                case "FAV":
                    Method = METHOD.METHOD_FAV;
                    break;
                default:
                    Log.err($"unknown method:{line}");
                    break;
            }
            RegDatetime = DateTime.Parse(values[3]);
            Done = false;
        }
    }

    class TsvRowList
    {
        List<PicEvalRow> RowList;

        public TsvRowList(string filepath)
        {
            RowList = new List<PicEvalRow>();

            using (var fs = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var reader = new StreamReader(fs))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var tsvRow = new PicEvalRow(line);
                        RowList.Add(tsvRow);
                    }
                }
            }
        }

        public List<PicEvalRow> GetRowList()
        {
            return RowList;
        }
    }

    class Table
    {
        List<string> Lines = new();

        public Table(string filepath)
        {
            using (var fs = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var reader = new StreamReader(fs))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        Lines.Add(line);
                    }
                }
            }
        }

        public IEnumerator<string> GetEnumerator()
        {
            foreach (var line in Lines)
            {
                yield return line;
            }
        }

        public static void SetParams(string line, 
            out string picpath,
            out long filesize,
            out int width,
            out int height,
            out string hash)
        {
            var values = line.Split('\t');
            picpath = values[0];
            filesize = long.Parse(values[1]);
            width = int.Parse(values[2]);
            height = int.Parse(values[3]);
            hash = values[4];
        }
    }
}
