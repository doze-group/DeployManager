using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace DeployManager
{
    public static class Helpers
    {
        const string FILE_NAME = "Cache.data";
        public static void Serialize<T>(this List<T> obj)
        {
            using (Stream stream = File.OpenWrite(FILE_NAME))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
            }
        }

        public static List<T> Deserialize<T>(this List<T> data)
        {
            using (Stream stream = File.OpenRead(FILE_NAME))
            {
                var formatter = new BinaryFormatter();
                return (List<T>)formatter.Deserialize(stream);
            }
        }
    }
}
