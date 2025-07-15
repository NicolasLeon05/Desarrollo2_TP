using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState 
    { 
        Boot,
        MainMenu,
        Credits,
        Gameplay,
        Paused
    }
    public GameState CurrentState { get; private set; }

    [SerializeField] private Level firstLevel;

    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        CurrentState = GameState.Boot;
    }

    private void Start()
    {
        SceneController.Instance.LoadLevel(firstLevel);
        CurrentState = GameState.MainMenu;
    }


    public void SetState(GameState newState)
    {
        CurrentState = newState;
    }

    public void ResetGame()
    {
        if (Player.Instance != null)
            Destroy(Player.Instance.gameObject);

        SceneController.Instance.UnloadAllScenes();
        SceneController.Instance.LoadLevel(firstLevel);
        CurrentState = GameState.MainMenu;
    }

    public void PauseTime()
    {
        Time.timeScale = 0f;
    }

    public void ResumeTime()
    {
        Time.timeScale = 1f;
    }

    public void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ShowMouse()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
