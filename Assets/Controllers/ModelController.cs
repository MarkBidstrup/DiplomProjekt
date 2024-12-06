using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


// Class is used to control model state.
public class ModelController
{
    private Model model;
    private static ModelController _instance;

    private ModelController() { }

    public static ModelController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ModelController();
            }
            return _instance;
        }
    }

    // Initializes model.
    public void InitializeModel(string modelName)
    {
        if (ModelExists(modelName))
        {
            model = JsonUtil.Deserialize<Model>(modelName);
        }
        else
        {
            model = new Model(Guid.NewGuid(), modelName, new List<Issue>());
            JsonUtil.Serialize(model, modelName);
        }
    }

    public void AddIssueToModel(Guid issueId, string subject, string dueDate, string assignedTo, string description, Vector3 spawnPosition, string modelName)
    {
        Issue issue = new Issue(issueId, spawnPosition, subject, dueDate, assignedTo, description);
        if (model.Issues != null)
        {
            model.AddIssue(issue);
        }
        JsonUtil.Serialize(model, modelName);
    }

    public void UpdateIssue(int index, string subject, string dueDate, string assignedTo, string description, string modelName)
    {
        model.UpdateIssue(index, subject, dueDate, assignedTo, description);
        JsonUtil.Serialize(model, modelName);
    }

    public void DeleteIssue(int index, string modelName)
    {
        model.DeleteIssue(index);
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
