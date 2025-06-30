using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{

    private EventSystem eventSystem;
    private GameObject lastSelectedGameObject;

    [SerializeField] private InputActionReference navigateAction;
    private Vector2 navigateInput = Vector2.zero;


    private void Awake()
    {
        eventSystem = GetComponent<EventSystem>();
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

            Debug.Log(navigateInput);


            if (eventSystem.currentSelectedGameObject == null)
                if (WasNavigatePressed())
                    eventSystem.SetSelectedGameObject(lastSelectedGameObject);
           

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
}
