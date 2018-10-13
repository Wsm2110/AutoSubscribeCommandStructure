using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class SubCommand
    {
        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        /// <value>
        /// The command.
        /// </value>
        public Action Command { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public String Identifier { get; set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SubCommand"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="action">The action.</param>
        public SubCommand(string name, Action action)
        {
            Identifier = name;
            Command = action;
        }
    }
}
