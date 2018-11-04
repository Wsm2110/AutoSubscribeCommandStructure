using Entities.Contracts;
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
        public Action<IArguments> Command { get; set; } 

        /// <summary>
        /// Gets or sets the long name.
        /// </summary>
        /// <value>
        /// The long name.
        /// </value>
        public string LongName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public String ShortName { get; set; } = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubCommand"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="action">The action.</param>
        public SubCommand(string shortName, string longName,  Action<IArguments> action)
        {
            ShortName = shortName;
            LongName = longName;
            Command = action;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubCommand{T}"/> class.
        /// </summary>
        /// <param name="longName">The long name.</param>
        /// <param name="action">The action.</param>
        public SubCommand(string longName, Action<IArguments> action)
        {
            LongName = LongName;
            Command = action;
        } 
    }
}
