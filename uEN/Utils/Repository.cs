using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace uEN
{
    public static class Repository
    {
        private static readonly List<ComposablePartCatalog> catalogList = new List<ComposablePartCatalog>();
        static Repository()
        {
            var assemblies = ConfigurationManager.GetSection("Repository.AssemblyCatalog") as NameValueCollection;
            foreach (var each in assemblies.AllKeys)
            {
                var assembly = LoadAssembly(each);
                if (assembly != null)
                {
                    catalogList.Add(new AssemblyCatalog(assembly));
                }
            }

            var types = ConfigurationManager.GetSection("Repository.TypeCatalog") as NameValueCollection;
            foreach (var each in types.AllKeys)
            {
                var type = LoadType(each);
                if (type != null)
                {
                    catalogList.Add(new TypeCatalog(type));
                }
            }

            var catalog = new AggregateCatalog();
            foreach (var each in catalogList)
            {
                catalog.Catalogs.Add(each);
            }
            container = new CompositionContainer(catalog);
        }
        private static Assembly LoadAssembly(string s)
        {
            Assembly assembly = null;
            try
            {
                assembly = Assembly.Load(s);
            }
            catch
            {
            }
            return assembly;
        }
        private static Type LoadType(string s)
        {
            Type type = null;
            try
            {
                type = Type.GetType(s);
            }
            catch
            {
            }
            return type;
        }

        public static CompositionContainer container;
        public static void Compose(this object obj)
        {
            container.ComposeParts(obj);
        }

        public static T GetPriorityExport<T>()
        {
            var list = container.GetExports<T, IPriority>();
            var mostPriority = list.OrderBy(x => x.Metadata.Priority)
                                   .FirstOrDefault();
            return mostPriority != null ? mostPriority.Value : default(T);
        }
        public const string Priority = "Priority";

        public static object GetPriorityExport(string typeName)
        {
            return GetPriorityExport(Type.GetType(typeName));
        }
        public static object GetPriorityExport(Type type)
        {
            var mostPriority = container.GetExports(type, typeof(IPriority), null)
                                .OrderBy(x => ((IPriority)x.Metadata).Priority)
                                .FirstOrDefault();
            return mostPriority != null ? mostPriority.Value : null;
        }


        public const string Context = "Context";
        public static T GetContextExport<T>(Func<string, bool> predicate)
        {
            var ret = container.GetExports<T, IContext>()
                               .FirstOrDefault(x => predicate(x.Metadata.Context));
            return ret == null ? default(T) : ret.Value;
        }

        public static IEnumerable<Lazy<T>> GetExports<T>()
        {
            return container.GetExports<T>();
        }

    }

    public interface IPriority { int Priority { get; } }
    public interface IContext { string Context { get; } }


}
