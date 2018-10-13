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
        private IList<SubCommand> _subCommands;

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
            _subCommands = new List<SubCommand>();
            _subCommands.Add(new SubCommand("Test", () => { }));
        }

        /// <summary>
        /// Notifies the commands.
        /// </summary>
        private void NotifyCommands()
        {
            _eventAggregator.PublishOnUIThread(new RegisterCommandsArgs
            {
                SubCommands = _subCommands
            });
        }
    }
}
