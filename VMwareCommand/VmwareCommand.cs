using Entities.Arguments;
using Entities.Contracts;
using Entities.Extentions;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMwareCommand
{
    public class VmwareCommand : ICommandBuilder
    {
        /// <summary>
        /// Gets or sets the event aggregator.
        /// </summary>
        /// <value>
        /// The event aggregator.
        /// </value>
        public IEventAggregator EventAggregator { get; set; }

        /// <summary>
        /// Gets or sets the name of the module.
        /// </summary>
        /// <value>
        /// The name of the module.
        /// </value>
        /// 
        public string ModuleName { get; set; } = "Vmware";
        /// <summary>
        /// Gets or sets the sub commands.
        /// </summary>
        /// <value>
        /// The sub commands.
        /// </value>
        public IList<SubCommand> SubCommands { get; set; } = new List<SubCommand>();
        
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            RegisterCommands();
            NotifyCommands();
        }

        /// <summary>
        /// Registers the commands.
        /// </summary>
        private void RegisterCommands()
        {     
            SubCommands.Add(new SubCommand("t", "test",  arguments => Test(arguments)));
        }
        
        /// <summary>
        /// Notifies the commands.
        /// </summary>
        private void NotifyCommands()
        {
            EventAggregator.PublishOnUIThread(new RegisterCommandsArgs
            {
                Module = nameof(VMwareCommand),
                Command = this
            });
        }

        /// <summary>
        /// Tests the specified a.
        /// </summary>
        /// <param name="a">a.</param>
        public void Test(IArguments a)
        {

        }

    }
}
