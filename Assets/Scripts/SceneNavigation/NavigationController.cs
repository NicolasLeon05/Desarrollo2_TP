using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class NavigationController : MonoBehaviour //SelectionKeeper
{

    private EventSystem eventSystem;
    private GameObject lastSelectedOption;

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject creditsMenu;
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private GameObject firstMainMenuButton;
    [SerializeField] private GameObject firstCreditsMenuButton;
    [SerializeField] private GameObject firstPauseMenuButton;

    [SerializeField] private InputActionReference navigateAction;
    private Vector2 navigateInput = Vector2.zero;


    private void Awake()
    {
        eventSystem = GetComponent<EventSystem>();
        lastSelectedOption = eventSystem.firstSelectedGameObject;
    }

    private void Start()
    {
        mainMenu.SetActive(true);
        creditsMenu.SetActive(false);
        pauseMenu.SetActive(false);  
    }

    private void OnEnable()
    {
        if (navigateAction != null)
        {
            navigateAction.action.performed += OnNavigate;
            navigateAction.action.canceled += OnNavigate;
        }
    }

    void Update()
    {
        if (eventSystem != null)
        {
            //Debug.Log(eventSystem.currentSelectedGameObject);

            Debug.Log(lastSelectedOption);


            if (eventSystem.currentSelectedGameObject == null)
            {
                if (WasNavigatePressed())
                    eventSystem.SetSelectedGameObject(lastSelectedOption);
            }
            else
                lastSelectedOption = eventSystem.currentSelectedGameObject;

        }
        else
        {
            Debug.Log("Event system is null");
        }

    }
    private void OnNavigate(InputAction.CallbackContext obj)
    {
        navigateInput = obj.ReadValue<Vector2>();
    }

    private bool WasNavigatePressed()
    {
        return navigateInput != Vector2.zero;
    }

    public void SetMainMenuActive()
    {
        mainMenu.SetActive(true);
        creditsMenu.SetActive(false);
        pauseMenu.SetActive(false);

        eventSystem.SetSelectedGameObject(firstMainMenuButton);
        lastSelectedOption = firstMainMenuButton;
    }

    public void SetCreditsActive()
    {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
        pauseMenu.SetActive(false);

        eventSystem.SetSelectedGameObject(firstCreditsMenuButton);
        lastSelectedOption = firstCreditsMenuButton;
    }

    public void SetPauseActive()
    {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(false);
        pauseMenu.SetActive(true);

        eventSystem.SetSelectedGameObject(firstPauseMenuButton);
        lastSelectedOption = firstPauseMenuButton;
    }
}
