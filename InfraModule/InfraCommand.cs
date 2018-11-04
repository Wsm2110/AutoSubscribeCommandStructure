using Entities.Arguments;
using Entities.Contracts;
using Entities.Extentions;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraModule
{

    public class InfraCommand : ICommandBuilder
    {
        /// <summary>
        /// Gets or sets the name of the module.
        /// </summary>
        /// <value>
        /// The name of the module.
        /// </value>
        public string ModuleName { get; set; } = "Infra";
        /// <summary>
        /// Gets or sets the sub commands.
        /// </summary>
        /// <value>
        /// The sub commands.
        /// </value>
        public IList<SubCommand> SubCommands { get; set; } = new List<SubCommand>();
        /// <summary>
        /// Gets or sets the event aggregator.
        /// </summary>
        /// <value>
        /// The event aggregator.
        /// </value>
        public IEventAggregator EventAggregator { get; set; }
        
        /// <summary>
        /// Registers the commands.
        /// </summary>
        private void RegisterCommands()
        {
            SubCommands = new List<SubCommand>
            {
                new SubCommand("p", "prime", arguments => { Console.WriteLine("call from infra module"); }),
                new SubCommand("l", "load", arguments => { Console.WriteLine("call from infra module"); }),
                new SubCommand("a", "add", arguments => { Console.WriteLine("call from infra module"); }),
                new SubCommand("p", "remove", arguments => { Console.WriteLine("call from infra module"); }),
                new SubCommand("sd", "selfdestruct", arguments => { Console.WriteLine("call from infra module"); })
            };
        }

        /// <summary>
        /// Notifies the commands.
        /// </summary>
        private void NotifyCommands()
        {
            EventAggregator.PublishOnUIThread(new RegisterCommandsArgs
            {
                Module = "Infra",
                Command = this
            });
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            RegisterCommands();
            NotifyCommands();
        }
    }
}
