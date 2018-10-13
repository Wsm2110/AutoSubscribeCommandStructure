using Autofac;
using Autofac.Builder;
using Entities.Arguments;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Extentions
{
    public static class ContainerBuilderExtentions
    {

        /// <summary>
        /// Registers the name of the instance by type.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="name">The name.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException">
        /// type
        /// or
        /// instance
        /// </exception>
        public static IRegistrationBuilder<dynamic, SimpleActivatorData, SingleRegistrationStyle> RegisterInstanceByTypeName(this ContainerBuilder builder, string name, object[] args = null)
        {
            var type = GetModules().SelectMany(item => item.DefinedTypes).FirstOrDefault(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (type == null)
            {
                throw new NullReferenceException(nameof(type));
            }

            var instance = Activator.CreateInstance(type, args);

            if (instance == null)
            {
                throw new NullReferenceException(nameof(instance));
            }

            return builder.RegisterInstance(instance);
        }



        /// <summary>
        /// Traversals the specified dir.
        /// </summary>
        /// <param name="dir">The dir.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        /// <exception cref="System.IO.DirectoryNotFoundException">Unable to found module directory</exception>
        public static DirectoryInfo Traversal(DirectoryInfo dir, int count)
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
        /// Gets the modules.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Assembly> GetModules()
        {
            var dir = new DirectoryInfo(Environment.CurrentDirectory);
            var moduleDir = Traversal(dir.Parent, 0);

            var result = new List<Assembly>();

            moduleDir.GetFiles("*.dll").Apply(item => result.Add(Assembly.LoadFrom(item.FullName)));

            return result;
        }

    }
}
