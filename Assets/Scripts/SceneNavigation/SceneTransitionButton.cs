using UnityEngine;

public class SceneTransitionButton : MonoBehaviour
{
    [SerializeField] private Level levelToLoad;

    /// <summary>
    /// Loads the assigned level, replacing current non-persistent scenes
    /// </summary>
    public void LoadLevel()
    {
        SceneController.Instance.LoadLevel(levelToLoad);
    }

    /// <summary>
    /// Adds the assigned level to the current set of loaded scenes
    /// </summary>
    public void AddLevel()
    {
        SceneController.Instance.AddLevel(levelToLoad);
    }

    /// <summary>
    /// Exits the game
    /// </summary>
    public void ExitGame()
    {
        SceneController.Instance.Exit();
    }

    /// <summary>
    /// Unloads all non-persistent scenes and transitions back to the main menu
    /// Also resumes time and shows the cursor.
    /// </summary>
    public void ReturnToMainMenu()
    {
        GameEvents.TriggerReturnToMainMenu();
    }
}