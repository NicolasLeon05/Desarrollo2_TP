using UnityEngine;

public class MenuTransitionButton : MonoBehaviour
{
    [SerializeField] private Menu targetMenu;
    [SerializeField] private NavigationController navigationController;
    [SerializeField] private GameManager.GameState stateToTransition;

  
    public void ActivateBaseMenu()
    {
        GameEvents.TriggerActivateBaseMenu();
    }

    /// <summary>
    /// Triggers an event that sets the targetMenu as active
    /// and the current state as the stateToTransition
    /// </summary>
    public void ActivateMenu()
    {
        if (targetMenu != null)
            GameEvents.TriggerActivateMenu(targetMenu, stateToTransition);
    }

    /// <summary>
    /// Calls the SetAllInactive() function from NavigationController.
    /// Sets the GameManager state to the assigned target state
    /// </summary>
    public void SetAllInactive()
    {
        GameEvents.TriggerSetAllMenusInactive();
        GameManager.Instance.SetState(stateToTransition);
    }
}
