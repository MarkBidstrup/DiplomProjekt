using System;
using UnityEngine;
using UnityEngine.XR;


public class InputHandler : MonoBehaviour
{
    public float activationThreshold = 0.1f;
    public event Action OnMenuButtonPressed;
    private InputDevice leftHandDevice;
    private bool menuButtonWasPressed = false;

    void Update()
    {
        if (!leftHandDevice.isValid)
        {
            leftHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        }

        leftHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        bool menuButtonPressed = false;
        if (leftHandDevice.isValid && leftHandDevice.TryGetFeatureValue(CommonUsages.menuButton, out menuButtonPressed))
        {
            if (menuButtonPressed && !menuButtonWasPressed)
            {
                OnMenuButtonPressed?.Invoke();
            }
            menuButtonWasPressed = menuButtonPressed;
        }
    }
}

