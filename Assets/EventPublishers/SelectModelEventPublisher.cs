using System;
using TMPro;
using UnityEngine;


/// <summary>
/// EventPublisher associated with the select model UI.
/// </summary>
public class SelectModelEventPublisher : MonoBehaviour
{
    // Referenced in the unity prefab editor inspector
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private TMP_Dropdown selectedModel;

    /// <summary>
    /// Delegate for handling the Select Model event.
    /// </summary>
    /// <param name="selectedModel">The selected model's name.</param>
    public delegate void OnSelectModelDelegate(string selectedModel);

    public event OnSelectModelDelegate OnSelectModel;
    public event Action<GameObject> OnClose;

    /// <summary>
    /// Invokes the <see cref="OnSelectModel"/> event with the selected model's name.
    /// </summary>
    public void OnSelectModelPressed()
    {
        OnSelectModel?.Invoke(selectedModel.options[selectedModel.value].text);
    }

    /// <summary>
    /// Invokes the <see cref="OnClose"/> event and provides the associated prefab.
    /// </summary>
    public void OnCloseButtonPressed()
    {
        OnClose?.Invoke(prefab);
    }
}
