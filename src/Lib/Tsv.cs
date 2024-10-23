using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PictureManagerApp.src.Lib
{
    class TsvRow
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

        private string FileName;
        private string EntryName;
        private METHOD Method;
        private DateTime RegDatetime;
        private bool Done;

        public TsvRow(string line)
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
        List<TsvRow> RowList;

        public TsvRowList(string filepath)
        {
            RowList = new List<TsvRow>();

            using (var reader = new StreamReader(filepath))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var tsvRow = new TsvRow(line);
                    RowList.Add(tsvRow);
                }
            }
        }
    }

    class Tsv
    {
        public static void BuildRows()
        {
            

        }
    }
}
