using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace uEN.Extensions
{
    public static class ObjectExtensions
    {
        public static byte[] ToByteSerialize(this object obj)
        {
            if (obj == null) return null;
            using (var mem = new MemoryStream())
            {
                var ser = new BinaryFormatter();
                ser.Serialize(mem, obj);
                return mem.ToArray();
            }
        }

        public static object FromByteDeserialize(this byte[] arr)
        {
            if (arr == null) return null;
            using (MemoryStream st = new MemoryStream(arr))
            {
                var ser = new BinaryFormatter();
                return ser.Deserialize(st);
            }
        }

        public static byte[] ReadFully(this Stream stream)
        {
            if (stream is MemoryStream)
                return ((MemoryStream)stream).ToArray();     

            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
