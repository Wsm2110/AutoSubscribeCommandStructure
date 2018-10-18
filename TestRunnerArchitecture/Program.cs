using Autofac;
using CommandLine;
using Entities.Arguments;
using Entities.Contracts;
using Entities.Extentions;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Console = Colorful.Console;

namespace TestRunnerArchitecture
{


    public class Program : IHandle<RegisterCommandsArgs>
    {
        private static IContainer _container;
        private static IList<ConsoleSubCommand> ConsoleSubCommands;

        static void Main(string[] args)
        {
            Console.BackgroundColor = Color.Purple;
            Console.Clear();

            ConsoleSubCommands = new List<ConsoleSubCommand>();

            IoCInitializer();

            PrintModules();

            while (true)
            {
                //cfg hostfile
                var input = Console.ReadLine();
                var module = input.GetModule();
                var subCommand = input.GetSubCommand();

                var result = ConsoleSubCommands.Where(item => item.Module.Equals(module, StringComparison.OrdinalIgnoreCase))
                                               .Where(item => (item.LongName.Equals(subCommand, StringComparison.OrdinalIgnoreCase) || item.ShortName.Equals(subCommand, StringComparison.OrdinalIgnoreCase)));

                //TODO Create CommandParser wich delegates to proper subcommands

                result.FirstOrDefault().Command();

            }
        }

        private static void PrintModules()
        {
            Console.WriteLine("Modules loaded:");
            Console.WriteLine(string.Join("\n", ConsoleSubCommands.Select(item => item.Module).Distinct()));
        }

        /// <summary>
        /// Ioes the c initializer.
        /// </summary>
        private static void IoCInitializer()
        {
            var entry = Assembly.GetExecutingAssembly();
            var builder = new ContainerBuilder();

            // Scan an assembly for components
            builder.RegisterAssemblyTypes(entry)
                   .AsImplementedInterfaces();

            // Scan modules folder
            ContainerBuilderExtentions.GetModules().Apply(item => builder.RegisterAssemblyTypes(item)
                                              .AsImplementedInterfaces());

            // create instance of eventaggregator
            var eventAggregator = new EventAggregator();

            //register eventaggregator
            builder.RegisterInstance(eventAggregator)
                   .As<IEventAggregator>();

            //Inorder for the eventaggregator to work we have to subscribe before commands are activated
            eventAggregator.Subscribe(new Program());

            //resolve modules dynamically without using project references
            builder.RegisterInstanceByTypeName("VmwareCommand", new object[] { eventAggregator })
                   .Named("c1", typeof(ICommandBuilder))
                   .OnActivated(args => (args.Instance as ICommandBuilder).Initialize())
                   .SingleInstance()
                   .AutoActivate();

            builder.RegisterInstanceByTypeName("InfraCommand", new object[] { eventAggregator })
                   .Named("c2", typeof(ICommandBuilder))
                   .OnActivated(args => (args.Instance as ICommandBuilder).Initialize())
                   .SingleInstance()
                   .AutoActivate();

            _container = builder.Build();
        }

        /// <summary>
        /// Handles the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle(RegisterCommandsArgs message)
        {
            message.SubCommands.Apply(item => ConsoleSubCommands.Add(new ConsoleSubCommand(message.Module, item.ShortName, item.LongName, item.Command)));
        }
    }
}
