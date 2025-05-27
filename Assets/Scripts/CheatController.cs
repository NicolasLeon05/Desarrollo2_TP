using UnityEngine;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private InputActionReference speedCheat;
    [SerializeField] private InputActionReference flyCheat;
    [SerializeField] private InputActionReference nextLevelCheat;

    [SerializeField] private Player player;

    private Transform nextGoal;

    private void OnEnable()
    {
        if (speedCheat != null)
            speedCheat.action.performed += OnSpeedCheat;

        if (speedCheat != null)
            flyCheat.action.performed += OnFlyCheat;

        if (speedCheat != null)
            nextLevelCheat.action.performed += OnNextLevelCheat;
    }
    private void OnSpeedCheat(InputAction.CallbackContext context)
    {
        player.ApplySpeedCheat();
    }

    private void OnFlyCheat(InputAction.CallbackContext context)
    {
        player.ApplyFlyCheat();
    }

    private void OnNextLevelCheat(InputAction.CallbackContext context)
    {
        SetObjective();
    }  

    public void SetObjective()
    {
        nextGoal = GameObject.FindGameObjectWithTag("Goal").transform;
        player.ApplyNextLevelCheat(nextGoal);
    }

}
