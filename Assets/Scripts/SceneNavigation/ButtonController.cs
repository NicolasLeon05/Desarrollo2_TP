using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonController : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        SceneController.Instance.LoadSceneByName(sceneName);
    }

    public void LoadNext()
    {
        SceneController.Instance.LoadNext();
    }

    public void LoadNextAdditive()
    {
        SceneController.Instance.LoadNextAdditive();
    }

    public void ExitGame()
    {
        SceneController.Instance.Exit();
    }

    public void ReturnToMainMenu()
    {
        string sceneName = gameObject.scene.name;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name != sceneName && scene.name != SceneManager.GetSceneByBuildIndex(0).name)
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }
    }
}