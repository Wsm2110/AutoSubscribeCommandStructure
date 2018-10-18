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
                Console.WriteLine("Unable to find any arguments, input is most likely empty");
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
                Console.WriteLine("Unable to find subcommand...");
                return string.Empty;
            }

            //logic
            return mod[1];
        }


    }
}
