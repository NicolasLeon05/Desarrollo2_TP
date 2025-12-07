using UnityEngine;

public class SceneTransitionButton : MonoBehaviour
{
    [SerializeField] private Level levelToLoad;
    private SceneController sceneController;


    private void Awake()
    {
        ServiceProvider.TryGetService(out sceneController);
    }

    /// <summary>
    /// Loads the assigned level, replacing current non-persistent scenes
    /// </summary>
    public void LoadLevel()
    {
        sceneController.LoadLevel(levelToLoad);
    }

    /// <summary>
    /// Adds the assigned level to the current set of loaded scenes
    /// </summary>
    public void AddLevel()
    {
        sceneController.AddLevel(levelToLoad);
    }

    /// <summary>
    /// Exits the game
    /// </summary>
    public void ExitGame()
    {
        sceneController.Exit();
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