using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Contracts
{
    public interface ICommandBuilder
    {
        /// <summary>
        /// Gets or sets the name of the module.
        /// </summary>
        /// <value>
        /// The name of the module.
        /// </value>
        string ModuleName { get; set; }

        /// <summary>
        /// Gets or sets the sub commands.
        /// </summary>
        /// <value>
        /// The sub commands.
        /// </value>
        IList<SubCommand> SubCommands { get; set; }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        void Initialize();
    }
}
