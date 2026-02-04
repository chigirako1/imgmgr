using PictureManagerApp.src.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PictureManagerApp.src.Lib
{
    internal static class Dgv
    {
        private const int LIST_THUMBNAIL_WIDTH = 64;
        private const int LIST_THUMBNAIL_HEIGHT = 64;

        private const string LIST_DGV_ZIP_CLM_IDX = "#";
        private const string LIST_DGV_ZIP_CLM_STAR = "★";
        private const string LIST_DGV_ZIP_CLM_PAGE_TOTAL = "全";
        private const string LIST_DGV_ZIP_CLM_PAGE_NOW = "今";
        private const string LIST_DGV_ZIP_CLM_PERCENT = "%";
        private const string LIST_DGV_ZIP_CLM_THUMB = "T";
        private const string LIST_DGV_ZIP_CLM_NAME = "名前";
        private const string LIST_DGV_ZIP_CLM_RATING = "評価";
        private const string LIST_DGV_ZIP_CLM_FILENAME = "ファイル名";
        private const string LIST_DGV_ZIP_CLM_PATH = "パス";

        //private bool list_thumnail = true;
        //private bool list_thumnail = false;

        public static void InitiZipList(IEnumerable<string> zipfiles, DataGridView dgv, bool list_thumbnail)
        {
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgv.AllowUserToAddRows = false;

            if (list_thumbnail)
            {
                dgv.RowTemplate.MinimumHeight = LIST_THUMBNAIL_HEIGHT;
            }

            //dgv.ReadOnly = true;                      //読取専用
            //dgv.EditMode = DataGridViewEditMode.EditOnKeystroke;

            dgv.AllowUserToDeleteRows = false;        //行削除禁止
            dgv.AllowUserToAddRows = false;           //行挿入禁止
            
            //dgv.AllowUserToResizeRows = false;        //行の高さ変更禁止
            
            dgv.RowHeadersVisible = false;            //行ヘッダーを非表示にする
            dgv.MultiSelect = false;                  //セル、行、列が複数選択禁止
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;       //セルを選択すると行全体が選択されるようにする

            var ds = GetDataTable(zipfiles, list_thumbnail);
            if (ds == null)
            {
                return;
            }
            dgv.DataSource = ds;

            dgv.Columns[LIST_DGV_ZIP_CLM_STAR].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.Columns[LIST_DGV_ZIP_CLM_IDX].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns[LIST_DGV_ZIP_CLM_PAGE_NOW].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns[LIST_DGV_ZIP_CLM_PAGE_TOTAL].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns[LIST_DGV_ZIP_CLM_PERCENT].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns[LIST_DGV_ZIP_CLM_RATING].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

        }

        static private DataTable GetDataTable(IEnumerable<string> zipfiles, bool list_thumbnail)
        {
            var typeint32 = Type.GetType("System.Int32");

            var dt = new DataTable();

            var col_idx = dt.Columns.Add(LIST_DGV_ZIP_CLM_IDX, typeint32);
            var col_star = dt.Columns.Add(LIST_DGV_ZIP_CLM_STAR);

            var col_page_total = dt.Columns.Add(LIST_DGV_ZIP_CLM_PAGE_TOTAL, typeint32);
            var col_page_now = dt.Columns.Add(LIST_DGV_ZIP_CLM_PAGE_NOW, typeint32);
            var col_percent = dt.Columns.Add(LIST_DGV_ZIP_CLM_PERCENT, typeint32);

            var col_thumbnail = dt.Columns.Add(LIST_DGV_ZIP_CLM_THUMB);
            col_thumbnail.DataType = System.Type.GetType("System.Byte[]");
            col_thumbnail.AllowDBNull = true;

            var col_name = dt.Columns.Add(LIST_DGV_ZIP_CLM_NAME);
            var col_rating = dt.Columns.Add(LIST_DGV_ZIP_CLM_RATING, typeint32);

            var col_filename = dt.Columns.Add(LIST_DGV_ZIP_CLM_FILENAME);
            var col_path = dt.Columns.Add(LIST_DGV_ZIP_CLM_PATH);


            //var objColumn = new DataColumn("Checkbox", Type.GetType("Bool"));
            var objColumn = new DataColumn("Checkbox", typeof(bool));
            dt.Columns.Add(objColumn);
            //var column = new DataGridViewCheckBoxColumn();
            //dt.Columns.Add(column);


            int i = 0;
            foreach (var f in zipfiles.OrderBy(x => x))
            {
                i++;

                var newRow = dt.NewRow();
                newRow.SetField(col_idx, i);
                newRow.SetField(col_star, "");

                if (list_thumbnail)
                {
                    var ba = MyFiles.GetThumbnailByteArray(f, LIST_THUMBNAIL_WIDTH, LIST_THUMBNAIL_HEIGHT, 1);
                    newRow.SetField(col_thumbnail, ba);
                }

                var filename = Path.GetFileName(f);
                newRow.SetField(col_filename, filename);
                newRow.SetField(col_path, f);

                var info = DirItem.GetInfoFromFilename(filename);
                newRow.SetField(col_name, info.Name);
                newRow.SetField(col_rating, info.Rating);

                newRow.SetField(col_page_total, info.PageTotal);
                newRow.SetField(col_page_now, 0);
                newRow.SetField(col_percent, 0);

                dt.Rows.Add(newRow);
            }

            return dt;
        }

        static public int GetPathColIdx(DataGridView dgv)
        {
            var idx = dgv.Columns[LIST_DGV_ZIP_CLM_PATH].Index;
            return idx;
        }

        static public string GetPath(DataGridView dgv, DataGridViewRow row)
        {
            var idx = Dgv.GetPathColIdx(dgv);

            var filepath = (string)row.Cells[idx].Value;

            return filepath;
        }

        static public int GetRowIdx(DataGridView dgv)
        {
            var dt2 = (DataTable)dgv.DataSource;
            var dr2 = dt2.Rows[dgv.CurrentRow.Index];
            var row_idx = dgv.CurrentRow.Index;

            return row_idx;
        }

        static public void SetColumnValue(DataGridView dgv, int row_idx, int col_idx, string chara)
        {
            var datasource = (DataTable)dgv.DataSource;
            var cl = datasource.Columns[col_idx];

            datasource.Rows[row_idx].SetField(cl, chara);
        }

        static public void SetThumbnail(DataGridView dgv, int row_idx, string zippath, int pic_idx = 1)
        {
            var datasource = (DataTable)dgv.DataSource;
            var cl = datasource.Columns[5];

            int width;
            int height;
            var test = true;
            if (test)
            {
                dgv.RowTemplate.MinimumHeight = LIST_THUMBNAIL_HEIGHT;
                width = LIST_THUMBNAIL_WIDTH;
                height = LIST_THUMBNAIL_HEIGHT;
            }
            else
            {
                width = LIST_THUMBNAIL_WIDTH / 2;
                height = LIST_THUMBNAIL_HEIGHT / 2;
            }

            var idx = pic_idx;
            var ba = MyFiles.GetThumbnailByteArray(zippath, width, height, idx);
            datasource.Rows[row_idx].SetField(cl, ba);
        }

        public static void InitPxvDGV(DataTable dt)
        {
            var dbPath = Sqlite.GetSqliteFilePath();

            using (var con = new SQLiteConnection("Data Source=" + dbPath))
            {
                con.Open();

                var tblname = "artists";
                var colname = "id, pxvid, pxvname, feature, rating, filenum, status";
                var where_p = "";
                var pause = false;
                if (pause)
                {
                    where_p += $" status IN ({GetSqlCond()})";
                    where_p += " AND ";
                }
                where_p += " feature = 'AI'";
                var order_phrase = " status DESC, rating DESC, filenum DESC";
                var adapter = Sqlite.GetSQLiteDataAdapter(con, tblname, colname, where_p, order_phrase);
                adapter.Fill(dt);
            }
        }

        private static string GetSqlCond()
        {
            string[] list = {
                "退会",
                "停止",//凍結？

                "長期更新なし",
                "半年以上更新なし",
                "彼岸",
                "別アカウントに移行",//
                "作品ゼロ",
                //"一部消えた",
                "ほぼ消えた",
            };

            var list2 = list.Select(x => $"'{x}'").ToArray();

            return System.String.Join(",", list2);
        }

    }
}
