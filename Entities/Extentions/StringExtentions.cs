using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Extentions
{
   public static class StringExtentions
    {

        /// <summary>
        /// Gets the module.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string GetModule(this string input)
        {
            var mod = input.Split(' ').ToList();

            if (!mod.Any())
            {
                return string.Empty;
            }

            //logic
            return mod[0];
        }



        /// <summary>
        /// Gets the module.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string GetSubCommand(this string input)
        {
            var mod = input.Split(' ').ToList();

            if (mod.Count < 2)
            {         
                return string.Empty;
            }

            //logic
            return mod[1];
        }


    }
}
