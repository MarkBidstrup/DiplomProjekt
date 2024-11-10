using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using UnityEngine;


public static class JsonUtil
{
    public static string FilePath {  get; private set; }
    
    public static void Serialize<T>(T obj, string filePath)
    {
        FilePath = Path.Combine(Application.dataPath, "Json/" + filePath);

        var settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented
        };

        string jsonString = JsonConvert.SerializeObject(obj, settings);

        string folderPath = Path.GetDirectoryName(FilePath);
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        File.WriteAllText(FilePath, jsonString);
        Debug.Log("Data serialized and saved to: " + FilePath);
    }

    public static T Deserialize<T>(string filePath)
    {
        FilePath = Path.Combine(Application.dataPath, "Json/" + filePath);

        if (File.Exists(FilePath))
        {
            string jsonString = File.ReadAllText(FilePath);
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
        else
        {
            Debug.LogError("File not found at: " + FilePath);
            return default;
        }
    }
}

