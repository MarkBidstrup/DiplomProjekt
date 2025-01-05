using UnityEngine;
using TMPro;
using System;


/// <summary>
/// EventPublisher associated with the create issue UI.
/// </summary>
public class CreateIssueEventPublisher : MonoBehaviour
{
    // Referenced in the unity prefab editor inspector
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private TMP_InputField subject;
    [SerializeField]
    private TMP_InputField dueDate;
    [SerializeField]
    private TMP_Dropdown assignedTo;
    [SerializeField]
    private TMP_InputField description;

    /// <summary>
    /// Delegate for handling create issue events.
    /// </summary>
    /// <param name="subject">The issue subject.</param>
    /// <param name="dueDate">The issue due date.</param>
    /// <param name="assignedTo">The issue assignee.</param>
    /// <param name="description">The issue description.</param>
    public delegate void OnCreateIssueDelegate(string subject, string dueDate, string assignedTo, string description);

    public event OnCreateIssueDelegate OnCreateIssue;
    public event Action<GameObject> OnClose;

    /// <summary>
    /// Invokes the <see cref="OnCreateIssue"/> event with the data entered in the UI.
    /// </summary>
    public void OnCreateIssuePressed()
    {
        OnCreateIssue?.Invoke(subject.text, dueDate.text, assignedTo.options[assignedTo.value].text, description.text);
    }

    /// <summary>
    /// Invokes the <see cref="OnClose"/> event and provides the associated prefab.
    /// </summary>
    public void OnCloseButtonPressed()
    {
        OnClose?.Invoke(prefab);
    }
}
