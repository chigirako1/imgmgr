using PictureManagerApp.src.Lib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;

namespace PictureManagerApp.src.Model
{
    internal class DirList
    {
        private List<DirItem> mDirList = new();

        private Dictionary<string, DirItem> mDic = new();

        //=====================================================================
        // クラスメソッド
        //=====================================================================
        public static IEnumerable<string> EnumerateDirectories(string path, bool sort = false)
        {
            //var dirs = Directory.EnumerateDirectories(path);
            //return dirs.OrderBy(x => x);

            IEnumerable<string> dirs = null;

            // 親ディレクトリ内のすべてのサブディレクトリを取得し、
            // 各ディレクトリのファイル数をカウントしてソートする
            try
            {
                dirs = Directory.EnumerateDirectories(path);
                if (sort)
                {
                    var sortedDirectories = dirs
                        .Select(dirPath => new
                        {
                            DirectoryPath = dirPath,
                            FileCount = GetFileCount(dirPath) // 各ディレクトリのファイル数を取得
                        })
                        .OrderByDescending(item => item.FileCount);

                    dirs = sortedDirectories.Select(x => x.DirectoryPath);
                }
                else
                {
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"アクセス権限エラー: {ex.Message}");
                Console.WriteLine("管理者として実行するか、適切な権限を持つディレクトリを指定してください。");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"エラーが発生しました: {ex.Message}");
            }

            return dirs;
        }

        /// <summary>
        /// 指定されたディレクトリ内のファイルの数を取得します。
        /// サブディレクトリは含まれません。
        /// </summary>
        /// <param name="directoryPath">ファイル数をカウントするディレクトリのパス。</param>
        /// <returns>ディレクトリ内のファイルの数。</returns>
        private static int GetFileCount(string directoryPath)
        {
            try
            {
                // EnumerateFiles を使用すると、GetFiles() のようにすべてのファイルパスを
                // メモリに読み込むのではなく、イテレータとしてファイルパスを返すため、
                // 大量のファイルがある場合にメモリ効率が良いです。
                var dirs = Directory.EnumerateFiles(directoryPath,
                    "*",
                    //SearchOption.TopDirectoryOnly
                    SearchOption.AllDirectories
                    );
                var cnt = dirs.Count();
                return cnt;
            }
            catch (UnauthorizedAccessException)
            {
                // アクセス権限がない場合など
                Console.WriteLine($"警告: ディレクトリ '{directoryPath}' へのアクセスが拒否されました。ファイル数は0として扱われます。");
                return 0;
            }
            catch (DirectoryNotFoundException)
            {
                // ディレクトリが見つからない場合（非常に稀ですが、タイミングによっては発生する可能性あり）
                Console.WriteLine($"警告: ディレクトリ '{directoryPath}' が見つかりませんでした。ファイル数は0として扱われます。");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"エラー: ディレクトリ '{directoryPath}' のファイル数取得中にエラーが発生しました: {ex.Message}");
                return 0;
            }
        }

        //=====================================================================
        // インスタンスメソッド
        //=====================================================================
        public void Add(DirItem diritem)
        {
            if (this.mDic.ContainsKey(diritem.Path))
            {
                //登録済み
                //??
                throw new InvalidOperationException();
            }

            mDirList.Add(diritem);
            mDic.Add(diritem.Path, diritem);
        }

        public void Update(PictureModel model)
        {
            var path = model.GetPath();
            if (mDic.ContainsKey(path))
            {
                var diritem = mDic[path];
                var idx = model.GetCurrentFileIndex();
                diritem.PageNo = idx;
                diritem.TotalPageNo = model.PictureTotalNumber;
            }
            else
            {
                throw new InvalidOperationException();//tekitou
            }
        }

        public DirItem GetDirItem(string path)
        {
            if (this.mDic.ContainsKey(path))
            {
                return mDic[path];
            }

            return null;
        }
    }
}
