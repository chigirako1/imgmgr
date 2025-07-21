using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.Design.AxImporter;

namespace PictureManagerApp.src.Lib
{
    public static class Util
    {
        public static string InputBox(string prompt, string title, string defVal)
        {
            string result;
            result = Microsoft.VisualBasic.Interaction.InputBox(prompt, title, defVal);
            return result;
        }

        public static void a()
        {
            
        }
    }
}
