using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadNext()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            SceneManager.LoadScene(0);
            GameManager.Instance.SetState(GameManager.GameState.Boot);
        }
    }

    public void LoadNextAdditive()
    {
        StartCoroutine(LoadNextAdditiveRoutine());
    }

    private IEnumerator LoadNextAdditiveRoutine()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextIndex, LoadSceneMode.Additive);
            while (!asyncLoad.isDone)
                yield return null;

            Scene newScene = SceneManager.GetSceneByBuildIndex(nextIndex);
            if (newScene.IsValid())
                SceneManager.SetActiveScene(newScene);

            SoundManager.Instance.DestroyDuplicatedAudioListeners();
            GameManager.Instance.SetState(GameManager.GameState.Gameplay);
        }
        else
        {
            if (Player.Instance != null)
                Destroy(Player.Instance.gameObject);

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.name != SceneManager.GetSceneByBuildIndex(0).name)
                {
                    SceneManager.UnloadSceneAsync(scene);
                }
            }

            AsyncOperation loadMenus = SceneManager.LoadSceneAsync("Menus", LoadSceneMode.Additive);
            while (!loadMenus.isDone)
                yield return null;

            Scene menusScene = SceneManager.GetSceneByName("Menus");
            if (menusScene.IsValid())
                SceneManager.SetActiveScene(menusScene);

            GameManager.Instance.SetState(GameManager.GameState.Menus);
        }

    }

    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        GameManager.Instance.SetState(sceneName == "Menus" ? GameManager.GameState.Menus : GameManager.GameState.Gameplay);
    }

    public void LoadSceneByNameAdditive(string sceneName)
    {
        StartCoroutine(LoadSceneAdditiveAndSetActive(sceneName));
    }

    private IEnumerator LoadSceneAdditiveAndSetActive(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
            yield return null;

        Scene newScene = SceneManager.GetSceneByName(sceneName);
        if (newScene.IsValid())
            SceneManager.SetActiveScene(newScene);

        GameManager.Instance.SetState(GameManager.GameState.Gameplay);
    }

    public void Exit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
