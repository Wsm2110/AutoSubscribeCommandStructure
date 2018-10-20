using System;
using Entities.Arguments;
using Entities.Contracts;
using System.Collections.Generic;
using Entities.Models;
using Entities.Extentions;
using System.Linq;

namespace TestRunnerArchitecture
{
    public class CommandEntryPoint : ICommandEntryPoint, IHandle<RegisterCommandsArgs>
    {
        private List<ICommandBuilder> _commands = new List<ICommandBuilder>();
        private SubCommand _subCommand;
        private readonly IEventAggregator _eventAggregator;
        private ConsoleError _error;
        private string _args;
        private ICommandBuilder _module;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandEntryPoint"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        public CommandEntryPoint(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
        }

        public void Handle(RegisterCommandsArgs message)
        {
            _commands.Add(message.Command);
        }

        /// <summary>
        /// Adds the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">command</exception>
        public ICommandEntryPoint Add(ICommandBuilder command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            _commands.Add(command);

            return this;
        }

        /// <summary>
        /// Setups the specified commands.
        /// </summary>
        /// <param name="commands">The commands.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">commands</exception>
        public ICommandEntryPoint Setup(IEnumerable<ICommandBuilder> commands)
        {
            if (commands == null)
            {
                throw new ArgumentNullException(nameof(commands));
            }

            _commands.AddRange(commands);

            return this;
        }

        /// <summary>
        /// Finds the command.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public ICommandEntryPoint FindCommand(string args)
        {
            _args = args;
            _module = _commands.FirstOrDefault(item => item.ModuleName.Equals(args.GetModule(), StringComparison.OrdinalIgnoreCase));

            if (_module == null)
            {
                _error = ConsoleError.ModuleNotFound;
                return this;
            }

            var subCommand = _module.SubCommands.FirstOrDefault(item => item.ShortName.Equals(args.GetSubCommand(), StringComparison.OrdinalIgnoreCase) ||
                                                           item.LongName.Equals(args.GetSubCommand(), StringComparison.OrdinalIgnoreCase));

            if (subCommand == null)
            {
                _error = ConsoleError.SubCommandNotFound;
                return this;
            }

            _subCommand = subCommand;

            return this;
        }

        /// <summary>
        /// Parses the arguments.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ICommandEntryPoint ParseArguments<T>()
        {
            if (_error == ConsoleError.Default)
            {
                var x = default(T);

            }

            return this;
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        /// <returns></returns>
        public ICommandEntryPoint Execute()
        {
            if (_error == ConsoleError.Default)
            {
                if (_subCommand != null)
                {
                    _subCommand.Command();
                }
                else
                {
                    _error = ConsoleError.SubCommandNotFound;
                }
            }
            return this;
        }

        /// <summary>
        /// Called when [error].
        /// </summary>
        /// <param name="action">The action.</param>
        public void OnError(Action<ConsoleError, string> action)
        {
            if (_error != ConsoleError.Default)
            {
                action(_error, _module?.ModuleName);
                _error = ConsoleError.Default;
            }
        }

        /// <summary>
        /// Gets the modules.
        /// </summary>
        /// <returns></returns>
        public string[] GetModules()
        {
            return _commands.Select(item => item.ModuleName).ToArray();
        }
    }
}