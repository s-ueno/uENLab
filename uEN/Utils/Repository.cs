using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace uEN.Utils
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

        private static CompositionContainer container;
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
    }

    public interface IPriority { int Priority { get; } }



}
