using System;
using UnityEngine;


/// <summary>
/// EventPublisher associated with the main menu UI.
/// </summary>
public class MainMenuEventPublisher : MonoBehaviour
{
    public event Action createIssueButtonPressed;
    public event Action selectModelButtonPressed;
    public event Action viewIssuesButtonPressed;

    /// <summary>
    /// Invokes the <see cref="createIssueButtonPressed"/> event.
    /// </summary>
    public void OnCreateIssueButtonPressed()
    {
        createIssueButtonPressed?.Invoke();
    }

    /// <summary>
    /// Invokes the <see cref="selectModelButtonPressed"/> event.
    /// </summary>
    public void OnSelectModelButtonPressed()
    {
        selectModelButtonPressed?.Invoke();
    }

    /// <summary>
    /// Invokes the <see cref="viewIssuesButtonPressed"/> event.
    /// </summary>
    public void OnViewIssuesButtonPressed()
    {
        viewIssuesButtonPressed?.Invoke();
    }
}
