using Autofac;
using Entities.Arguments;
using Entities.Contracts;
using Entities.Extentions;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestRunnerArchitecture
{
    public class Program : IHandle<RegisterCommandsArgs>
    {
        private static IContainer _container;
        private static IList<SubCommand> subCommand;

        static void Main(string[] args)
        {
            subCommand = new List<SubCommand>();

            IoCInitializer();

            Console.ReadKey(true);
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
                   .SingleInstance()
                   .Named("c1", typeof(ICommandBuilder))
                   .OnActivated(args => (args.Instance as ICommandBuilder).Initialize())
                   .AutoActivate();

            builder.RegisterInstanceByTypeName("InfraCommand", new object[] { eventAggregator })
                   .SingleInstance()
                   .Named("c2", typeof(ICommandBuilder))
                   .OnActivated(args => (args.Instance as ICommandBuilder).Initialize())
                   .AutoActivate();

            _container = builder.Build();
        }

        /// <summary>
        /// Handles the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle(RegisterCommandsArgs message)
        {
            message.SubCommands.Apply(item => subCommand.Add(item));
        }
    }
}
