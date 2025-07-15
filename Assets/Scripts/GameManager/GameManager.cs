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

    /// <summary>
    /// Changes the current game state
    /// </summary>
    public void SetState(GameState newState)
    {
        CurrentState = newState;
    }

    /// <summary>
    /// Destroys the instance of the player and unloads all scenes.
    /// Then loads the first scene again
    /// </summary>
    public void ResetGame()
    {
        if (Player.Instance != null)
            Destroy(Player.Instance.gameObject);

        SceneController.Instance.UnloadAllScenes();
        SceneController.Instance.LoadLevel(firstLevel);
        CurrentState = GameState.MainMenu;
    }

    /// <summary>
    /// Pauses the time by setting the timeScale to 0
    /// </summary>
    public void PauseTime()
    {
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Resumes the time by setting the timeScale to 1
    /// </summary>
    public void ResumeTime()
    {
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Locks the cursor and makes it invisible
    /// </summary>
    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// Unlocks the cursor and makes it visible
    /// </summary>
    public void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
