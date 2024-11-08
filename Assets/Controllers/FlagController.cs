using System;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;

public class FlagController : MonoBehaviour
{
    public GameObject flagPrefab;
    public Transform player;
    public float spawnDistance = 2.0f;
    private Dictionary<Guid, GameObject> flagPrefabDictionary = new Dictionary<Guid, GameObject>();
    private InputDevice rightHandDevice;
    private bool aButtonWasPressed = false;
    private List<Flag> flagList = new List<Flag>();
    private string jsonPath = "Flags";

    void Start()
    {
        List<Flag> jsonFlagList = JsonUtil.Deserialize<List<Flag>>(jsonPath);
        if (jsonFlagList != null)
        {
            flagList.AddRange(jsonFlagList);
        }

        if (flagList != null)
        {
            foreach (Flag flag in flagList)
            {
                GameObject flagInstance = Instantiate(flagPrefab, flag.Location, Quaternion.identity);
                flagPrefabDictionary.Add(flag.FlagId, flagInstance);
            }
        }
    }

    void Update()
    {
        //if (!rightHandDevice.isValid)
        //{
        //    rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        //}

        //// Check if the A button is pressed on the right controller
        //bool aButtonPressed = false;
        //if (rightHandDevice.isValid && rightHandDevice.TryGetFeatureValue(CommonUsages.primaryButton, out aButtonPressed))
        //{
        //    // Detect when the button is pressed down (transition from not pressed to pressed)
        //    if (aButtonPressed && !aButtonWasPressed)
        //    {
        //        Debug.Log("A button is pressed!");
        //        SpawnFlag();
        //    }

        //    // Update the previous button state
        //    aButtonWasPressed = aButtonPressed;
        //}
    }

    public void SpawnFlag(string subject, string dueDate, string assignedTo, string description)
    {
        if (flagPrefab != null)
        {
            Vector3 spawnPosition = Camera.main.transform.position + Camera.main.transform.forward * spawnDistance;
            spawnPosition.y = Camera.main.transform.position.y;

            GameObject flagInstance = Instantiate(flagPrefab, spawnPosition, Quaternion.identity);

            Flag flag = new Flag(Guid.NewGuid(), spawnPosition, subject, dueDate, assignedTo, description);
            flagList.Add(flag);
            flagPrefabDictionary.Add(flag.FlagId, flagInstance);

            JsonUtil.Serialize(flagList, jsonPath);
        }
    }

    void DeleteFlag(Guid flagId)
    {
        Flag flagToRemove = flagList.Find(f => f.FlagId == flagId);

        if (flagToRemove != null)
        {
            if (flagPrefabDictionary.TryGetValue(flagId, out GameObject flagInstance))
            {
                Destroy(flagInstance);
                flagPrefabDictionary.Remove(flagId);
            }

            flagList.Remove(flagToRemove);

            JsonUtil.Serialize(flagList, jsonPath);
        }
    }
}
