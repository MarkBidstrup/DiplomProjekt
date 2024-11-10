using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectModelEventHandler : MonoBehaviour
{
    public TMP_Dropdown selectedModel;

    public delegate void OnSelectModelDelegate(string selectedModel);

    public event OnSelectModelDelegate OnSelectModel;
    public void OnSelectModelPressed()
    {
        Debug.Log("OnSelectModelPressed invoked");
        OnSelectModel?.Invoke(selectedModel.options[selectedModel.value].text);
    }
}
