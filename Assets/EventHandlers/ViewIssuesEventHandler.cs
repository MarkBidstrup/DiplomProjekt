using UnityEngine;
using TMPro;
using System;


public class ViewIssuesEventHandler : MonoBehaviour
{
    // Referenced in the unity prefab editor inspector
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private TMP_Dropdown selectIssue;
    [SerializeField]
    private TMP_InputField dueDate;
    [SerializeField]
    private TMP_Dropdown assignedTo;
    [SerializeField]
    private TMP_InputField description;

    public delegate void OnUpdateDelegate(int index, string subject, string dueDate, string assignedTo, string description);
    public delegate void OnDeleteDelegate(int index, GameObject prefab);
    public delegate void OnTeleportDelegate(int index, GameObject prefab);

    public event OnUpdateDelegate OnUpdate;
    public event Action<GameObject> OnClose;
    public event OnDeleteDelegate OnDelete;
    public event OnTeleportDelegate OnTeleport;

    public void OnUpdatePressed()
    {
        OnUpdate?.Invoke(selectIssue.value, selectIssue.options[selectIssue.value].text, dueDate.text, assignedTo.options[assignedTo.value].text, description.text);
    }

    public void OnCloseButtonPressed()
    {
        OnClose?.Invoke(prefab);
    }

    public void OnDeleteButtonPressed()
    {
        OnDelete?.Invoke(selectIssue.value, prefab);
    }

    public void OnTeleportButtonPressed()
    {
        OnTeleport?.Invoke(selectIssue.value, prefab);
    }
}
