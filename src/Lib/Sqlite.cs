#if DEBUG
//#define DEV_SQL
#endif

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
using System.Xml.Linq;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using System.Windows.Forms;
using static PictureManagerApp.src.Lib.PicEvalRow;

namespace PictureManagerApp.src.Lib
{
    public class Sqlite
    {
#if DEV_SQL
        //private const string DATA_SRC = @"D:\data\src\vs_cs\development.sqlite3";
        //private const string DATA_SRC = @"D:\data\src\ror\myapp\db\development - bak240324.sqlite3";
        private const string DATA_SRC_DIRPATH = @"D:\export-done\test\out\";
        private const string DATA_SRC_FILENAME = "development.sqlite3";
#else
        private const string DATA_SRC_DIRPATH = @"D:\data\src\ror\myapp\db\";
        private const string DATA_SRC_FILENAME = "development.sqlite3";
#endif
        private const string DATA_SRC_PATH = DATA_SRC_DIRPATH + DATA_SRC_FILENAME;

        public static string SQL_STATEMENT_WHERE_NO_UPDATE = "status != ''";
        public static string SQL_STATEMENT_WHERE_3D = "feature = '3D'";
        public static string SQL_STATEMENT_WHERE_AI = "feature = 'AI'";
        public static string SQL_STATEMENT_WHERE_AI_SUSPEND = "feature = 'AI' and status = '停止'";
        public static string SQL_STATEMENT_WHERE_AI_NO_UPDATE = "feature = 'AI' and status != ''";


        private static string sqlite_db_file_path;

        static Sqlite()
        {
            string data_src_path;
            if (File.Exists(DATA_SRC_PATH))
            {
                data_src_path = DATA_SRC_PATH;
            }
            else
            {
                var basePath = System.Environment.CurrentDirectory;
                var filePath = DATA_SRC_FILENAME;
                data_src_path = Path.Combine(basePath, filePath);
            }

            sqlite_db_file_path = data_src_path;
        }

        public static string GetSqliteFilePath()
        {
            return sqlite_db_file_path;
        }

        public static SQLiteDataAdapter GetSQLiteDataAdapter(SQLiteConnection con, string tblname, string colname)
        {
            var adapter = new SQLiteDataAdapter($"SELECT {colname} FROM {tblname};", con);
            return adapter;
        }

        public static SQLiteDataAdapter GetSQLiteDataAdapter(SQLiteConnection con, string tblname, string colname, string where_phrase)
        {
            var adapter = new SQLiteDataAdapter($"SELECT {colname} FROM {tblname} WHERE {where_phrase};", con);
            return adapter;
        }

        private static SQLiteConnection GetSQLiteConnection()
        {
            //TODO:なんかやたら呼ばれる。サムネイル更新の度？に呼ばれるので対策必要
            //Log.log($"db file='{sqlite_db_file_path}'");
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

        public static long GetPxvArtistIdFromName(string pxv_user_name)
        {
            using (var cn = GetSQLiteConnection())
            {
                cn.Open();
                using (var cmd = new SQLiteCommand(cn))
                {
                    //cmd.CommandText = "select sqlite_version()";
                    //result = (string)cmd.ExecuteScalar();

                    cmd.CommandText = $"SELECT pxvid FROM artists WHERE pxvname = '{pxv_user_name}'";
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var pxvid = (long)reader["pxvid"];
                            return pxvid;
                        }
                    }
                }
                return 0;
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

        public static void UpdatePxvArtistDelInfo(long pxvid, string del_info)
        {
            using (var cn = GetSQLiteConnection())
            {
                cn.Open();
                using (var cmd = new SQLiteCommand(cn))
                {
                    cmd.CommandText = $"UPDATE artists set del_info = '{del_info}' WHERE pxvid = {pxvid}";
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

        private static int GetMaxId(string tbl_name, SQLiteConnection cn)
        {
            var sql_str = $"SELECT MAX(id) FROM {tbl_name};";
            var selectMaxCmd = new SQLiteCommand(sql_str, cn);
            object val = selectMaxCmd.ExecuteScalar();
            var maxId = int.Parse(val.ToString());
            return maxId;
        }

        public static void UpdateTweetsTable(long tweetid, string screen_name)
        {
            using (var cn = GetSQLiteConnection())
            {
                cn.Open();
                using (var cmd = new SQLiteCommand(cn))
                {
                    var tbl_name = "tweets";
                    var chg_col_name = "status";
                    var col_name_cond = "tweet_id";
                    var col_status_val = "'重複'";

                    var update = false;
                    cmd.CommandText = $"SELECT * FROM {tbl_name} WHERE {col_name_cond} = {tweetid}";
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            //既存のレコードがあるので更新
                            update = true;
                        }
                    }

                    if (update)
                    {
                        cmd.CommandText = $"UPDATE {tbl_name} set {chg_col_name} = {col_status_val}, updated_at = CURRENT_TIMESTAMP" +
                                            $" WHERE {col_name_cond} = {tweetid}";
                        var result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            Log.log($"更新成功\t'{tweetid}'@{screen_name}");
                        }
                        else
                        {
                            Log.log($"更新失敗\t'{tweetid}@{screen_name}'");
                        }
                    }
                    else
                    {
                        var maxId = GetMaxId(tbl_name, cn);
                        cmd.CommandText = $"INSERT into {tbl_name}(id, tweet_id, screen_name, status, created_at, updated_at)" +
                                          $"values({maxId + 1}, {tweetid}, '{screen_name}', {col_status_val}, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP)";
                        var changedline = cmd.ExecuteNonQuery();
                        Log.log($"変更した行の数:{changedline}\t'{tweetid}@{screen_name}'");
                    }
                }
                cn.Close();
            }
        }
    }//class
}//namespace 
