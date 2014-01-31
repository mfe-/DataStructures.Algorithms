using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Get.Mathematics
{
    public partial class Mathematics
    {
        /// <summary>
        /// Greatest common divisor using the Euclidean algorithm
        /// http://en.wikipedia.org/wiki/Greatest_common_divisor
        /// http://en.wikipedia.org/wiki/Binary_GCD_algorithm
        /// http://en.wikipedia.org/wiki/Euclidean_algorithm
        /// </summary>
        /// <param name="a">Natural number</param>
        /// <param name="b">Natural number</param>
        /// <returns>Greatest common divisor of a and b</returns>
        public static int gcd(int a, int b)
        {
            int r = 0, q, x, y;

            if (a == b) return a;
            if (a == 0) return b;
            if (b == 0) return a;
            //change input
            if (a / b == 0)
            {
                x = a;
                y = b;
                a = y;
                b = x;
            }
            do
            {
                x = a; y = b;
                q = Convert.ToInt32(Math.Round(Convert.ToDecimal(a / b), 0));
                r = a - b * q;
                
                if (Debugger.IsAttached) Debug.WriteLine(a + " = " + b + " * " + q + " + " + r + " " + r + " < " + b);
                a = y;
                b = r;
            }
            while (b!=0);
            //a contains value of b
            return a == 0 ? 1 : a;
        }
        /// <summary>
        /// Greatest common divisor based on the Euclidean algorithm
        /// http://www.daniweb.com/software-development/csharp/code/217166/two-ways-to-implement-the-gcd
        /// </summary>
        /// <param name="x">Natural number</param>
        /// <param name="y">Natural number</param>
        /// <returns>Greatest common divisor of a and b</returns>
        public static int gcd2(int x, int y)
        {
            while (x != y)
            {
                if (x > y)
                {
                    x = x - y;
                }
                else
                {
                    y = y - x;
                }
            }
            return x;
        }
    }
}
