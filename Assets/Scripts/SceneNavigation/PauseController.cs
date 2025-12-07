using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    [SerializeField] private Menu pauseMenu;
    [SerializeField] private InputActionReference pauseAction;
    [SerializeField] private NavigationController navigationController;

    private bool isPaused = false;
    private SceneController sceneController;
    private GameManager gameManager;

    private void Awake()
    {
        ServiceProvider.TryGetService(out sceneController);
        ServiceProvider.TryGetService(out gameManager);
    }
    private void OnEnable()
    {
        if (pauseAction != null)
        {
            pauseAction.action.performed += OnPause;
        }
    }

    private void OnDisable()
    {
        if (pauseAction != null)
        {
            pauseAction.action.performed -= OnPause;
        }
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        if (gameManager == null)
            return;

        var currentState = gameManager.CurrentState;

        if (currentState == GameManager.GameState.Gameplay || currentState == GameManager.GameState.Paused)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        ChangePausedState();

        if (isPaused)
        {
            gameManager.PauseTime();
            gameManager.ShowCursor();
            gameManager.SetState(GameManager.GameState.Paused);

            navigationController.SetMenuActive(pauseMenu);
            int pauseSceneBuildIndex = pauseMenu.gameObject.scene.buildIndex;
            sceneController.SetSceneActive(pauseSceneBuildIndex);
        }
        else
        {
            gameManager.ResumeTime();
            gameManager.LockCursor();
            gameManager.SetState(GameManager.GameState.Gameplay);

            navigationController.SetAllInactive();
            sceneController.SetSceneActive(sceneController.PreviousActiveScene.Index);
        }
    }

    public void ChangePausedState()
    {
        isPaused = !isPaused;
    }
}
