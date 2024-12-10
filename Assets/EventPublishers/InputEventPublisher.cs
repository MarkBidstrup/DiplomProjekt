using System;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;


// Checks for controller input and publishes events.
public class InputEventPublisher : MonoBehaviour
{
    public event Action OnMenuButtonPressed;
    public event Action OnLeftPrimaryButtonPressed;
    public event Action OnRightPrimaryButtonPressed;
    public event Action OnRightSecondaryButtonPressed;
    private InputDevice leftHandDevice;
    private InputDevice rightHandDevice;
    private bool menuButtonWasPressed = false;
    private bool leftPrimaryButtonWasPressed = false;
    private bool rightPrimaryButtonWasPressed = false;
    private bool rightSecondaryButtonWasPressed = false;
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
                OnLeftPrimaryButtonPressed.Invoke();
            }
            leftPrimaryButtonWasPressed = leftPrimaryButtonPressed;
        }

        // Check for right controller primary button press.
        if (!rightHandDevice.isValid)
        {
            rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        }
        bool rightPrimaryButtonPressed = false;
        if (rightHandDevice.isValid && rightHandDevice.TryGetFeatureValue(CommonUsages.primaryButton, out rightPrimaryButtonPressed))
        {
            if (rightPrimaryButtonPressed && !rightPrimaryButtonWasPressed && !createIssueUIOpen)
            {
                OnRightPrimaryButtonPressed.Invoke();
            }
            rightPrimaryButtonWasPressed = rightPrimaryButtonPressed;
        }
        bool rightSecondaryButtonPressed = false;
        if (rightHandDevice.isValid && rightHandDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out rightSecondaryButtonPressed))
        {
            if (rightSecondaryButtonPressed && !rightSecondaryButtonWasPressed && !createIssueUIOpen)
            {
                OnRightSecondaryButtonPressed.Invoke();
            }
            rightSecondaryButtonWasPressed = rightSecondaryButtonPressed;
        }
    }
}

