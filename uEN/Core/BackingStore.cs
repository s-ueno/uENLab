using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace uEN.Core
{
    public static class BackingStore
    {
        public static void SetBackingStore<T>(this T obj, object value, [CallerMemberName] string key = null) where T : class
        {
            var appStore = CreateStore();
            var directoryPath = typeof(T).FullName;
            if (!appStore.DirectoryExists(directoryPath))
            {
                appStore.CreateDirectory(directoryPath);
            }

            if (value == null)
            {
                Trace.TraceInformation(string.Format("value is NULL. SetBackingStore -> {0}", Path.Combine(directoryPath, key)));
                return;
            }
            using (var stream = new IsolatedStorageFileStream(Path.Combine(directoryPath, key), FileMode.OpenOrCreate, appStore))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, value);
            }
        }
        public static object GetBackingStore<T>(this T obj, [CallerMemberName] string key = null) where T : class
        {
            var appStore = CreateStore();
            var directoryPath = typeof(T).FullName;
            if (!appStore.DirectoryExists(directoryPath))
            {
                appStore.CreateDirectory(directoryPath);
            }

            object result = null;
            try
            {
                var path = Path.Combine(directoryPath, key);
                using (var stream = new IsolatedStorageFileStream(path, FileMode.OpenOrCreate, appStore))
                {
                    if (stream.Length == 0)
                    {
                        Trace.TraceInformation(string.Format("value is Empty. GetBackingStore -> {0}", path));
                        return result;
                    }
                    var formatter = new BinaryFormatter();
                    result = formatter.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceInformation(ex.Message);
                //Trace.TraceError(ex.ToString());
            }
            return result;
        }
        public static void RemoveBackingStore<T>(this T obj) where T : class
        {
            var appStore = CreateStore();
            appStore.Remove();
        }
        private static IsolatedStorageFile CreateStore()
        {
            if (AppDomain.CurrentDomain.ActivationContext != null)
            {
                var asi = new ApplicationSecurityInfo(AppDomain.CurrentDomain.ActivationContext);
                var store = IsolatedStorageFile.GetStore(
                        IsolatedStorageScope.User |
                        IsolatedStorageScope.Assembly, null, null, asi.ApplicationEvidence, null);
                return store;
            }
            return IsolatedStorageFile.GetUserStoreForAssembly();
        }
    }
}
