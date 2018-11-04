using System;
using Entities.Arguments;
using Entities.Contracts;
using System.Collections.Generic;
using Entities.Models;
using Entities.Extentions;
using System.Linq;
using System.Reflection;
using System.Linq.Expressions;

namespace TestRunnerArchitecture
{
    public class CommandEntryPoint : ICommandEntryPoint, IHandle<RegisterCommandsArgs>
    {
        #region fields

        private List<ICommandBuilder> _commands = new List<ICommandBuilder>();
        private SubCommand _subCommand;
        private readonly IEventAggregator _eventAggregator;
        private ConsoleError _error;
        private string _args;
        private ICommandBuilder _module;

        #endregion

        #region Properties

        public IEventAggregator EventAggregator { get; set; }

        #endregion

        public void Handle(RegisterCommandsArgs message)
        {
            _commands.Add(message.Command);
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
        public ICommandEntryPoint ParseArguments()
        {
            var xx = _subCommand.Command.GetMethodInfo();

            var b = xx.GetParameters();


            return this;
        }

        IEnumerable<Type> GetNestedDelegates(Type type)
        {
            return type.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic)
                       .Where(t => t.BaseType == typeof(MulticastDelegate));
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

                    _subCommand.Command(null);
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