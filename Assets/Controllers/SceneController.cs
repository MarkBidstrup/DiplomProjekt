using Dummiesman;
using UnityEngine;
using System;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;
using UnityEngine.InputSystem;


/// <summary>
/// Controls the logic of GameObjects in the Unity scene.
/// </summary>
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
    [SerializeField]
    private InputActionReference leftInputAction;

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


    /// <summary>
    /// Gets reference to model controller.
    /// </summary>
    private void Awake()
    {
        modelController = ModelController.Instance;
    }

    /// <summary>
    /// Subscribes to input events and makes sure the main menu is not active.
    /// </summary>
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

    /// <summary>
    /// Toggles the main menu UI and subscribes/unsubscribes to menu events.
    /// </summary>
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

            DestroyUI(currentMainMenu);
        }
    }

    /// <summary>
    /// Instantiates UI object.
    /// </summary>
    /// <param name="UIPrefab">The prefab of the UI to be instantiated.</param>
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

    /// <summary>
    /// Destroys current main menu, instantiates the create issue prefab and subscribes to the events of the create issue event publisher.
    /// </summary>
    private void HandleCreateIssueButtonPressedEvent()
    {
        if (modelController.GetModel() == null)
        {
            return;
        }
        DestroyUI(currentMainMenu);
        SpawnUI(createIssuePrefab);

        createIssueEventPublisher = currentCreateIssue.GetComponentInChildren<CreateIssueEventPublisher>();

        if (createIssueEventPublisher != null)
        {
            createIssueEventPublisher.OnCreateIssue += HandleCreateIssueEvent;
            createIssueEventPublisher.OnClose += HandleCloseEvent;
        }
    }

    /// <summary>
    /// Destroys current main menu, instantiates the select model prefab and subscribes to the events of the select model event publisher.
    /// </summary>
    private void HandleSelectModelButtonPressedEvent()
    {
        DestroyUI(currentMainMenu);
        SpawnUI(selectModelPrefab);
        selectModelEventPublisher = currentSelectModel.GetComponentInChildren<SelectModelEventPublisher>();
        if (selectModelEventPublisher != null)
        {
            selectModelEventPublisher.OnSelectModel += HandleSelectModelEvent;
            selectModelEventPublisher.OnClose += HandleCloseEvent;
        }
    }

    /// <summary>
    /// Destroys current main menu, instantiates the view issues prefab and subscribes to the events of the view issues event publisher.
    /// </summary>
    private void HandleViewIssuesButtonPressedEvent()
    {
        if (modelController.GetModel() == null)
        {
            return;
        }
        DestroyUI(currentMainMenu);
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

    /// <summary>
    /// Destroys flag prefab, deletes issue by index and destroys current view issues prefab.
    /// </summary>
    /// <param name="index">The issue index.</param>
    /// <param name="prefabInstance">The prefab instance.</param>
    private void HandleDeleteEvent(int index, GameObject prefabInstance)
    {
        if (index > 0)
        {
            int indexWithOffset = index - 1; // Offset needed because of template option
            DestroyFlag(indexWithOffset);
            modelController.DeleteIssue(indexWithOffset, currentModelName);
        }

        DestroyUI(currentViewIssues);
    }

    /// <summary>
    /// Updates issue by index and destroys current view issues prefab.
    /// </summary>
    /// <param name="index">The issue index.</param>
    /// <param name="subject">The issue subject.</param>
    /// <param name="dueDate">The issue due date.</param>
    /// <param name="assignedTo">The issue assignee.</param>
    /// <param name="description">The issue description.</param>
    private void HandleUpdateEvent(int index, string subject, string dueDate, string assignedTo, string description)
    {
        int indexWithOffset = index - 1; // Offset needed because of template option
        modelController.UpdateIssue(indexWithOffset, subject, dueDate, assignedTo, description, currentModelName);
        DestroyUI(currentViewIssues);
    }

    /// <summary>
    /// Instantiates flag prefab and destroys the current create issue prefab.
    /// </summary>
    /// <param name="subject">The issue subject.</param>
    /// <param name="dueDate">The issue due date.</param>
    /// <param name="assignedTo">The issue assignee.</param>
    /// <param name="description">The issue description.</param>
    private void HandleCreateIssueEvent(string subject, string dueDate, string assignedTo, string description)
    {
        SpawnFlag(subject, dueDate, assignedTo, description);
        DestroyUI(currentCreateIssue);
    }

    /// <summary>
    /// Instantiates flag prefab and adds new issue to the model's list of issues.
    /// </summary>
    /// <param name="subject">The issue subject.</param>
    /// <param name="dueDate">The issue due date.</param>
    /// <param name="assignedTo">The issue assignee.</param>
    /// <param name="description">The issue description.</param>
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

    /// <summary>
    /// Destroys flag prefab by index.
    /// </summary>
    /// <param name="index">The issue index.</param>
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

    /// <summary>
    /// Imports 3D model at runtime, assigns materials to the model, moves user's viewpoint to the highest value of the y-axis of the model, initializes model and instantiates flag prefabs associated with the model. 
    /// </summary>
    /// <param name="index">The issue index.</param>
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
        leftInputAction.action.Enable(); // Hack to prevent disabling of Unity input actions, which will prevent user locomotion movement.
    }

    /// <summary>
    /// Moves the character to the highest value of the y-axis in the model.
    /// </summary>
    /// <param name="model">The current model.</param>
    private void MoveCharacterToModelHighestPoint(GameObject model)
    {
        float highestY = CalculateHighestY(model);

        Vector3 playerPosition = character.transform.position;
        playerPosition.y = highestY;
        character.transform.position = playerPosition;
    }

    /// <summary>
    /// Calculates the highest y value of the model.
    /// </summary>
    /// <param name="model">The current model.</param>
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

    /// <summary>
    /// Assigns material base color to the current models materials.
    /// </summary>
    /// <param name="model">The current model.</param>
    private void AssignColorToMaterialInModel(GameObject model)
    {
        Renderer[] renderers = model.GetComponentsInChildren<Renderer>();
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

    /// <summary>
    /// Destroys all flags.
    /// </summary>
    private void DestroyFlags()
    {
        GameObject[] flags = GameObject.FindGameObjectsWithTag("Flag");
        foreach (var flag in flags)
        {
            Destroy(flag);
        }
    }

    /// <summary>
    /// Instantiates flags associated with the issues of the model.
    /// </summary>
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

    /// <summary>
    /// Moves the character to the location of an issue by index and destroys the UI prefab.
    /// </summary>
    /// <param name="index">The issue index.</param>
    /// <param name="prefabInstance">The UI prefab.</param>
    private void HandleTeleportEvent(int index, GameObject prefabInstance)
    {
        if (index > 0)
        {
            Issue issueToTeleport = modelController.GetModel().Issues[index-1];
            character.transform.position = issueToTeleport.Location;
            DestroyUI(prefabInstance);
        }
    }

    /// <summary>
    /// Destroys the UI prefab and unsubscribes to the events associated with the UI prefab.
    /// </summary>
    /// <param name="prefabInstance">The UI prefab.</param>
    private void HandleCloseEvent(GameObject prefabInstance)
    {
        DestroyUI(prefabInstance);
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

    /// <summary>
    /// Toggles movement speed boost.
    /// </summary>
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

    /// <summary>
    /// Destroys the UI prefab and enables the left input action.
    /// </summary>
    /// <param name="UIPrefab">The UI prefab.</param>
    private void DestroyUI(GameObject UIPrefab)
    {
        Destroy(UIPrefab);
        leftInputAction.action.Enable(); // Hack to prevent disabling of Unity input actions, which will prevent user locomotion movement.
    }
}