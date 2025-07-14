using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    //[SerializeField] private SceneAssets sceneAssets;
    [SerializeField] public List<Level> levels;

    private List<SceneRef> loadedScenes;
    private List<SceneRef> persistentLoadedScenes;

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
            if (!scene.IsScenePersistent)
                UnloadSceneByIndex(scene.SceneIndex);
        }

        AddLevel(level);
    }

    public void AddLevel(Level level)
    {
        foreach (var scene in level.scenes)
        {
            LoadAdditiveById(scene.SceneIndex);
            if (scene.IsSceneActive)
                SetSceneActive(scene.SceneIndex);

            if (scene.IsScenePersistent)
                persistentLoadedScenes.Add(scene);
        }
    }

    public void LoadAdditiveById(int index)
    {
        StartCoroutine(LoadAdditiveByIdRoutine(index));
    }

    private IEnumerator LoadAdditiveByIdRoutine(int index)
    {
        if (index < SceneManager.sceneCountInBuildSettings)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
            while (!asyncLoad.isDone)
                yield return null;

            Scene newScene = SceneManager.GetSceneByBuildIndex(index);
            if (newScene.IsValid())
                SceneManager.SetActiveScene(newScene);

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
        else
        {
            if (Player.Instance != null)
                Destroy(Player.Instance.gameObject);

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);

                //MODIFICAR
                //if (scene.name != SceneManager.GetSceneByBuildIndex(sceneAssets.BootScene).name)
                //{
                //    UnloadSceneByIndex(i);
                //}
            }

            //MODIFICAR
            //AsyncOperation loadMenus = SceneManager.LoadSceneAsync(sceneAssets.MenusScene, LoadSceneMode.Additive);
            //while (!loadMenus.isDone)
            //    yield return null;
            //
            //Scene menusScene = SceneManager.GetSceneAt(sceneAssets.MenusScene);
            //if (menusScene.IsValid())
            //    SceneManager.SetActiveScene(menusScene);

            //GameManager.Instance.SetState(GameManager.GameState.MainMenu);
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
        //MODIFICAR
        //GameManager.Instance.SetState(GameManager.GameState.Gameplay);
    }


    public void SetSceneActive(int index)
    {
        Scene scene = SceneManager.GetSceneAt(index);
        SceneManager.SetActiveScene(scene);
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
