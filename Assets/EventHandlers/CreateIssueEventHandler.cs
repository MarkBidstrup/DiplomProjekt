using UnityEngine;
using TMPro;
using UnityEngine.Rendering.UI;

public class CreateIssueEventHandler : MonoBehaviour
{
    public TMP_InputField subject;
    public TMP_InputField dueDate;
    public TMP_Dropdown assignedTo;
    public TMP_InputField description;

    public delegate void OnCreateIssueDelegate(string subject, string dueDate, string assignedTo, string description);

    public event OnCreateIssueDelegate OnCreateIssue;
    public void OnCreateIssuePressed()
    {
        Debug.Log("OnCreateIssuePressed invoked");
        OnCreateIssue?.Invoke(subject.text, dueDate.text, assignedTo.options[assignedTo.value].text, description.text);
    }
}
