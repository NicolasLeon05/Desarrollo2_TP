using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    [SerializeField] private Menu pauseMenu;
    [SerializeField] private InputActionReference pauseAction;
    [SerializeField] private NavigationController navigationController;

    private bool isPaused = false;

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
        if (GameManager.Instance == null)
            return;

        var currentState = GameManager.Instance.CurrentState;

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
            GameManager.Instance.PauseTime();
            GameManager.Instance.ShowMouse();

            navigationController.SetMenuActive(pauseMenu);
            int pauseSceneBuildIndex = pauseMenu.gameObject.scene.buildIndex;
            SceneController.Instance.SetSceneActive(pauseSceneBuildIndex);
        }
        else
        {
            GameManager.Instance.ResumeTime();
            GameManager.Instance.LockMouse();

            navigationController.SetAllInactive();
            SceneController.Instance.SetSceneActive(SceneController.Instance.PreviousActiveScene.Index);
        }
    }

    public void ChangePausedState()
    {
        isPaused = !isPaused;
    }
}
