using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class NavigationController : MonoBehaviour
{
    [SerializeField] private EventSystem eventSystem;
    private GameObject lastSelectedOption;

    private List<Menu> menus = new();
    [SerializeField] Menu baseMenu;
    private Menu activeMenu;

    [SerializeField] private InputActionReference navigateAction;
    private Vector2 navigateInput = Vector2.zero;

    private void Awake()
    {
        eventSystem = GetComponent<EventSystem>();
        lastSelectedOption = eventSystem.firstSelectedGameObject;
        AddMenusToList();
        activeMenu = baseMenu.GetComponent<Menu>();
    }

    private void Start()
    {
        SetBaseMenuActive();

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

    void AddMenusToList()
    {
        menus.Clear();

        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<Menu>(out var menu))
                menus.Add(menu);
        }
    }

    private void SetBaseMenuActive()
    {
        SetMenuActive(baseMenu);
    }

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

    public void SetAllInactive()
    {
        foreach (var menu in menus)
            menu.gameObject.SetActive(false);
    }

    private void OnNavigate(InputAction.CallbackContext obj)
    {
        navigateInput = obj.ReadValue<Vector2>();
    }

    private bool WasNavigatePressed()
    {
        return navigateInput != Vector2.zero;
    }
}
