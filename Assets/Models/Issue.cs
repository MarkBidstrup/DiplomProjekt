using UnityEngine;
using System;


/// <summary>
/// Represents an issue with associated data.
/// </summary>
public class Issue
{
    public Guid IssueId { get; }
    public Vector3 Location {  get; }
    public string Subject { get; set; }
    public string DueDate { get; set; }
    public string AssignedTo { get; set; }
    public string Description { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Issue"/> class.
    /// </summary>
    /// <param name="issueId">The issue Guid.</param>
    /// <param name="location">The issue location.</param>
    /// <param name="subject">The issue subject.</param>
    /// <param name="dueDate">The issue due date.</param>
    /// <param name="assignedTo">The issue assignee.</param>
    /// <param name="description">The issue description.</param>
    public Issue(Guid issueId, Vector3 location, string subject, string dueDate, string assignedTo, string description) 
    {
        IssueId = issueId; 
        Location = location;
        Subject = subject;
        DueDate = dueDate;
        AssignedTo = assignedTo;
        Description = description;
    }
}

