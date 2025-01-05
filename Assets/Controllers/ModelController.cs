using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

/// <summary>
/// Controls the model logic.
/// </summary>
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

    /// <summary>
    /// Initializes model.
    /// </summary>
    /// <param name="modelName">The model name.</param>
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

    /// <summary>
    /// Adds an issue to the model's list of issues.
    /// </summary>
    /// <param name="issueId">The issue Guid.</param>
    /// <param name="subject">The issue subject.</param>
    /// <param name="dueDate">The issue due date.</param>
    /// <param name="assignedTo">The issue assignee.</param>
    /// <param name="description">The issue description.</param>
    /// <param name="spawnPosition">The issue location.</param>
    /// <param name="modelName">The model name, which the issue is associated with.</param>
    public void AddIssueToModel(Guid issueId, string subject, string dueDate, string assignedTo, string description, Vector3 spawnPosition, string modelName)
    {
        Issue issue = new Issue(issueId, spawnPosition, subject, dueDate, assignedTo, description);
        if (model.Issues != null)
        {
            model.AddIssue(issue);
        }
        JsonUtil.Serialize(model, modelName);
    }

    /// <summary>
    /// Updates an issue by index in the models list of issues.
    /// </summary>
    /// <param name="index">The issue index.</param>
    /// <param name="subject">The issue subject.</param>
    /// <param name="dueDate">The issue due date.</param>
    /// <param name="assignedTo">The issue assignee.</param>
    /// <param name="description">The issue description.</param>
    /// <param name="modelName">The model name, which the issue is associated with.</param>
    public void UpdateIssue(int index, string subject, string dueDate, string assignedTo, string description, string modelName)
    {
        model.UpdateIssue(index, subject, dueDate, assignedTo, description);
        JsonUtil.Serialize(model, modelName);
    }

    /// <summary>
    /// Deletes an issue by index in the models list of issues.
    /// </summary>
    /// <param name="index">The issue index.</param>
    /// <param name="modelName">The model name, which the issue is associated with.</param>
    public void DeleteIssue(int index, string modelName)
    {
        model.DeleteIssue(index);
        JsonUtil.Serialize(model, modelName);
    }

    /// <summary>
    /// Model getter.
    /// </summary>
    public Model GetModel()
    {
        return model;
    }

    /// <summary>
    /// Checks if a model with the model name given exists in the json directory.
    /// </summary>
    /// <returns>True if the model exists, False if it does not exist.</returns>
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
