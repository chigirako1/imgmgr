using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PictureManagerApp.src.Lib
{
    public static class Log
    {
        public static void log(string s)
        {
            //Console.WriteLine("[LOG]" + s);
            //System.Diagnostics.Debug.WriteLine("[LOG]  " + s);
            puts("[LOG]  " + s);

            //MethodBase.GetCurrentMethod().Name
        }

        public static void trc(string s,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = -1)
        {
            //Console.WriteLine(s);
            //System.Diagnostics.Debug.WriteLine($"[TRC] {memberName}:{s}");
            puts($"[TRC] {memberName}:{s}");
        }

        public static void warning(string s)
        {
            Console.Error.WriteLine(s);
        }

        public static void err(string s)
        {
            Console.Error.WriteLine(s);
        }

        public static void fatal(string s)
        {
            Console.Error.WriteLine(s);
        }

        public static void dbg(string s)
        {
#if DEBUG
            //Console.Error.WriteLine("[DBG]" + s);
            //System.Diagnostics.Debug.WriteLine("[DBG]" + s);
            puts("[DBG]" + s);
#endif
        }

        private static void puts(string str)
        {
            System.Diagnostics.Debug.WriteLine(str);
        }
    }
}
