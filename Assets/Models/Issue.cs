using UnityEngine;
using System;


// Holds issue data.
public class Issue
{
    public Guid IssueId { get; }
    public Vector3 Location {  get; }
    public string Subject { get; set; }
    public string DueDate { get; set; }
    public string AssignedTo { get; set; }
    public string Description { get; set; }

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

