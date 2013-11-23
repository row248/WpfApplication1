using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFS.Helpers
{
    public class Helper
    {
        public static int? ToNullabelInt32(string s)
        {
            int i;
            return Int32.TryParse(s, out i) ? (int?)i : null;
        }

        /// <summary>
        /// Warning: Only valid string -> int!
        /// </summary>
        public static int ToInt32(string s)
        {
            int i;
            Int32.TryParse(s, out i);
            return i;
        }
    }
}
