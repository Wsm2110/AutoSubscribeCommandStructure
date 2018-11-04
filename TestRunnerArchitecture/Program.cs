using Autofac;
using CommandLine;
using Entities.Arguments;
using Entities.Contracts;
using Entities.Extentions;
using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using Console = Colorful.Console;

namespace TestRunnerArchitecture
{


    public class Program
    {
        private static IContainer _container;

        static void Main(string[] args)
        {
            Console.BackgroundColor = Color.Purple;
            Console.Clear();

            int DA = 244;
            int V = 212;
            int ID = 255;

            Console.WriteAscii("TestRunner", Color.FromArgb(DA, V, ID));

            Console.WriteLine("Version 1.9.2.1 rc1");
            Console.WriteLine('\n');

            Console.Write(" Usage: ", Color.Yellow);
            Console.Write("<command> <subcommand> [<arguments>]", Color.White);

            IoCInitializer();

            PrintModules();


            while (true)
            {
                _container.Resolve<ICommandEntryPoint>().FindCommand(Console.ReadLine())
                                 .ParseArguments()
                                 .Execute()
                                 .OnError((e, module) => Console.WriteLine($"{module} [{e}]"));
            }
        }

        private static void PrintModules()
        {
            Console.WriteLine("Modules loaded:");
            //   Console.WriteLine(string.Join("\n", _container.Resolve<ICommandEntryPoint>().GetModules()));
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

            //register eventaggregator
            builder.RegisterType<EventAggregator>()
                   .As<IEventAggregator>()
                   .SingleInstance()
                   .AutoActivate();


            // Scan modules folder
            var plugins = ModuleExtentions.GetModules();

            //register external assemblies
            builder.RegisterAssemblyTypes(plugins.ToArray()).Where(t => t.Name.EndsWith("Command")).As<ICommandBuilder>()
                                                            .PropertiesAutowired()
                                                            .OnActivated(args =>
                                                            {
                                                                var command = (args.Instance as ICommandBuilder);
                                                                if (command != null)
                                                                {
                                                                    command.Initialize();
                                                                    return;
                                                                }

                                                                throw new NullReferenceException($"Unable to cast object to {nameof(ICommandBuilder)}");
                                                            })
                                                            .AutoActivate()
                                                            .SingleInstance();

            //should always be last 
            builder.RegisterType<CommandEntryPoint>().As<ICommandEntryPoint>()
                                                     .OnActivated(item =>
                                                     {
                                                         item.Instance.EventAggregator.Subscribe(item.Instance);
                                                     })
                                                     .PropertiesAutowired()
                                                     .SingleInstance()
                                                     .AutoActivate();

            _container = builder.Build();
        }
    }
}
