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
    public class InfraCommand: ICommandBuilder
    {
           
        private readonly IEventAggregator _eventAggregator;
        private IList<SubCommand> _subCommands;

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
            _subCommands = new List<SubCommand>
            {
                new SubCommand("Test1", () => { })
            };
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
