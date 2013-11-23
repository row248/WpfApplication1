using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFS.Helpers
{
    public class UnixDate
    {
        public static int Now
        {
            get
            {
                return (int)DateTimeToUnixTimestamp(DateTime.Now);
            }
        }

        #region Privates

        private static double DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return (dateTime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
        }

        #endregion
    }
}
