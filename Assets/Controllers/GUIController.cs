using Dummiesman;
using UnityEngine;

// Responsible for controlling graphical elements and events from UI.
public class GUIController : MonoBehaviour
{
    // Referenced in the unity editor inspector
    [SerializeField]
    private GameObject mainMenuPrefab;
    [SerializeField]
    private GameObject createIssuePrefab;
    [SerializeField]
    private GameObject selectModelPrefab;
    [SerializeField]
    private GameObject flagPrefab;
    [SerializeField]
    private GameObject plane;
    [SerializeField]
    private Material material;
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private InputHandler inputHandler;

    // current 3D objects
    private GameObject currentMainMenu;
    private GameObject currentCreateIssue;
    private GameObject currentSelectModel;
    private GameObject currentModel;

    // Event handlers
    private MainMenuEventHandler mainMenuEventHandler;
    private CreateIssueEventHandler createIssueEventHandler;
    private SelectModelEventHandler selectModelEventHandler;

    private ModelController modelController;
    private float spawnDistance = 4f;
    private string currentModelName;

    // Called when the scene initializes.
    private void Start()
    {
        modelController = GetComponent<ModelController>();
        if (inputHandler != null)
        {
            inputHandler.OnMenuButtonPressed += ToggleMenu;
        }

        if (mainMenuPrefab != null)
        {
            mainMenuPrefab.SetActive(false);
        }
    }

    // Toggles the main menu UI and subscribes/unsubscribes to menu events.
    private void ToggleMenu()
    {
        if (currentMainMenu == null)
        {
            SpawnUI(mainMenuPrefab);
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

    // Spawns UI object in the scene.
    private void SpawnUI(GameObject UIPrefab)
    {
        Vector3 spawnPosition = cameraTransform.position + cameraTransform.forward * spawnDistance;

        if (UIPrefab == mainMenuPrefab)
        {
            currentMainMenu = Instantiate(mainMenuPrefab, spawnPosition, Quaternion.identity);
            currentMainMenu.transform.LookAt(cameraTransform);
            currentMainMenu.transform.rotation = Quaternion.LookRotation(currentMainMenu.transform.position - cameraTransform.position);
        }
        else if (UIPrefab == createIssuePrefab)
        {
            currentCreateIssue = Instantiate(createIssuePrefab, spawnPosition, Quaternion.identity);
            currentCreateIssue.transform.LookAt(cameraTransform);
            currentCreateIssue.transform.rotation = Quaternion.LookRotation(currentCreateIssue.transform.position - cameraTransform.position);
        }
        else if (UIPrefab == selectModelPrefab)
        {
            currentSelectModel = Instantiate(selectModelPrefab, spawnPosition, Quaternion.identity);
            currentSelectModel.transform.LookAt(cameraTransform);
            currentSelectModel.transform.rotation = Quaternion.LookRotation(currentSelectModel.transform.position - cameraTransform.position);
        }
        UIPrefab.SetActive(true);
    }

    // Runs when create issue button is pressed in the main menu UI.
    private void OnCreateIssueButtonPress()
    {
        Destroy(currentMainMenu);
        SpawnUI(createIssuePrefab);

        createIssueEventHandler = currentCreateIssue.GetComponentInChildren<CreateIssueEventHandler>();

        if (createIssueEventHandler != null)
        {
            createIssueEventHandler.OnCreateIssue += CreateIssue;
        }
    }

    // Runs when create issue button is pressed in the main menu UI.
    private void OnSelectModelButtonPress()
    {
        Destroy(currentMainMenu);
        SpawnUI(selectModelPrefab);
        selectModelEventHandler = currentSelectModel.GetComponentInChildren<SelectModelEventHandler>();
        if (selectModelEventHandler != null)
        {
            selectModelEventHandler.OnSelectModel += LoadModel;
        }
    }

    // Runs when the create issue button is pressed in the create issue UI.
    private void CreateIssue(string subject, string dueDate, string assignedTo, string description)
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
            Instantiate(flagPrefab, spawnPosition, Quaternion.identity);
            modelController.AddFlagToModel(subject, dueDate, assignedTo, description, spawnPosition, currentModelName);
        }
    }

    // Runs when select button is pressed in the select model UI.
    // Destroys all flags in the scene.
    // Imports model in runtime with OBJLoader.
    // Instantiates flags in the loaded model to the scene.
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

    // Moves the model down to the plane in the scene.
    private void SnapToPlane(GameObject obj)
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

    // Assigns one material to the 3D model.
    private void AssignMaterialToObj(GameObject obj)
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

    // Destroys all flags in the scene.
    private void DestroyFlags()
    {
        GameObject[] flags = GameObject.FindGameObjectsWithTag("Flag");
        foreach (var flag in flags)
        {
            Destroy(flag);
        }
    }

    // Instantiates flags from the model in the scene.
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