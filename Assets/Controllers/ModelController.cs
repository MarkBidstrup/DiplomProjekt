using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dummiesman;
using UnityEngine.VFX;
using System.IO;
using System.Xml.Linq;
using System;
using Newtonsoft.Json;

public class ModelController : MonoBehaviour
{
    private Model model;
    private List<Flag> flagList;

    void Start()
    {
        
    }

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

    public void AddFlagToModel(string subject, string dueDate, string assignedTo, string description, Vector3 spawnPosition, string modelName)
    {
        Flag flag = new Flag(Guid.NewGuid(), spawnPosition, subject, dueDate, assignedTo, description);
        if (flagList != null)
        {
            flagList.Add(flag);
        }
        JsonUtil.Serialize(model, modelName);
    }

    public Model GetModel()
    {
        return model;
    }
}
