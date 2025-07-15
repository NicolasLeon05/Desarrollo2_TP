using UnityEngine;

public class SceneTransitionButton : MonoBehaviour
{
    [SerializeField] private Level levelToLoad;

    public void LoadLevel()
    {
        SceneController.Instance.LoadLevel(levelToLoad);
    }

    public void AddLevel()
    {
        SceneController.Instance.AddLevel(levelToLoad);
    }

    public void ExitGame()
    {
        SceneController.Instance.Exit();
    }

    public void ReturnToMainMenu()
    {
        GameManager.Instance.ResumeTime();
        GameManager.Instance.ShowMouse();

        SceneController.Instance.UnloadNonPersistentScenes();
        GameManager.Instance.SetState(GameManager.GameState.MainMenu);
    }
}