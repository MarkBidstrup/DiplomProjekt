using UnityEngine;
using TMPro;
using System;


// EventHandler used by the create issue UI.
public class CreateIssueEventHandler : MonoBehaviour
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

    public delegate void OnCreateIssueDelegate(string subject, string dueDate, string assignedTo, string description);

    public event OnCreateIssueDelegate OnCreateIssue;
    public event Action<GameObject> OnClose;

    public void OnCreateIssuePressed()
    {
        OnCreateIssue?.Invoke(subject.text, dueDate.text, assignedTo.options[assignedTo.value].text, description.text);
    }

    public void OnCloseButtonPressed()
    {
        OnClose?.Invoke(prefab);
    }
}
