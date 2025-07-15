using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] public List<Level> levels;

    private List<SceneRef> loadedScenes = new();
    private List<SceneRef> persistentLoadedScenes = new();

    private SceneRef currentActiveScene;
    private SceneRef previousActiveScene;
    public SceneRef CurrentActiveScene => currentActiveScene;
    public SceneRef PreviousActiveScene => previousActiveScene;

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

    public void LoadLevel(Level level)
    {
        foreach (var scene in loadedScenes)
        {
            if (!scene.IsPersistent)
                UnloadSceneByIndex(scene.Index);
        }

        AddLevel(level);
    }

    public void AddLevel(Level level)
    {
        foreach (var scene in level.scenes)
        {
            if (!loadedScenes.Contains(scene))
            {
                LoadAdditiveByRef(scene);
                loadedScenes.Add(scene);
            }

            if (scene.IsPersistent && !persistentLoadedScenes.Contains(scene))
                persistentLoadedScenes.Add(scene);
        }
    }

    public void UnloadAllScenes()
    {
        foreach (var scene in loadedScenes)
        {
            UnloadSceneByIndex(scene.Index);
        }

        loadedScenes.Clear();
        persistentLoadedScenes.Clear();
    }

    public void UnloadNonPersistentScenes()
    {
        foreach (var scene in loadedScenes)
        {
            if (!scene.IsPersistent)
                UnloadSceneByIndex(scene.Index);
        }

        loadedScenes.RemoveAll(scene => !scene.IsPersistent);
    }

    public void LoadAdditiveByRef(SceneRef scene)
    {
        StartCoroutine(LoadAdditiveByRefRoutine(scene));
    }

    private IEnumerator LoadAdditiveByRefRoutine(SceneRef scene)
    {
        if (scene.Index < SceneManager.sceneCountInBuildSettings)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene.Index, LoadSceneMode.Additive);
            while (!asyncLoad.isDone)
                yield return null;

            Scene newScene = SceneManager.GetSceneByBuildIndex(scene.Index);
            if (newScene.IsValid() && scene.IsActive)
                SetSceneActive(scene.Index);


            SoundManager.Instance.DestroyDuplicatedAudioListeners();
            //GameManager.Instance.SetState(GameManager.GameState.Gameplay);
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
            //GameManager.Instance.SetState(GameManager.GameState.Gameplay);
        }
    }

    public void UnloadSceneByIndex(int index)
    {
        StartCoroutine(UnloadSceneByIndexRoutine(index));
    }

    private IEnumerator UnloadSceneByIndexRoutine(int index)
    {
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(index);
        while (!asyncUnload.isDone)
            yield return null;
    }


    public void SetSceneActive(int index)
    {
        Scene scene = SceneManager.GetSceneByBuildIndex(index);
        if (scene.IsValid())
        {
            SceneManager.SetActiveScene(scene);

            SceneRef sceneRef = loadedScenes.Find(s => s.Index == index);
            if (sceneRef != null)
            {
                previousActiveScene = currentActiveScene;
                currentActiveScene = sceneRef;
            }
        }
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
