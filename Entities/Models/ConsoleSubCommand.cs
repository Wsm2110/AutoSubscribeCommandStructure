using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class ConsoleSubCommand : SubCommand
    {
        /// <summary>
        /// Gets or sets the module.
        /// </summary>
        /// <value>
        /// The module.
        /// </value>
        public string Module { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleSubCommand"/> class.
        /// </summary>
        /// <param name="shortName"></param>
        /// <param name="longName"></param>
        /// <param name="action">The action.</param>
        public ConsoleSubCommand(string module, string shortName, string longName, Action action) : base(shortName, longName, action)
        {
            this.Module = module;
        }
    }
}
