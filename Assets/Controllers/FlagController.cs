using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;


// FlagController is no longer in use. See delete flag comment.
public class FlagController : MonoBehaviour
{
    public GameObject flagPrefab;
    public float spawnDistance = 2.0f;
    private Dictionary<Guid, GameObject> flagPrefabDictionary = new Dictionary<Guid, GameObject>();
    private List<Flag> flagList;
    private string jsonPath = "Flags";

    // Old code, not used
    // Implement this in ModelController instead
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
