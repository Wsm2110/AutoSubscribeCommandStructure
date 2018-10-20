using Entities.Contracts;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Arguments
{
    public class RegisterCommandsArgs
    {

        public string Module { get; set; }

        /// <summary>
        /// Gets or sets the sub commands.
        /// </summary>
        /// <value>
        /// The sub commands.
        /// </value>
       public ICommandBuilder Command { get; set; }
    }
}
