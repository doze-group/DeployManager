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
        static string FILE_NAME = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "DeployManager\\Data") +"\\Cache.data";
        
        //ProgramData/DeployManager

        public static void c_folder()
        {
            // Specify the directory you want to manipulate.
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var path = Path.Combine(appData, "DeployManager\\Data");
            Console.WriteLine(FILE_NAME);
            try
            {
                // Determine whether the directory exists.
                if (Directory.Exists(path))
                {
                    Console.WriteLine("That path exists already.");
                    return;
                }

                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(path);
                Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(path));

                // Delete the directory.
                //di.Delete();
                //Console.WriteLine("The directory was deleted successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
            finally { }
        }

        public static void Serialize<T>(this List<T> obj)
        {
            c_folder();
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
