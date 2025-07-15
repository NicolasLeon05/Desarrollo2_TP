using UnityEngine;

public class MenuTransitionButton : MonoBehaviour
{
    [SerializeField] private Menu targetMenu;
    [SerializeField] private NavigationController navigationController;
    [SerializeField] private GameManager.GameState stateToTransition;

    /// <summary>
    /// Calls the SetMenuActive() function from NavigationController.
    /// Sets the GameManager state to the assigned target state
    /// </summary>
    public void ActivateMenu()
    {
        if (targetMenu != null)
            navigationController.SetMenuActive(targetMenu);
        GameManager.Instance.SetState(stateToTransition);
    }

    /// <summary>
    /// Calls the SetAllInactive() function from NavigationController.
    /// Sets the GameManager state to the assigned target state
    /// </summary>
    public void SetAllInactive()
    {
        navigationController.SetAllInactive();
        GameManager.Instance.SetState(stateToTransition);
    }
}
