using System;
using UnityEngine;
using UnityEngine.XR;
using CommonUsages = UnityEngine.XR.CommonUsages;
using InputDevice = UnityEngine.XR.InputDevice;


/// <summary>
/// EventPublisher associated with controller inputs.
/// </summary>
public class InputEventPublisher : MonoBehaviour
{
    public event Action OnMenuButtonPressed;
    public event Action OnLeftPrimaryButtonPressed;
    public event Action OnRightPrimaryButtonPressed;
    private InputDevice leftHandDevice;
    private InputDevice rightHandDevice;
    private bool menuButtonWasPressed = false;
    private bool leftPrimaryButtonWasPressed = false;
    private bool rightPrimaryButtonWasPressed = false;
    private bool createIssueUIOpen = false;


    /// <summary>
    /// Checks for controller input.
    /// </summary>
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
    }
}

