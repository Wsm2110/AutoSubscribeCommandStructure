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
        private readonly IEventAggregator _eventAggregator;

        public string ModuleName { get; set; } = "Vmware";
        public IList<SubCommand> SubCommands { get; set; } = new List<SubCommand>();

        /// <summary>
        /// Initializes a new instance of the <see cref="VmwareCommand"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        public VmwareCommand(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;      
        }

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
            SubCommands.Add(new SubCommand("t", "test", () => { Console.WriteLine("call from vmware command"); }));
        }

        /// <summary>
        /// Notifies the commands.
        /// </summary>
        private void NotifyCommands()
        {
            _eventAggregator.PublishOnUIThread(new RegisterCommandsArgs
            {
                Module = nameof(VMwareCommand),
                Command = this
            });
        }
    }
}
