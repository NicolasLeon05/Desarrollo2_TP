using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(GrappleSystem))]
public class GrappleController : MonoBehaviour
{
    private GrappleSystem grapple;

    [SerializeField] private InputActionReference grappleFireAction;
    [SerializeField] private InputActionReference grappleCancelAction;

    private void Awake()
    {
        grapple = GetComponent<GrappleSystem>();
    }

    private void OnEnable()
    {
        grappleFireAction.action.performed += OnFire;
        grappleCancelAction.action.performed += OnCancel;
    }

    private void OnDisable()
    {
        grappleFireAction.action.performed -= OnFire;
        grappleCancelAction.action.performed -= OnCancel;
    }

    private void OnFire(InputAction.CallbackContext ctx)
    {
        grapple.Fire();
    }

    private void OnCancel(InputAction.CallbackContext ctx)
    {
        grapple.Cancel();
    }
}
