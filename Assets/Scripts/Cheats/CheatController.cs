using UnityEngine;
using UnityEngine.InputSystem;

public class CheatController : MonoBehaviour
{
    [SerializeField] private InputActionReference speedCheat;
    [SerializeField] private InputActionReference flyCheat;
    [SerializeField] private InputActionReference nextLevelCheat;

    private Player player;

    private void Start()
    {
        ServiceProvider.TryGetService(out player);
    }

    private void OnEnable()
    {
        if (speedCheat != null)
            speedCheat.action.performed += OnSpeedCheat;

        if (speedCheat != null)
            flyCheat.action.performed += OnFlyCheat;

        if (speedCheat != null)
            nextLevelCheat.action.performed += OnNextLevelCheat;
    }

    /// <summary>
    /// Applies increased speed cheat to player when corresponding input is pressed
    /// </summary>
    private void OnSpeedCheat(InputAction.CallbackContext context)
    {
        player.ApplySpeedCheat();
    }

    /// <summary>
    /// Applies fly cheat to player when corresponding input is pressed
    /// </summary>
    private void OnFlyCheat(InputAction.CallbackContext context)
    {
        player.ApplyFlyCheat();
    }

    /// <summary>
    /// Applies cheat that auto clears the current level when corresponding input is pressed
    /// </summary>
    private void OnNextLevelCheat(InputAction.CallbackContext context)
    {
        SetObjective();
    }

    /// <summary>
    /// Teleports player to the goal of the current level when corresponding input is pressed
    /// </summary>
    public void SetObjective()
    {
        GoalPoint goal;
        if (ServiceProvider.TryGetService(out goal))
        {
            player.ApplyTeleportCheat(goal.transform);
        }
    }

}
