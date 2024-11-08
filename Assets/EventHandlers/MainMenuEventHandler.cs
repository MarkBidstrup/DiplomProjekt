using System;
using UnityEngine;

public class MainMenuEventHandler : MonoBehaviour
{
    public event Action createIssueButtonPressed;

    public void OnCreateIssueButtonPressed()
    {
        createIssueButtonPressed?.Invoke();
    }
}
