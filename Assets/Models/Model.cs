using System;
using System.Collections.Generic;


/// <summary>
/// Represents a model with associated data.
/// </summary>
public class Model
{
    public Guid ModelId { get; } // Currently not used for anything, but should be implemented to reference the correct model instead of modelName in case of duplicate model names.
    public string ModelName { get; }
    public List<Issue> Issues { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Model"/> class.
    /// </summary>
    /// <param name="modelId">The model Guid.</param>
    /// <param name="modelName">The model name.</param>
    /// <param name="issues">The issue list.</param>
    public Model(Guid modelId, string modelName, List<Issue> issues) 
    {
        ModelId = modelId;
        ModelName = modelName;
        Issues = issues;
    }

    /// <summary>
    /// Adds issue to the issue list.
    /// </summary>
    /// <param name="issue">The issue.</param>
    public void AddIssue(Issue issue)
    {
        Issues.Add(issue);
    }

    /// <summary>
    /// Updates issue in the issue list by index.
    /// </summary>
    /// <param name="index">The issue index.</param>
    /// <param name="subject">The issue subject.</param>
    /// <param name="dueDate">The issue due date.</param>
    /// <param name="assignedTo">The issue assignee.</param>
    /// <param name="description">The issue description.</param>
    public void UpdateIssue(int index, string subject, string dueDate, string assignedTo, string description)
    {
        Issues[index].Subject = subject;
        Issues[index].DueDate = dueDate;
        Issues[index].AssignedTo = assignedTo;
        Issues[index].Description = description;
    }

    /// <summary>
    /// Deletes an issue by index in the models list of issues.
    /// </summary>
    /// <param name="index">The issue index.</param>
    public void DeleteIssue(int index)
    {
        Issues.RemoveAt(index);
    }
}

