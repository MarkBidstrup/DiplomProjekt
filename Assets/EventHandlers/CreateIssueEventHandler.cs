using UnityEngine;
using TMPro;


// EventHandler used by the create issue UI.
public class CreateIssueEventHandler : MonoBehaviour
{
    // Referenced in the unity prefab editor inspector
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
    public void OnCreateIssuePressed()
    {
        Debug.Log("OnCreateIssuePressed invoked");
        OnCreateIssue?.Invoke(subject.text, dueDate.text, assignedTo.options[assignedTo.value].text, description.text);
    }
}
