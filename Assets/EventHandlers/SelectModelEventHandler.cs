using TMPro;
using UnityEngine;


// EventHandler used by the select model UI.
public class SelectModelEventHandler : MonoBehaviour
{
    // Referenced in the unity prefab editor inspector
    [SerializeField]
    private TMP_Dropdown selectedModel;

    public delegate void OnSelectModelDelegate(string selectedModel);

    public event OnSelectModelDelegate OnSelectModel;
    public void OnSelectModelPressed()
    {
        Debug.Log("OnSelectModelPressed invoked");
        OnSelectModel?.Invoke(selectedModel.options[selectedModel.value].text);
    }
}
