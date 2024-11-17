using System;
using System.Collections.Generic;


// Holds model data.
public class Model
{
    public Guid ModelId { get; }
    public string ModelName { get; }
    public List<Issue> Issues { get; }

    public Model(Guid modelId, string modelName, List<Issue> issues) 
    {
        ModelId = modelId;
        ModelName = modelName;
        Issues = issues;
    }

    public void AddIssue(Issue issue)
    {
        Issues.Add(issue);
    }

    public void UpdateIssue(int index, string subject, string dueDate, string assignedTo, string description)
    {
        Issues[index].Subject = subject;
        Issues[index].DueDate = dueDate;
        Issues[index].AssignedTo = assignedTo;
        Issues[index].Description = description;
    }

    public void DeleteIssue(int index)
    {
        Issues.RemoveAt(index);
    }
}

