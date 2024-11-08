using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dummiesman;
using UnityEngine.VFX;
using System.IO;
using System.Xml.Linq;

public class ModelController : MonoBehaviour
{
    public GameObject plane;
    public Material material;

    void Start()
    {
        string objPath = "C:\\Temp\\Models\\vase\\vase.obj";
        string mtlPath = "C:\\Temp\\Models\\vase\\vase.mtl";
        GameObject loadedObj = LoadModelOBJLoader(objPath,mtlPath);

        //AssignMaterialToObj(loadedObj);
        SnapToPlane(loadedObj);
    }

    void SnapToPlane(GameObject obj)
    {
        Renderer objRenderer = obj.GetComponentInChildren<Renderer>();
        if (objRenderer != null)
        {
            float objBottomY = objRenderer.bounds.min.y;

            Vector3 planePosition = plane.transform.position;
            obj.transform.position = new Vector3(planePosition.x, planePosition.y - objBottomY, planePosition.z);
        }
        else
        {
            Debug.LogError("No Renderer found on the loaded object or its children.");
        }
    }

    void AssignMaterialToObj(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            Material[] newMaterials = new Material[renderer.materials.Length];
            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = material;
            }
            renderer.materials = newMaterials;
        }
    }

    GameObject LoadModelOBJLoader(string filePath, string mtlPath)
    {
        OBJLoader loader = new OBJLoader();
        return loader.Load(filePath, mtlPath);
    }
}
