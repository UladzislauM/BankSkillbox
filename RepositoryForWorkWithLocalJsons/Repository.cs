using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows;

namespace Bank
{
    public class Repository
    {
        /// <summary>
        /// Serialization json 
        /// </summary>
        public bool SaveData<T>(T @object, string address)
        {
            try
            {
                string directoryPath = Path.GetDirectoryName(address);

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                if (!File.Exists(address))
                {
                    string newFileName = "BankData_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".json";
                    string newFilePath = Path.Combine(Path.GetDirectoryName(address), newFileName);
                }

                using (StreamWriter writer = File.CreateText(address))
                {
                    string output = JsonConvert.SerializeObject(@object);
                    writer.Write(output);
                    return true;
                }
            }
            catch
            {
                throw new Exception("Repository exeption");
            }
        }

        /// <summary>
        /// Загрузка коллекции из файла в Общем виде
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T LoadDataAll<T>(string address)
        {
            try
            {

                if (!File.Exists(address))
                {
                    throw new Exception("The file doesn't exist, create a new one, or specify the path to another one");
                }

                using (StreamReader reader = File.OpenText(address))
                {
                    var fileText = reader.ReadToEnd();
                    return JsonConvert.DeserializeObject<T>(fileText);
                }
            }
            catch
            {
                throw new Exception("Repository exception");
            }
        }

    }
}