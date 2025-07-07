using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private InputActionReference pauseAction;
    [SerializeField] private NavigationController navigationController;

    private bool isPaused = false;

    private void Awake()
    {
        Debug.Log("Active CameraController: " + name + " in scene: " + gameObject.scene.name);
        pauseMenu.SetActive(false);
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
        if (GameManager.Instance == null)
            return;

        GameManager.GameState currentState = GameManager.Instance.CurrentState;

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
            navigationController.SetPauseActive();
            string sceneName = gameObject.scene.name;
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        }
        else
        {
            navigationController.SetGameplayActive();
            int highestSceneIndex = SceneManager.sceneCount - 1;
            SceneManager.SetActiveScene(SceneManager.GetSceneAt(highestSceneIndex));
        }
    }

    public void ChangePausedState()
    {
        isPaused = !isPaused;
    }
}
