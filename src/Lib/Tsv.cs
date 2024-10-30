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

        public string FileName { get; private set; }
        public string EntryName { get; private set; }
        public METHOD Method { get; private set; }
        public DateTime RegDatetime { get; private set; }
        public bool Done { get; private set; }

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

        public List<TsvRow> hoge()
        {
            return RowList;
        }
    }

    class Tsv
    {
        public static void BuildRows()
        {
            

        }
    }
}
