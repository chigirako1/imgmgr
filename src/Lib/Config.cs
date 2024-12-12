using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PictureManagerApp.src.Lib
{
    internal class Config
    {
        public string FileName { get; set; }

        public void Load()
        {
            var configFile = @"test.config";
            var exeFileMap = new ExeConfigurationFileMap { ExeConfigFilename = configFile };
            var config = ConfigurationManager.OpenMappedExeConfiguration(exeFileMap, ConfigurationUserLevel.None);


            try
            {
                FileName = config.AppSettings.Settings["test"].Value;
            }
            catch (System.Exception e)
            {
                FileName = "a";
            }
        }

        public void Save()
        {
            var configFile = @"test.config";
            var exeFileMap = new ExeConfigurationFileMap { ExeConfigFilename = configFile };
            var config = ConfigurationManager.OpenMappedExeConfiguration(exeFileMap, ConfigurationUserLevel.None);

            try
            {
                config.AppSettings.Settings["test"].Value = FileName;
            }
            catch (System.Exception e)
            {
            }
            //config.Save();
        }
    }
}
