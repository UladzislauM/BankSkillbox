using Newtonsoft.Json;
using System;
using System.IO;

namespace Bank
{
    /// <summary>
    /// Repository for working with jsons.
    /// </summary>
    public class RepositoryForJson
    {
        /// <summary>
        /// Json serialization
        /// </summary>
        public bool SaveDataToJson<T>(T @object, string address)
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
                    string newFileName = "BankData_" + DateTime.Now.ToString("yyyy-MM-dd_HH.mm") + ".json";
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
        /// Json deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T LoadAllDataFromJson<T>(string address)
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
