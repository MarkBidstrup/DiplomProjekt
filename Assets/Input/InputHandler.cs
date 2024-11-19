using System;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;


// Handles controller input.
public class InputHandler : MonoBehaviour
{
    public event Action OnMenuButtonPressed;
    public event Action OnleftPrimaryButtonPressed;
    private InputDevice leftHandDevice;
    private InputDevice rightHandDevice;
    private bool menuButtonWasPressed = false;
    private bool leftPrimaryButtonWasPressed = false;
    private bool createIssueUIOpen = false;

    void Update()
    {
        // Check for left menu button press.
        if (!leftHandDevice.isValid)
        {
            leftHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        }
        bool menuButtonPressed = false;
        if (leftHandDevice.isValid && leftHandDevice.TryGetFeatureValue(CommonUsages.menuButton, out menuButtonPressed))
        {
            if (menuButtonPressed && !menuButtonWasPressed)
            {
                OnMenuButtonPressed?.Invoke();
            }
            menuButtonWasPressed = menuButtonPressed;
        }
        bool leftPrimaryButtonPressed = false;
        if (leftHandDevice.isValid && leftHandDevice.TryGetFeatureValue(CommonUsages.primaryButton, out leftPrimaryButtonPressed))
        {
            if (leftPrimaryButtonPressed && !leftPrimaryButtonWasPressed && !createIssueUIOpen)
            {
                OnleftPrimaryButtonPressed.Invoke();
            }
            leftPrimaryButtonWasPressed = leftPrimaryButtonPressed;
        }

        // Check for right controller primary button press.
        if (!rightHandDevice.isValid)
        {
            rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        }
    }
}

