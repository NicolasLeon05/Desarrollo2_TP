using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Bootstrap : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(LoadFirstScene());
        Debug.Log("BOOTSTRAP LOADED");
    }

    private IEnumerator LoadFirstScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneIndex, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Scene firstScene = SceneManager.GetSceneByBuildIndex(nextSceneIndex);
        if (firstScene.IsValid())
        {
            SceneManager.SetActiveScene(firstScene);
        }
    }
}
