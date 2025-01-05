using UnityEngine;
using TMPro;
using System;


/// <summary>
/// EventPublisher associated with the view issues UI.
/// </summary>
public class ViewIssuesEventPublisher : MonoBehaviour
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

    /// <summary>
    /// Delegate for handling issue updates.
    /// </summary>
    /// <param name="index">The issue index.</param>
    /// <param name="subject">The issue subject.</param>
    /// <param name="dueDate">The issue due date.</param>
    /// <param name="assignedTo">The issue assignee.</param>
    /// <param name="description">The issue description.</param>
    public delegate void OnUpdateDelegate(int index, string subject, string dueDate, string assignedTo, string description);

    /// <summary>
    /// Delegate for handling issue deletion.
    /// </summary>
    /// <param name="index">The issue index.</param>
    /// <param name="prefab">The UI prefab.</param>
    public delegate void OnDeleteDelegate(int index, GameObject prefab);

    /// <summary>
    /// Delegate for handling issue teleportation.
    /// </summary>
    /// <param name="index">The issue index.</param>
    /// <param name="prefab">The UI prefab.</param>
    public delegate void OnTeleportDelegate(int index, GameObject prefab);

    public event OnUpdateDelegate OnUpdate;
    public event Action<GameObject> OnClose;
    public event OnDeleteDelegate OnDelete;
    public event OnTeleportDelegate OnTeleport;

    /// <summary>
    /// Invokes the <see cref="OnUpdate"/> event with the updated issue details.
    /// </summary>
    public void OnUpdatePressed()
    {
        OnUpdate?.Invoke(selectIssue.value, selectIssue.options[selectIssue.value].text, dueDate.text, assignedTo.options[assignedTo.value].text, description.text);
    }

    /// <summary>
    /// Invokes the <see cref="OnClose"/> event and provides the UI prefab.
    /// </summary>
    public void OnCloseButtonPressed()
    {
        OnClose?.Invoke(prefab);
    }

    /// <summary>
    /// Invokes the <see cref="OnDelete"/> event and provides the index of the selected issue and the UI prefab.
    /// </summary>
    public void OnDeleteButtonPressed()
    {
        OnDelete?.Invoke(selectIssue.value, prefab);
    }

    /// <summary>
    /// Invokes the <see cref="OnTeleport"/> event and provides the index of the selected issue and the UI prefab.
    /// </summary>
    public void OnTeleportButtonPressed()
    {
        OnTeleport?.Invoke(selectIssue.value, prefab);
    }
}
