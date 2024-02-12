using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Text.RegularExpressions;

namespace PictureManagerApp.src.Lib
{
    public class PxvArtist
    {
        public long PxvID { set; get; }
        public string PxvName { set;  get; }
        public long Rating{ set; get; }
        public string R18 { set; get; }
        public string Feature { set; get; }

        public PxvArtist(string path)
        {
            var pattern = @"\((\d+)\)";
            System.Text.RegularExpressions.MatchCollection mc = System.Text.RegularExpressions.Regex.Matches(path, pattern);
            foreach (System.Text.RegularExpressions.Match m in mc)
            {
                var pxvid = Int32.Parse(m.Groups[1].Value);
                Sqlite.test(pxvid, this);
                break;
            }
        }
    }

    public class Sqlite
    {
        private const string DATA_SRC = "D:\\data\\src\\vs_cs\\development.sqlite3";

        public static void test(int pxvid, PxvArtist pxvartist)
        {
            var sqlConnectionSb = new SQLiteConnectionStringBuilder { DataSource = DATA_SRC };
            using (var cn = new SQLiteConnection(sqlConnectionSb.ToString()))
            {
                cn.Open();

                using (var cmd = new SQLiteCommand(cn))
                {
                    //cmd.CommandText = "select sqlite_version()";
                    //result = (string)cmd.ExecuteScalar();

                    cmd.CommandText = $"SELECT * FROM artists WHERE pxvid = {pxvid}";
                    using (var reader = cmd.ExecuteReader())
                    {
                        try
                        {
                            if (reader.Read())
                            {
                                pxvartist.PxvID = (long)reader["pxvid"];
                                pxvartist.PxvName = (string)reader["pxvname"];
                                pxvartist.Rating = (long)reader["rating"];
                                pxvartist.R18 = (string)reader["r18"];
                                pxvartist.Feature = (string)reader["feature"];
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.trc(ex.Message);
                        }
                    }
                }

            }
        }
    }
}
