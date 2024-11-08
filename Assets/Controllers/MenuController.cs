using UnityEngine;


public class MenuController : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject createIssue;
    public Transform cameraTransform;
    public InputHandler inputHandler;
    private float spawnDistance = 4f;
    private GameObject currentMainMenu;
    private GameObject currentCreateIssue;
    private MainMenuEventHandler mainMenuEventHandler;
    private CreateIssueEventHandler createIssueEventHandler;
    private Vector3 _spawnPosition;
    private FlagController flagHandler;

    private void Start()
    {
        flagHandler = GetComponent<FlagController>();
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
            }
        }
        else
        {
            mainMenuEventHandler = currentMainMenu.GetComponentInChildren<MainMenuEventHandler>();

            if (mainMenuEventHandler != null)
            {
                mainMenuEventHandler.createIssueButtonPressed -= OnCreateIssueButtonPress;
            }

            Destroy(currentMainMenu);
            currentMainMenu.SetActive(false);
        }
    }

    private void SpawnUI(GameObject menuPrefab)
    {
        _spawnPosition = cameraTransform.position + cameraTransform.forward * spawnDistance;

        if (menuPrefab == mainMenu)
        {
            currentMainMenu = Instantiate(mainMenu, _spawnPosition, Quaternion.identity);
            currentMainMenu.transform.LookAt(cameraTransform);
            currentMainMenu.transform.rotation = Quaternion.LookRotation(currentMainMenu.transform.position - cameraTransform.position);
        }
        else if (menuPrefab == createIssue)
        {
            currentCreateIssue = Instantiate(createIssue, _spawnPosition, Quaternion.identity);
            currentCreateIssue.transform.LookAt(cameraTransform);
            currentCreateIssue.transform.rotation = Quaternion.LookRotation(currentCreateIssue.transform.position - cameraTransform.position);
        }
    }

    private void OnCreateIssueButtonPress()
    {
        ToggleMenu();
        SpawnUI(createIssue);
        currentCreateIssue.SetActive(true);

        createIssueEventHandler = currentCreateIssue.GetComponentInChildren<CreateIssueEventHandler>();

        if (createIssueEventHandler != null)
        {
            createIssueEventHandler.OnCreateIssue += CreateIssue;
        }
    }

    private void CreateIssue(string subject, string dueDate, string assignedTo, string description)
    {
        flagHandler.SpawnFlag(subject, dueDate, assignedTo, description);
    }
}