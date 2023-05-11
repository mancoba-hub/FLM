using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FLM.LisoMbiza
{
    public static class ExportHelper
    {
        /// <summary>
        /// Exports the list to xml
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="entityList"></param>
        public static void ExportToXml<T>(string fileName, List<T> entityList)
        {
            FileInfo file = new FileInfo(fileName + ".xml");
            if (!file.Directory.Exists)
            {
                file.Directory.Create();
            }
            System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(List<T>));
            StreamWriter sw = file.CreateText();
            writer.Serialize(sw, entityList);            
            sw.Close();
        }

        /// <summary>
        /// Exports the list to json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <param name="entityList"></param>
        public static void ExportToJson<T>(string fileName, List<T> entityList)
        {
            string fileFullPath = fileName + ".json";
            var json = JsonConvert.SerializeObject(entityList);
            FileInfo file = new FileInfo(fileFullPath);
            if (!file.Directory.Exists)
            {
                file.Directory.Create();
            }

            File.WriteAllText(fileFullPath, json);
        }

        /// <summary>
        /// Exports the list to csv
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <param name="entityList"></param>
        public static void ExportToCsv<T>(string fileName, List<T> entityList)
        {
            string fileFullPath = fileName + ".csv";
            StringBuilder sbCsv = new StringBuilder();
            var header = "";
            var info = typeof(T).GetProperties().Reverse();
            foreach (var prop in info)
            {
                header += string.IsNullOrWhiteSpace(header) ? prop.Name : $",{prop.Name}";
            }
            sbCsv.AppendLine(header);

            foreach (var obj in entityList)
            {
                var line = "";
                foreach (var prop in info)
                {
                    string value = string.Empty;
                    
                    if (prop.PropertyType == typeof(bool))
                    {
                        value = prop.GetValue(obj, null).Equals(true) ? "Y" : "N";
                        line += string.IsNullOrWhiteSpace(line) ? prop.GetValue(obj, null) : $",{value}";
                    }
                    else
                    {
                        line += string.IsNullOrWhiteSpace(line) ? prop.GetValue(obj, null) : $",{prop.GetValue(obj, null)}";
                    }                    
                }
                sbCsv.AppendLine(line);
            }

            FileInfo file = new FileInfo(fileFullPath);
            if (!file.Directory.Exists)
            {
                file.Directory.Create();
            }

            File.WriteAllText(fileFullPath, sbCsv.ToString());
        }
    }
}
