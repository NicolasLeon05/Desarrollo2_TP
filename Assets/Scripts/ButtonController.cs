using UnityEngine;

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

    public void ExitGame()
    {
        SceneController.Instance.Exit();
    }
}
