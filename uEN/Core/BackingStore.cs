using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace uEN.Core
{
    public static class BackingStore
    {
        public static void SetBackingStore<T>(this T obj, object value, string key) where T : class
        {
            var appStore = IsolatedStorageFile.GetUserStoreForAssembly();
            var directoryPath = typeof(T).FullName;
            if (!appStore.DirectoryExists(directoryPath))
            {
                appStore.CreateDirectory(directoryPath);
            }

            using (var stream = new IsolatedStorageFileStream(Path.Combine(directoryPath, key), FileMode.OpenOrCreate, appStore))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, value);
            }
        }
        public static object GetBackingStore<T>(this T obj, string key) where T : class
        {
            var appStore = IsolatedStorageFile.GetUserStoreForAssembly();
            var directoryPath = typeof(T).FullName;
            if (!appStore.DirectoryExists(directoryPath))
            {
                appStore.CreateDirectory(directoryPath);
            }

            object result = null;
            try
            {
                using (var stream = new IsolatedStorageFileStream(Path.Combine(directoryPath, key), FileMode.OpenOrCreate, appStore))
                {
                    var formatter = new BinaryFormatter();
                    result = formatter.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
            }
            return result;
        }

        public static void RemoveBackingStore<T>(this T obj) where T : class
        {
            var appStore = IsolatedStorageFile.GetUserStoreForAssembly();
            appStore.Remove();
        }

    }
}
