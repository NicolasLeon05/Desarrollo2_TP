using UnityEngine;

public class GameFlowListener : MonoBehaviour
{
    [SerializeField] private NavigationController navigationController;
    private Menu baseMenu;

    private void Start()
    {
        baseMenu = navigationController.baseMenu;
    }

    private void OnEnable()
    {
        GameEvents.OnReturnToMainMenu += HandleReturnToMainMenu;
        GameEvents.OnActivateMenu += HandleActivateMenu;
    }

    private void OnDisable()
    {
        GameEvents.OnReturnToMainMenu -= HandleReturnToMainMenu;
        GameEvents.OnActivateMenu -= HandleActivateMenu;
    }

    private void HandleReturnToMainMenu()
    {
        GameManager.Instance.ResumeTime();
        GameManager.Instance.ShowCursor();
        SceneController.Instance.UnloadNonPersistentScenes();
        GameManager.Instance.SetState(GameManager.GameState.MainMenu);
    }

    private void HandleActivateMenu()
    {
        navigationController.SetMenuActive(baseMenu);
        GameManager.Instance.SetState(GameManager.GameState.MainMenu);
    }
}
