using System;
using System.Collections.Generic;
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
    public class LocalStorage<T>
    {
        private IsolatedStorageFile appStore = IsolatedStorageFile.GetUserStoreForAssembly();
        public LocalStorage()
        {
            if (!appStore.DirectoryExists(typeof(T).FullName))
            {
                appStore.CreateDirectory(typeof(T).FullName);
            }
        }

        public void Save(object value, [CallerMemberName] string key = null)
        {
            using (var stream = new IsolatedStorageFileStream(Path.Combine(typeof(T).FullName, key), FileMode.OpenOrCreate, appStore))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, value);
            }
        }
        public object Load([CallerMemberName] string key = null)
        {
            object result = null;
            try
            {
                using (var stream = new IsolatedStorageFileStream(Path.Combine(typeof(T).FullName, key), FileMode.OpenOrCreate, appStore))
                {
                    var formatter = new BinaryFormatter();
                    result = formatter.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
    }
}
