using UnityEngine;
using System;


internal class Flag
{
    public Guid FlagId { get; }
    public Vector3 Location {  get; }
    public string Subject { get; }
    public string DueDate { get; }
    public string AssignedTo { get; }
    public string Description { get; }

    public Flag(Guid flagId, Vector3 location, string subject, string dueDate, string assignedTo, string description) 
    {  
        FlagId = flagId; 
        Location = location;
        Subject = subject;
        DueDate = dueDate;
        AssignedTo = assignedTo;
        Description = description;
    }
}

