using System;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;


// Handles controller input.
public class InputHandler : MonoBehaviour
{
    public event Action OnMenuButtonPressed;
    private InputDevice leftHandDevice;
    private InputDevice rightHandDevice;
    private bool menuButtonWasPressed = false;
    private bool rightPrimaryButtonWasPressed = false;

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

        // Check for right controller primary button press.
        if (!rightHandDevice.isValid)
        {
            rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        }
        bool rightPrimaryButtonPressed = false;
        if (rightHandDevice.isValid && rightHandDevice.TryGetFeatureValue(CommonUsages.primaryButton, out rightPrimaryButtonPressed))
        {
            if (rightPrimaryButtonPressed && !rightPrimaryButtonWasPressed)
            {
                Debug.Log("Right primary button was pressed");
            }
            rightPrimaryButtonWasPressed = rightPrimaryButtonPressed;
        }
    }
}

