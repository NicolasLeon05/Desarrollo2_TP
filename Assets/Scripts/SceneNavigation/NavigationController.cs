using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class NavigationController : MonoBehaviour
{
    [SerializeField] private EventSystem eventSystem;
    private GameObject lastSelectedOption;

    private List<Menu> menus = new();
    [SerializeField] public Menu baseMenu;
    private Menu activeMenu;

    [SerializeField] private InputActionReference navigateAction;
    private Vector2 navigateInput = Vector2.zero;


    private GameManager gameManager;
    /// <summary>
    /// Initializes the event system, adds all child menus to the list,
    /// sets the base menu as active, and stores the first selected button
    /// </summary>
    private void Awake()
    {
        ServiceProvider.TryGetService(out gameManager);
        eventSystem = GetComponent<EventSystem>();
        lastSelectedOption = eventSystem.firstSelectedGameObject;
        AddMenusToList();
        activeMenu = baseMenu.GetComponent<Menu>();
    }

    /// <summary>
    /// Sets the base menu as the currently active menu and unlocks the cursor
    /// </summary>
    private void Start()
    {
        SetBaseMenuActive();
        gameManager.ShowCursor();
    }

    /// <summary>
    /// Subscribes to the navigation input action
    /// </summary>
    private void OnEnable()
    {
        if (navigateAction != null)
        {
            navigateAction.action.performed += OnNavigate;
            navigateAction.action.canceled += OnNavigate;
        }
    }

    /// <summary>
    /// Checks current selection in the event system.
    /// If nothing is selected and input was pressed, re-selects the last valid option.
    /// Plays sound if a new option is selected
    /// </summary>
    private void Update()
    {
        Debug.Log(gameManager.CurrentState);

        if (eventSystem != null)
        {
            if (eventSystem.currentSelectedGameObject == null)
            {
                if (WasNavigatePressed())
                    eventSystem.SetSelectedGameObject(lastSelectedOption);
            }
            else if (lastSelectedOption != eventSystem.currentSelectedGameObject)
            {
                lastSelectedOption = eventSystem.currentSelectedGameObject;
                SoundManager.Instance.PlaySound(SoundType.SelectButton);
            }
        }
        else
        {
            Debug.Log("Event system is null");
        }
    }

    /// <summary>
    /// Searches for all Menu components under this object and adds them to the internal list
    /// </summary>
    void AddMenusToList()
    {
        menus.Clear();

        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<Menu>(out var menu))
                menus.Add(menu);
        }
    }

    /// <summary>
    /// Activates the base menu defined in the inspector
    /// </summary>
    private void SetBaseMenuActive()
    {
        SetMenuActive(baseMenu);
    }

    /// <summary>
    /// Activates the specified menu and deactivates the rest.
    /// Sets focus on the first button of the active menu
    /// </summary>
    public void SetMenuActive(Menu menuToActivate)
    {
        foreach (var menu in menus)
        {
            bool isActive = menu == menuToActivate;
            menu.gameObject.SetActive(isActive);

            if (isActive)
            {
                activeMenu = menu;
                eventSystem.SetSelectedGameObject(menu.firstButton);
            }
            else
                menu.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Deactivates all menus under this object
    /// </summary>
    public void SetAllInactive()
    {
        foreach (var menu in menus)
            menu.gameObject.SetActive(false);
    }

    /// <summary>
    /// Reads the navigation input direction and stores it
    /// </summary>
    private void OnNavigate(InputAction.CallbackContext obj)
    {
        navigateInput = obj.ReadValue<Vector2>();
    }

    /// <summary>
    /// Returns true if a directional input is currently being pressed
    /// </summary>
    private bool WasNavigatePressed()
    {
        return navigateInput != Vector2.zero;
    }
}
