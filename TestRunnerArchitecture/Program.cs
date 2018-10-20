using Autofac;
using CommandLine;
using Entities.Arguments;
using Entities.Contracts;
using Entities.Extentions;
using System;
using System.Drawing;
using System.Reflection;
using Console = Colorful.Console;

namespace TestRunnerArchitecture
{


    public class Program
    {
        private static IContainer _container;
        private static ICommandEntryPoint commandEntryPoint;
        private static IEventAggregator eventAggregator;

        static void Main(string[] args)
        {
            Console.BackgroundColor = Color.Purple;
            Console.Clear();

            eventAggregator = new EventAggregator();
            commandEntryPoint = new CommandEntryPoint(eventAggregator);

            IoCInitializer();

            PrintModules();

            while (true)
            {
                commandEntryPoint.FindCommand(Console.ReadLine())
                                 .ParseArguments<Type>()
                                 .Execute()
                                 .OnError((e, module) =>  Console.WriteLine($"{module} [{e}]"));
            }
        }

        private static void PrintModules()
        {
            Console.WriteLine("Modules loaded:");
            Console.WriteLine(string.Join("\n", commandEntryPoint.GetModules()));
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

            //register eventaggregator
            builder.RegisterInstance(eventAggregator)
                   .As<IEventAggregator>();

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
    }
}
