using System;
using UnityEngine;

public class MainMenuEventHandler : MonoBehaviour
{
    public event Action createIssueButtonPressed;
    public event Action selectModelButtonPressed;

    public void OnCreateIssueButtonPressed()
    {
        createIssueButtonPressed?.Invoke();
    }

    public void OnSelectModelButtonPressed()
    {
        selectModelButtonPressed?.Invoke();
    }
}
