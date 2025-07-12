using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }
    [SerializeField] private SceneAssets sceneAssets;


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
                if (scene.name != SceneManager.GetSceneByBuildIndex(sceneAssets.BootScene).name)
                {
                    UnloadSceneByIndex(i);
                }
            }

            AsyncOperation loadMenus = SceneManager.LoadSceneAsync(sceneAssets.MenusScene, LoadSceneMode.Additive);
            while (!loadMenus.isDone)
                yield return null;

            Scene menusScene = SceneManager.GetSceneAt(sceneAssets.MenusScene);
            if (menusScene.IsValid())
                SceneManager.SetActiveScene(menusScene);

            GameManager.Instance.SetState(GameManager.GameState.MainMenu);
        }

    }

    public void LoadDefaultScene()
    {
        StartCoroutine(LoadDefaultSceneAdditiveRoutine());
    }

    private IEnumerator LoadDefaultSceneAdditiveRoutine()
    {
        int defaultSceneIndex = sceneAssets.MenusScene;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(defaultSceneIndex, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
            yield return null;

        Scene defaultScene = SceneManager.GetSceneByBuildIndex(defaultSceneIndex);
        if (defaultScene.IsValid())
            SceneManager.SetActiveScene(defaultScene);

        SoundManager.Instance.DestroyDuplicatedAudioListeners();
        GameManager.Instance.SetState(GameManager.GameState.MainMenu);
    }

    public void UnloadSceneByIndex(int index)
    {
        SceneManager.UnloadSceneAsync(index);
    }

    public void Exit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

#if UNITY_EDITOR
    public int GetIndex(SceneAsset asset)
    {
        if (!asset)
            return 0;

        return SceneUtility.GetBuildIndexByScenePath(AssetDatabase.GetAssetPath(asset));
    }
#endif
}
