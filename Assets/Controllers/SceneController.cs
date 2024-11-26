using Dummiesman;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;
using UnityEngine.UI;
using System;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;
using UnityEngine.XR.Interaction.Toolkit;


// Responsible for controlling graphical elements and events from UI.
public class SceneController : MonoBehaviour
{
    // Referenced in the unity editor inspector
    [SerializeField]
    private GameObject mainMenuPrefab;
    [SerializeField]
    private GameObject createIssuePrefab;
    [SerializeField]
    private GameObject selectModelPrefab;
    [SerializeField]
    private GameObject viewIssuesPrefab;
    [SerializeField]
    private GameObject flagPrefab;
    [SerializeField]
    private GameObject plane;
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private InputEventPublisher inputEventPublisher;
    [SerializeField]
    private GameObject character;

    // current 3D objects
    private GameObject currentMainMenu;
    private GameObject currentCreateIssue;
    private GameObject currentSelectModel;
    private GameObject currentViewIssues;
    private GameObject currentModel;

    // Event publishers
    private MainMenuEventPublisher mainMenuEventPublisher;
    private CreateIssueEventPublisher createIssueEventPublisher;
    private SelectModelEventPublisher selectModelEventPublisher;
    private ViewIssuesEventPublisher viewIssuesEventPublisher;

    private ModelController modelController;
    
    private float spawnDistance = 1f;
    private string currentModelName;
    private bool isTurbo = false;

    private void Awake()
    {
        modelController = GameObject.FindWithTag("ModelController").GetComponent<ModelController>();
    }

    // Called when the scene initializes.
    private void Start()
    {
        if (inputEventPublisher != null)
        {
            inputEventPublisher.OnMenuButtonPressed += HandleMenuButtonPressedEvent;
            inputEventPublisher.OnLeftPrimaryButtonPressed += HandleCreateIssueButtonPressedEvent;
            inputEventPublisher.OnRightPrimaryButtonPressed += HandleRightPrimaryButtonPressedEvent;
        }

        if (mainMenuPrefab != null)
        {
            mainMenuPrefab.SetActive(false);
        }
    }

    // Toggles the main menu UI and subscribes/unsubscribes to menu events.
    private void HandleMenuButtonPressedEvent()
    {
        if (currentMainMenu == null)
        {
            SpawnUI(mainMenuPrefab);
            currentMainMenu.SetActive(true);
            mainMenuEventPublisher = currentMainMenu.GetComponentInChildren<MainMenuEventPublisher>();

            if (mainMenuEventPublisher != null )
            {
                mainMenuEventPublisher.createIssueButtonPressed += HandleCreateIssueButtonPressedEvent;
                mainMenuEventPublisher.selectModelButtonPressed += HandleSelectModelButtonPressedEvent;
                mainMenuEventPublisher.viewIssuesButtonPressed += HandleViewIssuesButtonPressedEvent;
            }
        }
        else
        {
            mainMenuEventPublisher = currentMainMenu.GetComponentInChildren<MainMenuEventPublisher>();

            if (mainMenuEventPublisher != null)
            {
                mainMenuEventPublisher.createIssueButtonPressed -= HandleCreateIssueButtonPressedEvent;
                mainMenuEventPublisher.selectModelButtonPressed -= HandleSelectModelButtonPressedEvent;
                mainMenuEventPublisher.viewIssuesButtonPressed -= HandleViewIssuesButtonPressedEvent;
            }

            Destroy(currentMainMenu);
        }
    }

    // Spawns UI object in the scene.
    private void SpawnUI(GameObject UIPrefab)
    {
        Vector3 spawnPosition = cameraTransform.position + cameraTransform.forward * spawnDistance;

        switch (UIPrefab)
        {
            case GameObject prefab when prefab == mainMenuPrefab:
                currentMainMenu = Instantiate(mainMenuPrefab, spawnPosition, Quaternion.identity);
                currentMainMenu.transform.LookAt(cameraTransform);
                currentMainMenu.transform.rotation = Quaternion.LookRotation(currentMainMenu.transform.position - cameraTransform.position);
                break;

            case GameObject prefab when prefab == createIssuePrefab:
                currentCreateIssue = Instantiate(createIssuePrefab, spawnPosition, Quaternion.identity);
                currentCreateIssue.transform.LookAt(cameraTransform);
                currentCreateIssue.transform.rotation = Quaternion.LookRotation(currentCreateIssue.transform.position - cameraTransform.position);
                break;

            case GameObject prefab when prefab == selectModelPrefab:
                currentSelectModel = Instantiate(selectModelPrefab, spawnPosition, Quaternion.identity);
                currentSelectModel.transform.LookAt(cameraTransform);
                currentSelectModel.transform.rotation = Quaternion.LookRotation(currentSelectModel.transform.position - cameraTransform.position);
                break;

            case GameObject prefab when prefab == viewIssuesPrefab:
                currentViewIssues = Instantiate(viewIssuesPrefab, spawnPosition, Quaternion.identity);
                currentViewIssues.transform.LookAt(cameraTransform);
                currentViewIssues.transform.rotation = Quaternion.LookRotation(currentViewIssues.transform.position - cameraTransform.position);
                break;

            default:
                Debug.LogError("Unhandled UIPrefab type.");
                break;
        }

        UIPrefab.SetActive(true);
    }

    // Runs when create issue button is pressed in the main menu UI.
    private void HandleCreateIssueButtonPressedEvent()
    {
        if (modelController.GetModel() == null)
        {
            return;
        }
        Destroy(currentMainMenu);
        SpawnUI(createIssuePrefab);

        createIssueEventPublisher = currentCreateIssue.GetComponentInChildren<CreateIssueEventPublisher>();

        if (createIssueEventPublisher != null)
        {
            createIssueEventPublisher.OnCreateIssue += HandleCreateIssueEvent;
            createIssueEventPublisher.OnClose += HandleCloseEvent;
        }
    }

    // Runs when select model button is pressed in the main menu UI.
    private void HandleSelectModelButtonPressedEvent()
    {
        Destroy(currentMainMenu);
        SpawnUI(selectModelPrefab);
        selectModelEventPublisher = currentSelectModel.GetComponentInChildren<SelectModelEventPublisher>();
        if (selectModelEventPublisher != null)
        {
            selectModelEventPublisher.OnSelectModel += HandleSelectModelEvent;
            selectModelEventPublisher.OnClose += HandleCloseEvent;
        }
    }

    // Runs when view issues button is pressed in the main menu UI.
    private void HandleViewIssuesButtonPressedEvent()
    {
        if (modelController.GetModel() == null)
        {
            return;
        }
        Destroy(currentMainMenu);
        SpawnUI(viewIssuesPrefab);
        viewIssuesEventPublisher = currentViewIssues.GetComponentInChildren<ViewIssuesEventPublisher>();

        if (viewIssuesEventPublisher != null)
        {
            viewIssuesEventPublisher.OnUpdate += HandleUpdateEvent;
            viewIssuesEventPublisher.OnClose += HandleCloseEvent;
            viewIssuesEventPublisher.OnDelete += HandleDeleteEvent;
            viewIssuesEventPublisher.OnTeleport += HandleTeleportEvent;
        }
    }

    private void HandleDeleteEvent(int index, GameObject prefabInstance)
    {
        if (index > 0)
        {
            int indexWithOffset = index - 1; // Offset needed because of template option
            DestroyFlag(indexWithOffset);
            modelController.DeleteIssue(indexWithOffset, currentModelName);
        }
        Destroy(currentViewIssues);
    }

    private void HandleUpdateEvent(int index, string subject, string dueDate, string assignedTo, string description)
    {
        int indexWithOffset = index - 1; // Offset needed because of template option
        modelController.UpdateIssue(indexWithOffset, subject, dueDate, assignedTo, description, currentModelName);
        Destroy(currentViewIssues);
    }

    // Runs when the create issue button is pressed in the create issue UI.
    private void HandleCreateIssueEvent(string subject, string dueDate, string assignedTo, string description)
    {
        SpawnFlag(subject, dueDate, assignedTo, description);
        Destroy(currentCreateIssue);
    }

    // Spawns a 3D flag object in the scene and adds a flag to the model.
    private void SpawnFlag(string subject, string dueDate, string assignedTo, string description)
    {
        if (flagPrefab != null && currentModelName != null)
        {
            Vector3 spawnPosition = Camera.main.transform.position + Camera.main.transform.forward * spawnDistance;
            spawnPosition.y = Camera.main.transform.position.y;
            GameObject flagObject = Instantiate(flagPrefab, spawnPosition, Quaternion.identity);
            Guid flagId = Guid.NewGuid();
            FlagInstance flagInstance = flagObject.AddComponent<FlagInstance>();
            flagInstance.FlagId = flagId;
            modelController.AddIssueToModel(flagId, subject, dueDate, assignedTo, description, spawnPosition, currentModelName);
        }
    }

    private void DestroyFlag(int index)
    {
        Issue issueToDelete = modelController.GetModel().Issues[index];
        GameObject[] flagPrefabs = GameObject.FindGameObjectsWithTag("Flag");
        foreach (var flagPrefab in flagPrefabs)
        {
            FlagInstance flagInstance = flagPrefab.GetComponent<FlagInstance>();
            if (flagInstance != null && flagInstance.FlagId == issueToDelete.IssueId)
            {
                Destroy(flagPrefab);
                break;
            }
        }
    }

    // Runs when select button is pressed in the select model UI.
    // Destroys all flags in the scene.
    // Imports model in runtime with OBJLoader.
    // Instantiates flags in the loaded model to the scene.
    private void HandleSelectModelEvent(string modelName)
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
        AssignColorToMaterialInModel(currentModel);
        MoveCharacterToModelHighestPoint(currentModel);
        modelController.InitializeModel(modelName);
        InstantiateFlags();
        ReinitializeXRRig();
    }

    // Reinitialize XR Rig to prevent locomotion to break
    private void ReinitializeXRRig()
    {
        character.SetActive(false);
        character.SetActive(true);
    }

    // Moves the character to the highest point in the model
    private void MoveCharacterToModelHighestPoint(GameObject model)
    {
        float highestY = CalculateHighestY(model);

        Vector3 playerPosition = character.transform.position;
        playerPosition.y = highestY;
        character.transform.position = playerPosition;
    }

    // Calculates the highest y value of the model
    private float CalculateHighestY(GameObject model)
    {
        Renderer[] renderers = model.GetComponentsInChildren<Renderer>();

        float highestY = float.MinValue;

        foreach (Renderer renderer in renderers)
        {
            highestY = Mathf.Max(highestY, renderer.bounds.max.y);
        }

        return highestY;
    }

    // Assigns material base color to the 3D models materials.
    private void AssignColorToMaterialInModel(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            for (int i = 0;i < renderer.materials.Length;i++)
            {
                Material material = renderer.materials[i];
                Color currentColor = material.color; // get the color before changing shader.
                material.shader = Shader.Find("Universal Render Pipeline/Lit"); // Project pipeline does not support Standard (Specular setup) shader.
                material.SetColor("_BaseColor", currentColor);
            }
        }
    }

    // Destroys all flags in the scene.
    private void DestroyFlags()
    {
        GameObject[] flags = GameObject.FindGameObjectsWithTag("Flag");
        foreach (var flag in flags)
        {
            Destroy(flag);
        }
    }

    // Instantiates issues from the model as flag objects in the scene.
    private void InstantiateFlags()
    {
        if (modelController.GetModel().Issues == null)
        {
            return;
        }
        foreach (Issue issue in modelController.GetModel().Issues)
        {
            if (issue != null && flagPrefab != null)
            {
                GameObject flagObject = Instantiate(flagPrefab, issue.Location, Quaternion.identity);
                FlagInstance flagInstance = flagObject.AddComponent<FlagInstance>();
                flagInstance.FlagId = issue.IssueId;
            }
        }
    }

    // Handles the teleport event
    private void HandleTeleportEvent(int index, GameObject prefabInstance)
    {
        if (index > 0)
        {
            Issue issueToTeleport = modelController.GetModel().Issues[index-1];
            character.transform.position = issueToTeleport.Location;
            Destroy(prefabInstance);
        }
    }

    // Handles the event triggered when a close button is pressed
    private void HandleCloseEvent(GameObject prefabInstance)
    {
        Debug.Log(prefabInstance.name);
        Destroy(prefabInstance);

        if (prefabInstance == currentCreateIssue && createIssueEventPublisher != null)
        {
            createIssueEventPublisher.OnCreateIssue -= HandleCreateIssueEvent;
            createIssueEventPublisher.OnClose -= HandleCloseEvent;
        }
        else if (prefabInstance == currentSelectModel && selectModelEventPublisher != null)
        {
            selectModelEventPublisher.OnSelectModel -= HandleSelectModelEvent;
            selectModelEventPublisher.OnClose -= HandleCloseEvent;
        }
        else if (prefabInstance == currentViewIssues && viewIssuesEventPublisher != null)
        {
            viewIssuesEventPublisher.OnUpdate -= HandleUpdateEvent;
            viewIssuesEventPublisher.OnClose -= HandleCloseEvent;
            viewIssuesEventPublisher.OnDelete -= HandleDeleteEvent;
            viewIssuesEventPublisher.OnTeleport -= HandleTeleportEvent;
        }
    }

    // Handles the event triggered when the right primary button is pressed
    // Toggles turbo move speed
    private void HandleRightPrimaryButtonPressedEvent()
    {
        ContinuousMoveProvider moveProvider = FindObjectOfType<ContinuousMoveProvider>();
        if (moveProvider != null)
        {
            if (!isTurbo)
            {
                moveProvider.moveSpeed = 25f;
                isTurbo = true;
            }
            else
            {
                moveProvider.moveSpeed = 5f;
                isTurbo = false;
            }
        }
    }
}