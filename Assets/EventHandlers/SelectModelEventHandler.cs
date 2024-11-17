using System;
using TMPro;
using UnityEngine;


// EventHandler used by the select model UI.
public class SelectModelEventHandler : MonoBehaviour
{
    // Referenced in the unity prefab editor inspector
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private TMP_Dropdown selectedModel;

    public delegate void OnSelectModelDelegate(string selectedModel);

    public event OnSelectModelDelegate OnSelectModel;
    public event Action<GameObject> OnClose;

    public void OnSelectModelPressed()
    {
        OnSelectModel?.Invoke(selectedModel.options[selectedModel.value].text);
    }

    public void OnCloseButtonPressed()
    {
        Debug.Log(prefab.name);
        OnClose?.Invoke(prefab);
    }
}
