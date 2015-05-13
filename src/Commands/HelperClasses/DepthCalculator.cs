using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rirbatch.Commands.HelperClasses
{
    class DepthCalculator
    {
        /// <summary>
        /// Tiefe (leading spaces) zählen um heraus zu finden wie die verschachtelung ist und was zusammen gehört
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static int countTiefe(String line)
        {
            return line.Length - line.TrimStart().Length;
        }
    }
}
