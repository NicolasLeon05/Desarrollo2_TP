using UnityEngine;

public class MenuTransitionButton : MonoBehaviour
{
    [SerializeField] private Menu targetMenu;
    [SerializeField] private NavigationController navigationController;
    [SerializeField] private GameManager.GameState stateToTransition;

    public void ActivateMenu()
    {
        if (targetMenu != null)
            navigationController.SetMenuActive(targetMenu);
        GameManager.Instance.SetState(stateToTransition);
    }

    public void SetAllInactive()
    {
        navigationController.SetAllInactive();
        GameManager.Instance.SetState(stateToTransition);
    }
}
