using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectModelManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown dropdown;
    private string folderPath = "C:\\Temp\\Models";

    private void Start()
    {
        List<string> modelNames = GetModelNames(folderPath);
        DropdownUtil.SetDropdownOptions(modelNames, dropdown);
    }

    private List<string> GetModelNames(string path)
    {
        List<string> fileNames = new List<string>();

        if (Directory.Exists(path))
        {
            string[] files = Directory.GetFiles(path, "*.obj");

            foreach (string file in files)
            {
                fileNames.Add(Path.GetFileNameWithoutExtension(file));
            }
        }
        else
        {
            Debug.LogWarning("Directory does not exist: " + path);
        }

        return fileNames;
    }
}
