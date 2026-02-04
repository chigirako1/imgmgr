using PictureManagerApp.src.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PictureManagerApp.src.Lib
{
    public class Config
    {
        private readonly string KEY_TEST = "test";
        private readonly string KEY_LAST_PATH = "dir_path";
        private readonly string KEY_DEL_LIST = "del_list";
        private readonly string KEY_NUMKEY_STR = "numkey_str";
        //private readonly string KEY_THUMB_N_ROW = "thumb_n_row";
        //private readonly string KEY_THUMB_N_COL = "thumb_n_col";

        public string TEST_ELEM { get; set; }
        public string DelListSavePos { get; set; }
        public string LastPath { get; set; }
        public string NumkeyStrings { get; set; }

        public void Load()
        {
            var config = GetCfg();
            try
            {
                TEST_ELEM = config.AppSettings.Settings[KEY_TEST].Value;
                if (config.AppSettings.Settings[KEY_LAST_PATH] != null)
                {
                    LastPath = config.AppSettings.Settings[KEY_LAST_PATH].Value;
                }
                
                DelListSavePos = config.AppSettings.Settings[KEY_DEL_LIST].Value;
                //Log.trc($"del_list={DelListSavePos}");

                NumkeyStrings = config.AppSettings.Settings[KEY_NUMKEY_STR].Value;
            }
            catch (System.Exception e)
            {
                Log.trc(e.ToString());
                TEST_ELEM = "a";
            }
        }

        public void Save()
        {
            var config = GetCfg();
            try
            {
                if (config.AppSettings.Settings[KEY_TEST].Value != TEST_ELEM)
                {
                    config.AppSettings.Settings[KEY_TEST].Value = TEST_ELEM;
                }
            }
            catch (System.Exception e)
            {
                Log.trc(e.ToString());
            }
            //config.Save();
        }

        public void SavePath(string path)
        {
            var list = PictureModel.GetDirPathList();
            if (list.Contains(path))
            {
                Log.trc($"規定のパスは保存しない。'{path}'");
                return;
            }

            var config = GetCfg();
            //try
            {
                if (config.AppSettings.Settings[KEY_LAST_PATH] == null)
                {
                    config.AppSettings.Settings.Add(KEY_LAST_PATH, path);
                }

                if (config.AppSettings.Settings[KEY_LAST_PATH].Value != path)
                {
                    config.AppSettings.Settings[KEY_LAST_PATH].Value = path;
                }

                config.Save();
            }
            //catch (System.Exception e)
            {
                //Log.trc(e.ToString());
            }
            //config.Save();
        }

        private Configuration GetCfg()
        {
            var configFile = @"test.config";
            var exeFileMap = new ExeConfigurationFileMap { ExeConfigFilename = configFile };
            var config = ConfigurationManager.OpenMappedExeConfiguration(exeFileMap, ConfigurationUserLevel.None);

            return config;
        }
    }
}
