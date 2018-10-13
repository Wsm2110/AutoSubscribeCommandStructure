using Autofac;
using Entities.Arguments;
using Entities.Contracts;
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
            GetModules().Apply(item => builder.RegisterAssemblyTypes(item)
                                              .AsImplementedInterfaces());

            var eventAggregator = new EventAggregator();

            builder.RegisterInstance(eventAggregator)
                   .As<IEventAggregator>();
       
            //Inorder for the eventaggregator to work we have to subscribe before commands are activated
            eventAggregator.Subscribe(new Program());

            builder.RegisterType<VMwareCommand.VmwareCommand>()                             
                   .SingleInstance()
                   .Named("c1", typeof(ICommandBuilder))
                   .OnActivated(args => args.Instance.Initialize()) 
                   .AutoActivate();
            
            builder.RegisterType<InfraModule.InfraCommand>()                            
                   .SingleInstance()
                   .Named("c2", typeof(ICommandBuilder))
                   .OnActivated(args => args.Instance.Initialize())
                   .AutoActivate();

            _container = builder.Build();
        }

        /// <summary>
        /// Gets the modules.
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<Assembly> GetModules()
        {
            var dir = new DirectoryInfo(Environment.CurrentDirectory);
            var moduleDir = Traversal(dir.Parent, 0);

            var result = new List<Assembly>();

            moduleDir.GetFiles("*.dll").Apply(item => result.Add(Assembly.LoadFrom(item.FullName)));

            return result;
        }

        /// <summary>
        /// Traversals the specified dir.
        /// </summary>
        /// <param name="dir">The dir.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        /// <exception cref="System.IO.DirectoryNotFoundException">Unable to found module directory</exception>
        private static DirectoryInfo Traversal(DirectoryInfo dir, int count)
        {
            if (count == 5)
            {
                throw new DirectoryNotFoundException("Unable to find [module] directory");
            }

            var module = dir.Parent.GetDirectories().FirstOrDefault(d => d.Name.Equals("Modules", StringComparison.OrdinalIgnoreCase));
            if (module != null)
            {
                return module;
            }

            count++;

            return Traversal(dir.Parent, count);
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
