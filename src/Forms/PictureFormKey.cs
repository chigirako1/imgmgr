﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using PictureManagerApp.src.Lib;
using PictureManagerApp.src.Model;


namespace PictureManagerApp
{
    public partial class PictureForm : Form
    {
        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void InitKeys()
        {
            KeyFuncTbl[Keys.Escape] = KeyDownFunc_Escape;

            KeyFuncTbl[Keys.NumPad0] = KeyDownFunc_SelectToggle;
            KeyFuncTbl[Keys.Space] = KeyDownFunc_SelectToggle;

            KeyFuncTbl[Keys.Delete] = KeyDownFunc_Del;

            //KeyFuncTbl[Keys.F1] = ;used
            //KeyFuncTbl[Keys.F2] = ;used
            //KeyFuncTbl[Keys.F3] = ;used
            //KeyFuncTbl[Keys.F4] = ;used
            //KeyFuncTbl[Keys.F5] = ;used
            KeyFuncTbl[Keys.F6] = KeyDownFunc_ThumbnailChg;
            //KeyFuncTbl[Keys.F9] = ;used

            KeyFuncTbl[Keys.A] = KeyDownFunc_Left;
            KeyFuncTbl[Keys.S] = KeyDownFunc_Right;
            KeyFuncTbl[Keys.W] = KeyDownFunc_Home;
            KeyFuncTbl[Keys.Z] = KeyDownFunc_End;

            KeyFuncTbl[Keys.PageUp] = KeyDownFunc_PageUp;
            KeyFuncTbl[Keys.PageDown] = KeyDownFunc_PageDown;

            switch (mModel.ThumbViewType)
            {
                case THUMBNAIL_VIEW_TYPE.THUMBNAIL_VIEW_LIST:
                    KeyFuncTbl[Keys.Left] = KeyDownFunc_List_Left;
                    KeyFuncTbl[Keys.Right] = KeyDownFunc_List_Right;
                    KeyFuncTbl[Keys.Up] = KeyDownFunc_List_Up;
                    KeyFuncTbl[Keys.Down] = KeyDownFunc_List_Down;

                    KeyFuncTbl[Keys.Home] = KeyDownFunc_List_Home;
                    KeyFuncTbl[Keys.End] = KeyDownFunc_List_End;
                    break;
                case THUMBNAIL_VIEW_TYPE.THUMBNAIL_VIEW_TILE:
                case THUMBNAIL_VIEW_TYPE.THUMBNAIL_VIEW_OVERVIEW:
                case THUMBNAIL_VIEW_TYPE.THUMBNAIL_VIEW_NEXT:
                default:
                    KeyFuncTbl[Keys.Left] = KeyDownFunc_Left;
                    KeyFuncTbl[Keys.Right] = KeyDownFunc_Right;
                    KeyFuncTbl[Keys.Up] = KeyDownFunc_Up;
                    KeyFuncTbl[Keys.Down] = KeyDownFunc_Down;

                    KeyFuncTbl[Keys.Home] = KeyDownFunc_Home;
                    KeyFuncTbl[Keys.End] = KeyDownFunc_End;

                    break;
            }

        }

        private bool KeyDownFunc_List_Left(object sender, KeyEventArgs e)
        {
            //前のファイルに移動（ディレクトリ内をループ）
            mModel.ListPrev();
            return true;
        }

        private bool KeyDownFunc_List_Right(object sender, KeyEventArgs e)
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

        //
        private bool KeyDownFunc_Left(object sender, KeyEventArgs e)
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

        private bool KeyDownFunc_Right(object sender, KeyEventArgs e)
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

        private bool KeyDownFunc_SelectToggle(object sender, KeyEventArgs e)
        {
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
                mModel.Next();
            }
            return true;
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
            Log.trc($"[S]:{e.KeyCode}");
            if (KeyFuncTbl.TryGetValue(e.KeyCode, out KeyDownFunc value))
            {
                if (value(sender, e))
                {
                    UpdatePicture();
                }
            }
            Log.trc($"[E]");
        }

        private bool KeyDownFunc_(object sender, KeyEventArgs e)
        {
            return true;
        }


    }
}
