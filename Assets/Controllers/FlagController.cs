using System;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;

public class FlagController : MonoBehaviour
{
    public GameObject flagPrefab;
    public float spawnDistance = 2.0f;
    private Dictionary<Guid, GameObject> flagPrefabDictionary = new Dictionary<Guid, GameObject>();
    private InputDevice rightHandDevice;
    private bool aButtonWasPressed = false;
    private List<Flag> flagList;
    private string jsonPath = "Flags";

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
