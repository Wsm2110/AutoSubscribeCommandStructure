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
        private readonly IEventAggregator _eventAggregator;

        public string ModuleName { get; set; } = "Infra";
        public IList<SubCommand> SubCommands { get; set; } = new List<SubCommand>();

        /// <summary>
        /// Initializes a new instance of the <see cref="VmwareCommand"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        public InfraCommand(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        /// <summary>
        /// Registers the commands.
        /// </summary>
        private void RegisterCommands()
        {
            SubCommands = new List<SubCommand>
            {
                new SubCommand("p", "prime", () => { Console.WriteLine("call from infra module"); }),
                  new SubCommand("l", "load", () => { Console.WriteLine("call from infra module"); }),
                    new SubCommand("a", "add", () => { Console.WriteLine("call from infra module"); }),
                      new SubCommand("p", "remove", () => { Console.WriteLine("call from infra module"); }),
                        new SubCommand("sd", "selfdestruct", () => { Console.WriteLine("call from infra module"); })
            };
        }

        /// <summary>
        /// Notifies the commands.
        /// </summary>
        private void NotifyCommands()
        {
            _eventAggregator.PublishOnUIThread(new RegisterCommandsArgs
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
