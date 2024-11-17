using System;
using UnityEngine;


// EventHandler used by the main menu UI.
public class MainMenuEventHandler : MonoBehaviour
{
    public event Action createIssueButtonPressed;
    public event Action selectModelButtonPressed;
    public event Action viewIssuesButtonPressed;

    public void OnCreateIssueButtonPressed()
    {
        createIssueButtonPressed?.Invoke();
    }

    public void OnSelectModelButtonPressed()
    {
        selectModelButtonPressed?.Invoke();
    }

    public void OnViewIssuesButtonPressed()
    {
        viewIssuesButtonPressed?.Invoke();
    }
}
