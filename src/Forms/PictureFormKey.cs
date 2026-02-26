using PictureManagerApp.src.Lib;
using PictureManagerApp.src.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static PictureManagerApp.src.Model.FileList;
using PictureManagerApp.src.Forms;

namespace PictureManagerApp
{
    public partial class PictureForm : Form
    {
        private readonly Dictionary<Keys, KeyDownFunc> KeyFuncTbl = [];
        private Dictionary<Keys, uint> NumKeyMap = [];

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void InitNumKeyMap()
        {
            NumKeyMap[Keys.NumPad1] = 1;
            NumKeyMap[Keys.NumPad2] = 2;
            NumKeyMap[Keys.NumPad3] = 3;
            NumKeyMap[Keys.NumPad4] = 4;
            NumKeyMap[Keys.NumPad5] = 5;
            NumKeyMap[Keys.NumPad6] = 6;
            NumKeyMap[Keys.NumPad7] = 7;
            NumKeyMap[Keys.NumPad8] = 8;
            NumKeyMap[Keys.NumPad9] = 9;
        }

        private void InitKeys()
        {
            KeyFuncTbl[Keys.Escape] = KeyDownFunc_Escape;

            KeyFuncTbl[Keys.NumPad0] = KeyDownFunc_SelectToggle;

            KeyFuncTbl[Keys.NumPad1] = KeyDownFunc_SelectToggle;
            KeyFuncTbl[Keys.NumPad2] = KeyDownFunc_SelectToggle;
            KeyFuncTbl[Keys.NumPad3] = KeyDownFunc_SelectToggle;
            KeyFuncTbl[Keys.NumPad4] = KeyDownFunc_SelectToggle;
            KeyFuncTbl[Keys.NumPad5] = KeyDownFunc_SelectToggle;
            KeyFuncTbl[Keys.NumPad6] = KeyDownFunc_SelectToggle;
            KeyFuncTbl[Keys.NumPad7] = KeyDownFunc_SelectToggle;
            KeyFuncTbl[Keys.NumPad8] = KeyDownFunc_SelectToggle;
            KeyFuncTbl[Keys.NumPad9] = KeyDownFunc_SelectToggle;

            KeyFuncTbl[Keys.Space] = KeyDownFunc_SlideShow;//KeyDownFunc_SelectToggle;

            KeyFuncTbl[Keys.Delete] = KeyDownFunc_Del;

            KeyFuncTbl[Keys.Tab] = KeyDownFunc_Tab;

            KeyFuncTbl[Keys.F] = KeyDownFunc_Fav;

            //KeyFuncTbl[Keys.F1] = ;used
            //KeyFuncTbl[Keys.F2] = ;used
            //KeyFuncTbl[Keys.F3] = ;used
            //KeyFuncTbl[Keys.F4] = ;used
            //KeyFuncTbl[Keys.F5] = ;used
            KeyFuncTbl[Keys.F6] = KeyDownFunc_ThumbnailChg;
            //KeyFuncTbl[Keys.F9] = ;used

            KeyFuncTbl[Keys.Q] = KeyDownFunc_Next;
            KeyFuncTbl[Keys.A] = KeyDownFunc_Prev;

            KeyFuncTbl[Keys.W] = KeyDownFunc_Home;
            KeyFuncTbl[Keys.Z] = KeyDownFunc_End;

            KeyFuncTbl[Keys.L] = KeyDownFunc_RotLeft;
            KeyFuncTbl[Keys.R] = KeyDownFunc_RotRight;

            KeyFuncTbl[Keys.PageUp] = KeyDownFunc_PageUp;
            KeyFuncTbl[Keys.PageDown] = KeyDownFunc_PageDown;

            KeyFuncTbl[Keys.Left] = KeyDownFunc_Prev;
            KeyFuncTbl[Keys.Right] = KeyDownFunc_Next;
            KeyFuncTbl[Keys.Up] = KeyDownFunc_Up;
            KeyFuncTbl[Keys.Down] = KeyDownFunc_Down;

            KeyFuncTbl[Keys.Home] = KeyDownFunc_Home;
            KeyFuncTbl[Keys.End] = KeyDownFunc_End;

            KeyFuncTbl[Keys.Subtract] = KeyDownFunc_TagWindow;

            switch (mModel.ThumbViewType)
            {
                case THUMBNAIL_VIEW_TYPE.THUMBNAIL_VIEW_DIRECTORY:
                    KeyFuncTbl[Keys.PageUp] = KeyDownFunc_PrevGroup;
                    KeyFuncTbl[Keys.PageDown] = KeyDownFunc_NextGroup;
                    break;
                case THUMBNAIL_VIEW_TYPE.THUMBNAIL_VIEW_LIST:
                    KeyFuncTbl[Keys.Left] = KeyDownFunc_List_Prev;
                    KeyFuncTbl[Keys.Right] = KeyDownFunc_List_Next;
                    KeyFuncTbl[Keys.Up] = KeyDownFunc_List_Up;
                    KeyFuncTbl[Keys.Down] = KeyDownFunc_List_Down;

                    KeyFuncTbl[Keys.Home] = KeyDownFunc_List_Home;
                    KeyFuncTbl[Keys.End] = KeyDownFunc_List_End;
                    break;
                case THUMBNAIL_VIEW_TYPE.THUMBNAIL_VIEW_MANGA:
                    //左に進む（右綴じ）TODO:左綴じ対応？
                    KeyFuncTbl[Keys.Down] = KeyDownFunc_Manga_NextPage;
                    KeyFuncTbl[Keys.Up] = KeyDownFunc_Manga_PrevPage;

                    KeyFuncTbl[Keys.Left] = KeyDownFunc_Next;
                    KeyFuncTbl[Keys.Right] = KeyDownFunc_Prev;
                    break;
                case THUMBNAIL_VIEW_TYPE.THUMBNAIL_VIEW_TILE:
                case THUMBNAIL_VIEW_TYPE.THUMBNAIL_VIEW_OVERVIEW:
                case THUMBNAIL_VIEW_TYPE.THUMBNAIL_VIEW_NEXT:
                default:
                    if (mModel.IsZip())//めんどくさい。。。
                    {
                        KeyFuncTbl[Keys.Up] = KeyDownFunc_PrevGroup;
                        KeyFuncTbl[Keys.Down] = KeyDownFunc_NextGroup;
                    }
                    break;
            }

        }

        //---------------------------------------------------------------------
        // リスト表示
        //---------------------------------------------------------------------
        private bool KeyDownFunc_List_Prev(object sender, KeyEventArgs e)
        {
            //前のファイルに移動（ディレクトリ内をループ）
            mModel.ListPrev();
            return true;
        }

        private bool KeyDownFunc_List_Next(object sender, KeyEventArgs e)
        {
            //次のファイルに移動（ディレクトリ内をループ）
            mModel.ListNext();
            return true;
        }

        private bool KeyDownFunc_List_Up(object sender, KeyEventArgs e)
        {
            mModel.ListUp();
            return true;
        }

        private bool KeyDownFunc_List_Down(object sender, KeyEventArgs e)
        {
            mModel.ListDown();
            return true;
        }

        private bool KeyDownFunc_List_Home(object sender, KeyEventArgs e)
        {
            if (e.Shift)
            {
            }
            else if (e.Control)
            {
                mModel.MovePos(POS_MOVE_TYPE.MOVE_HOME);
            }
            else
            {
                //同一ディレクトリの先頭に移動
                mModel.MoveToDirTopImage();
            }
            return true;
        }

        private bool KeyDownFunc_List_End(object sender, KeyEventArgs e)
        {
            if (e.Shift)
            {
            }
            else if (e.Control)
            {
                mModel.MovePos(POS_MOVE_TYPE.MOVE_LAST);
            }
            else
            {
                //同一ディレクトリの末尾に移動
                mModel.MoveToDirEndImage();
            }
            return true;
        }

        private bool KeyDownFunc_Manga_PrevPage(object sender, KeyEventArgs e)
        {
            mModel.PrevPage();
            return true;
        }

        private bool KeyDownFunc_Manga_NextPage(object sender, KeyEventArgs e)
        {
            {
                mModel.NextPage();
            }
            return true;
        }

        //
        private bool KeyDownFunc_Prev(object sender, KeyEventArgs e)
        {
            if (e.Shift)
            {
                //mModel.PrevMarkedImage();
            }
            else if (e.Control)
            {
                mModel.PrevDirImage();
            }
            else
            {
                mModel.Prev();
            }
            return true;
        }

        private bool KeyDownFunc_Next(object sender, KeyEventArgs e)
        {
            if (e.Shift)
            {
                mModel.NextMarkedImage();
            }
            else if (e.Control)
            {
                mModel.NextDirImage();
            }
            else
            {
                mModel.Next();
            }
            return true;
        }

        private bool KeyDownFunc_Up(object sender, KeyEventArgs e)
        {
            mModel.Up();
            return true;
        }

        private bool KeyDownFunc_Down(object sender, KeyEventArgs e)
        {
            mModel.Down();
            return true;
        }

        private bool KeyDownFunc_PageUp(object sender, KeyEventArgs e)
        {
            mModel.PageUp();
            return true;
        }

        private bool KeyDownFunc_PageDown(object sender, KeyEventArgs e)
        {
            mModel.PageDown();
            return true;
        }

        private bool KeyDownFunc_Home(object sender, KeyEventArgs e)
        {
            if (e.Shift)
            {
            }
            else if (e.Control)
            {
                mModel.MovePos(POS_MOVE_TYPE.MOVE_HOME);
            }
            else
            {
                mModel.PrevDirImage();
            }
            return true;
        }

        private bool KeyDownFunc_End(object sender, KeyEventArgs e)
        {
            if (e.Shift)
            {
            }
            else if (e.Control)
            {
                mModel.MovePos(POS_MOVE_TYPE.MOVE_LAST);
            }
            else
            {
                mModel.MoveToDirEndImage();
            }
            return true;
        }

        private bool KeyDownFunc_Tab(object sender, KeyEventArgs e)
        {
            if (e.Shift)
            {
                mModel.PrevDirTopImage();
            }
            else if (e.Control)
            {
            }
            else
            {
                //mModel.MovePos(POS_MOVE_TYPE.MOVE_NEXT_DIR);
                mModel.NextDirImage();
            }
            return true;
        }

        private bool KeyDownFunc_NextGroup(object sender, KeyEventArgs e)
        {
            mModel.NextDirImage();

            return true;
        }

        private bool KeyDownFunc_PrevGroup(object sender, KeyEventArgs e)
        {
            mModel.PrevDirTopImage();

            return true;
        }

        private bool KeyDownFunc_Fav(object sender, KeyEventArgs e)
        {
            DoAction(ACTION_TYPE.ACTION_ADD_FAV_LIST);
            return true;
        }

        private bool KeyDownFunc_SelectToggle(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.NumPad0:
                    if (e.Shift)
                    {
                    }
                    else if (e.Control)
                    {
                        mModel.MarkAllSameDirFiles();
                    }
                    else
                    {
                        mModel.toggleMark();

                        if (mModel.ThumbViewType == THUMBNAIL_VIEW_TYPE.THUMBNAIL_VIEW_LIST)
                        {
                            //TODO:フォルダ内最後の画像だった場合は次のディレクトリの最後に移動する


                            mModel.Next();
                        }
                        else
                        {
                            mModel.Next();
                        }
                    }
                    break;
                default:
                    if (NumKeyMap.ContainsKey(e.KeyCode))
                    {
                        SetDstStr(mModel.GetDstStr(NumKeyMap[e.KeyCode]));
                    }
                    break;
            }
            return true;
        }

        private void SetDstStr(string str)
        {
            mModel.SetDstStr(str);
            var err = false;
            if (err)
            {
                var text = "err:" + str;
                var caption = "失敗";
                MessageBox.Show(text,
                    caption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {
                mModel.Next();
            }
        }

        private bool KeyDownFunc_Del(object sender, KeyEventArgs e)
        {
            if (e.Shift)
            {
            }
            else if (e.Control)
            {
            }
            else
            {
                mModel.AddDelList();
                mModel.Next();
            }
            return true;
        }

        private bool KeyDownFunc_Escape(object sender, KeyEventArgs e)
        {
            return WindowQuitOp();
        }

        private bool KeyDownFunc_ThumbnailChg(object sender, KeyEventArgs e)
        {
            //NextPic = !NextPic;
            mModel.ToggleThumbView();
            InitKeys();

            return true;
        }

        private void PictureForm_KeyDown(object sender, KeyEventArgs e)
        {
            //Log.trc($"[S]:{e.KeyCode}");
            if (KeyFuncTbl.TryGetValue(e.KeyCode, out KeyDownFunc value))
            {
                if (value(sender, e))
                {
                    UpdatePicture();
                }
            }
            //Log.trc($"[E]");
        }

        private bool KeyDownFunc_TagWindow(object sender, KeyEventArgs e)
        {
            var list = mModel.GetCharacterNameList();
            var f = new TagCandForm(list);
            var dresult = f.ShowDialog();
            if (dresult == DialogResult.OK)
            {
                var tag = f.GetSelectedTag();
                if (tag != null && tag != "")
                {
                    SetDstStr(tag);
                }
            }

            return true;
        }

        private bool KeyDownFunc_SlideShow(object sender, KeyEventArgs e)
        {
            ToggleSlideshow(mSlideMs);
            return true;
        }

        private bool KeyDownFunc_RotLeft(object sender, KeyEventArgs e)
        {
            //TODO:トグル
            mModel.ROT_TYPE = DISP_ROT_TYPE.DISP_ROT_R270;
            return true;
        }

        private bool KeyDownFunc_RotRight(object sender, KeyEventArgs e)
        {
            //TODO　トグル

            mModel.ROT_TYPE = DISP_ROT_TYPE.DISP_ROT_R090; 
            return true;
        }

        private bool KeyDownFunc_(object sender, KeyEventArgs e)
        {
            return true;
        }
    }
}
