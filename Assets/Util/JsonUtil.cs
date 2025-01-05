using Newtonsoft.Json;
using System.IO;
using UnityEngine;


/// <summary>
/// Utility class to serialize and deserialize object to and from JSON files.
/// </summary>
public static class JsonUtil
{
    private static string FilePath { get; set; }
    
    /// <summary>
    /// Serializes object to a JSON file.
    /// </summary>
    /// <typeparam name="T">The object type.</typeparam>
    /// <param name="obj">The object.</param>
    /// <param name="filePath">The filepath.</param>
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
    }

    /// <summary>
    /// Deserializes an object from a JSON file.
    /// </summary>
    /// <typeparam name="T">The object type.</typeparam>
    /// <param name="filePath">The filepath.</param>
    /// <returns>The deserialized object.</returns>
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

