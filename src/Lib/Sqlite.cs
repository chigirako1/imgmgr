using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Text.RegularExpressions;
using static System.Windows.Forms.LinkLabel;
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;

namespace PictureManagerApp.src.Lib
{
    public class Sqlite
    {
        //private const string DATA_SRC = @"D:\data\src\vs_cs\development.sqlite3";
        //private const string DATA_SRC = @"D:\data\src\ror\myapp\db\development - bak240324.sqlite3";
        private const string DATA_SRC_FILENAME = "development.sqlite3";
        private const string DATA_SRC_PATH = @"D:\data\src\ror\myapp\db\" + DATA_SRC_FILENAME;

        public static string SQL_STATEMENT_WHERE_NO_UPDATE = "status != ''";
        public static string SQL_STATEMENT_WHERE_3D = "feature = '3D'";
        public static string SQL_STATEMENT_WHERE_AI = "feature = 'AI'";
        public static string SQL_STATEMENT_WHERE_AI_SUSPEND = "feature = 'AI' and status = '停止'";
        public static string SQL_STATEMENT_WHERE_AI_NO_UPDATE = "feature = 'AI' and status != ''";


        private static string sqlite_db_file_path;

        static Sqlite()
        {
            var data_src_path = DATA_SRC_PATH;
            if (File.Exists(data_src_path))
            {

            }
            else
            {
                var basePath = System.Environment.CurrentDirectory;
                var filePath = DATA_SRC_FILENAME;
                data_src_path = Path.Combine(basePath, filePath);
            }

            sqlite_db_file_path = data_src_path;
        }

        private static SQLiteConnection GetSQLiteConnection()
        {
            var sqlConnectionSb = new SQLiteConnectionStringBuilder { DataSource = sqlite_db_file_path };
            return new SQLiteConnection(sqlConnectionSb.ToString());
        }


        public static void GetPxvArtistInfo(int pxvid, PxvArtist pxvartist)
        {
            using (var cn = GetSQLiteConnection())
            {
                cn.Open();
                using (var cmd = new SQLiteCommand(cn))
                {
                    //cmd.CommandText = "select sqlite_version()";
                    //result = (string)cmd.ExecuteScalar();

                    cmd.CommandText = $"SELECT * FROM artists WHERE pxvid = {pxvid}";
                    using (var reader = cmd.ExecuteReader())
                    {
                        SetPxvArtistInfo(reader, pxvartist);
                    }
                }
            }
        }

        public static void UpdatePxvArtistRating(long pxvid, long rating)
        {
            using (var cn = GetSQLiteConnection())
            {
                cn.Open();
                using (var cmd = new SQLiteCommand(cn))
                {
                    // データを更新する
                    //cmd.CommandText = "UPDATE sample set title = 'SQLite データ更新' WHERE no = 1";
                    //cmd.ExecuteNonQuery();
                    cmd.CommandText = $"UPDATE artists set rating = {rating} WHERE pxvid = {pxvid}";
                    var changedline = cmd.ExecuteNonQuery();
                    Log.trc($"変更した行の数:{changedline}");
                }
                cn.Close();
            }
        }

        public static List<PxvArtist> GetPxvArtists(string where_phrase)
        {
            var pxv_artists = new List<PxvArtist>();

            
            using (var cn = GetSQLiteConnection())
            {
                cn.Open();
                using (var cmd = new SQLiteCommand(cn))
                {
                    cmd.CommandText = $"SELECT * FROM artists WHERE " + where_phrase;
                    using (var reader = cmd.ExecuteReader())
                    {
                        try
                        {
                            int cnt = 0;
                            while (reader.Read())
                            {
                                //var pxvid = (long)reader["pxvid"];
                                //var recent_filenum = (long)reader["recent_filenum"];
                                //var filenum = (long)reader["filenum"];
                                cnt++;

                                var pxvartist = new PxvArtist();
                                pxvartist.SetPxvArtistParam(reader);
                                pxv_artists.Add(pxvartist);

                                //Log.trc($"{cnt} {pxvartist.PxvName}({pxvartist.PxvID})=|{{{pxvartist.Filenum}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.trc(ex.Message);
                        }
                    }
                }
            }

            return pxv_artists;
        }

        private static void SetPxvArtistInfo(SQLiteDataReader reader, PxvArtist pxvartist)
        {
            try
            {
                if (reader.Read())
                {
                    pxvartist.SetPxvArtistParam(reader);
                }
                else
                {
                    //Log.trc("no read ");
                }
            }
            catch (Exception ex)
            {
                Log.trc(ex.Message);
            }
        }

    }
}
