using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGEA
{
    public static class Conversao
    {
        public static int ToInt(this string s) => Convert.ToInt32(s);

        public static double ToDouble(this string s) => Convert.ToDouble(s);

    }
}
