using Dummiesman;
using System;
using UnityEngine;


public class GUIController : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject createIssue;
    public GameObject selectModel;
    public GameObject flagPrefab;
    public GameObject plane;
    public Material material;
    public Transform cameraTransform;
    public InputHandler inputHandler;
    private float spawnDistance = 4f;
    private GameObject currentMainMenu;
    private GameObject currentCreateIssue;
    private GameObject currentSelectModel;
    private GameObject currentModel;
    private MainMenuEventHandler mainMenuEventHandler;
    private CreateIssueEventHandler createIssueEventHandler;
    private SelectModelEventHandler selectModelEventHandler;
    private ModelController modelController;
    private string currentModelName;

    private void Start()
    {
        modelController = GetComponent<ModelController>();
        if (inputHandler != null)
        {
            inputHandler.OnMenuButtonPressed += ToggleMenu;
        }

        if (mainMenu != null)
        {
            mainMenu.SetActive(false);
        }
    }

    private void ToggleMenu()
    {
        if (currentMainMenu == null)
        {
            SpawnUI(mainMenu);
            currentMainMenu.SetActive(true);
            mainMenuEventHandler = currentMainMenu.GetComponentInChildren<MainMenuEventHandler>();

            if (mainMenuEventHandler != null )
            {
                mainMenuEventHandler.createIssueButtonPressed += OnCreateIssueButtonPress;
                mainMenuEventHandler.selectModelButtonPressed += OnSelectModelButtonPress;
            }
        }
        else
        {
            mainMenuEventHandler = currentMainMenu.GetComponentInChildren<MainMenuEventHandler>();

            if (mainMenuEventHandler != null)
            {
                mainMenuEventHandler.createIssueButtonPressed -= OnCreateIssueButtonPress;
                mainMenuEventHandler.selectModelButtonPressed -= OnSelectModelButtonPress;
            }

            Destroy(currentMainMenu);
        }
    }

    private void SpawnUI(GameObject UIPrefab)
    {
        Vector3 spawnPosition = cameraTransform.position + cameraTransform.forward * spawnDistance;

        if (UIPrefab == mainMenu)
        {
            currentMainMenu = Instantiate(mainMenu, spawnPosition, Quaternion.identity);
            currentMainMenu.transform.LookAt(cameraTransform);
            currentMainMenu.transform.rotation = Quaternion.LookRotation(currentMainMenu.transform.position - cameraTransform.position);
        }
        else if (UIPrefab == createIssue)
        {
            currentCreateIssue = Instantiate(createIssue, spawnPosition, Quaternion.identity);
            currentCreateIssue.transform.LookAt(cameraTransform);
            currentCreateIssue.transform.rotation = Quaternion.LookRotation(currentCreateIssue.transform.position - cameraTransform.position);
        }
        else if (UIPrefab == selectModel)
        {
            currentSelectModel = Instantiate(selectModel, spawnPosition, Quaternion.identity);
            currentSelectModel.transform.LookAt(cameraTransform);
            currentSelectModel.transform.rotation = Quaternion.LookRotation(currentSelectModel.transform.position - cameraTransform.position);
        }
        UIPrefab.SetActive(true);
    }

    private void OnCreateIssueButtonPress()
    {
        Destroy(currentMainMenu);
        SpawnUI(createIssue);

        createIssueEventHandler = currentCreateIssue.GetComponentInChildren<CreateIssueEventHandler>();

        if (createIssueEventHandler != null)
        {
            createIssueEventHandler.OnCreateIssue += CreateIssue;
        }
    }

    private void CreateIssue(string subject, string dueDate, string assignedTo, string description)
    {
        SpawnFlag(subject, dueDate, assignedTo, description);
        Destroy(currentCreateIssue);
    }

    private void OnSelectModelButtonPress()
    {
        Destroy(currentMainMenu);
        SpawnUI(selectModel);
        selectModelEventHandler = currentSelectModel.GetComponentInChildren<SelectModelEventHandler>();
        if (selectModelEventHandler != null)
        {
            selectModelEventHandler.OnSelectModel += LoadModel;
        }
    }

    private void SpawnFlag(string subject, string dueDate, string assignedTo, string description)
    {
        if (flagPrefab != null && currentModelName != null)
        {
            Vector3 spawnPosition = Camera.main.transform.position + Camera.main.transform.forward * spawnDistance;
            spawnPosition.y = Camera.main.transform.position.y;
            Instantiate(flagPrefab, spawnPosition, Quaternion.identity);
            modelController.AddFlagToModel(subject, dueDate, assignedTo, description, spawnPosition, currentModelName);
        }
    }

    private void LoadModel(string modelName)
    {
        if (modelName == null)
        {
            Debug.Log("No model is selected.");
            return;
        }
        if (currentSelectModel != null)
        {
            Destroy(currentSelectModel);
        }
        if (currentModel != null)
        {
            DestroyFlags();
            Destroy(currentModel);            
        }
        currentModelName = modelName;
        string objPath = $"C:\\Temp\\Models\\{modelName}.obj";
        string mtlPath = $"C:\\Temp\\Models\\{modelName}.mtl";
        OBJLoader loader = new OBJLoader();
        currentModel = loader.Load(objPath, mtlPath);
        AssignMaterialToObj(currentModel);
        SnapToPlane(currentModel);
        modelController.InitializeModel(modelName);
        InstantiateFlags();
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

    private void DestroyFlags()
    {
        GameObject[] flags = GameObject.FindGameObjectsWithTag("Flag");
        foreach (var flag in flags)
        {
            Destroy(flag);
        }
    }

    private void InstantiateFlags()
    {
        foreach (var flag in modelController.GetModel().Flags)
        {
            if (flag != null && flagPrefab != null)
            {
                Instantiate(flagPrefab, flag.Location, Quaternion.identity);
            }
        }
    }
}