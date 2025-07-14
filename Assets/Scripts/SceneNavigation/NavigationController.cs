using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class NavigationController : MonoBehaviour
{

    private EventSystem eventSystem;
    private GameObject lastSelectedOption;

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject creditsMenu;
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private GameObject firstMainMenuButton;
    [SerializeField] private GameObject firstCreditsMenuButton;
    [SerializeField] private GameObject firstPauseMenuButton;

    private List<GameObject> menus;
    //HACER FUNCION QUE TOME LOS CHILDREN CON GAMEOBJECT MENU Y LOS AGREGUE A ESTA LISTA

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
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnEnable()
    {
        if (navigateAction != null)
        {
            navigateAction.action.performed += OnNavigate;
            navigateAction.action.canceled += OnNavigate;
        }
    }

    private void Update()
    {
        Debug.Log(GameManager.Instance.CurrentState);

        if (eventSystem != null)
        {
            if (eventSystem.currentSelectedGameObject == null)
            {
                if (WasNavigatePressed())
                {
                    Debug.Log("Navigate action pressed");
                    eventSystem.SetSelectedGameObject(lastSelectedOption);
                }
            }
            else if (lastSelectedOption != eventSystem.currentSelectedGameObject)
            {
                lastSelectedOption = eventSystem.currentSelectedGameObject;
                SoundManager.Instance.PlaySound(SoundType.SelectButton);
                Debug.Log("Select button sound played");
            }
        }
        else
        {
            Debug.Log("Event system is null");
        }

    }

    public void SetMenuActive(string menuName)
    {
        foreach(var menu in menus)
        {
            if (menuName == menu.name)
                menu.SetActive(true);
            else
                menu.SetActive(false);
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
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;

        mainMenu.SetActive(false);
        creditsMenu.SetActive(false);
        pauseMenu.SetActive(true);

        eventSystem.SetSelectedGameObject(firstPauseMenuButton);
        lastSelectedOption = firstPauseMenuButton;

        GameManager.Instance.SetState(GameManager.GameState.Paused);
    }

    public void SetGameplayActive()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;

        mainMenu.SetActive(false);
        creditsMenu.SetActive(false);
        pauseMenu.SetActive(false);

        GameManager.Instance.SetState(GameManager.GameState.Gameplay);
    }
}
