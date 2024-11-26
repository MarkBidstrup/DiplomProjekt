using System;
using UnityEngine;


// EventPublisher used by the main menu UI.
public class MainMenuEventPublisher : MonoBehaviour
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
