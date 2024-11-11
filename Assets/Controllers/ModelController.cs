using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


// Class is used to control model.
public class ModelController : MonoBehaviour
{
    private Model model;
    private List<Flag> flagList;

    // Initializes model.
    public void InitializeModel(string modelName)
    {
        if (ModelExists(modelName))
        {
            model = JsonUtil.Deserialize<Model>(modelName);
            flagList = model.Flags;
        }
        else
        {
            flagList = new List<Flag>();
            model = new Model(Guid.NewGuid(), modelName, flagList);
            JsonUtil.Serialize(model, modelName);
        }
    }

    // Adds a flag to the model and serializes model data.
    public void AddFlagToModel(string subject, string dueDate, string assignedTo, string description, Vector3 spawnPosition, string modelName)
    {
        Flag flag = new Flag(Guid.NewGuid(), spawnPosition, subject, dueDate, assignedTo, description);
        if (flagList != null)
        {
            flagList.Add(flag);
        }
        JsonUtil.Serialize(model, modelName);
    }

    // Returns model.
    public Model GetModel()
    {
        return model;
    }

    // Checks if the model exists in the file path.
    private bool ModelExists(string modelName)
    {
        string filePath = Path.Combine(Application.dataPath, "Json/" + modelName);
        if (File.Exists(filePath))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
