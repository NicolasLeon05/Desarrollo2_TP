using UnityEngine;

public class GameManager : MonoBehaviour
{

    public enum GameState 
    { 
        Boot,
        MainMenu,
        Gameplay,
        Paused
    }
    public GameState CurrentState { get; private set; }

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

    public void SetState(GameState newState)
    {
        CurrentState = newState;
        //Debug.Log("Game state set to: " + newState);
    }
}
