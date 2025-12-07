using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] public List<Level> levels;
    [SerializeField] private Level bootLevel;

    private List<SceneRef> loadedScenes = new();
    private List<SceneRef> persistentLoadedScenes = new();

    private SceneRef currentActiveScene;
    private SceneRef previousActiveScene;
    public SceneRef CurrentActiveScene => currentActiveScene;
    public SceneRef PreviousActiveScene => previousActiveScene;


    /// <summary>
    /// Initializes the singleton instance of the SceneController.
    /// Destroys duplicates and persists across scenes.
    /// </summary>
    private void Awake()
    {
        ServiceProvider.SetService(this, true);

        SaveBootScenes();
    }

    /// <summary>
    /// Loads the given level and unloads all non-persistent scenes currently loaded.
    /// </summary>
    public void LoadLevel(Level level)
    {
        AddLevel(level);

        foreach (var scene in loadedScenes)
        {
            if (!scene.IsPersistent && !level.scenes.Contains(scene))
                UnloadSceneByIndex(scene.Index);
        }

    }

    /// <summary>
    /// Loads all scenes in the level additively if they aren't already loaded.
    /// Stores all scenes in private loadedScenes list
    /// Stores persistent scenes in private persistentLoadedScenes list
    /// </summary>
    public void AddLevel(Level level)
    {
        foreach (var scene in level.scenes)
        {
            if (!loadedScenes.Contains(scene) && !persistentLoadedScenes.Contains(scene))
            {
                LoadAdditiveByRef(scene);
                loadedScenes.Add(scene);

                if (scene.IsPersistent)
                    persistentLoadedScenes.Add(scene);
            }
        }
    }

    /// <summary>
    /// Unloads all scenes in the stack and clears the private lists
    /// </summary>
    public void UnloadAllScenes()
    {
        var scenesToUnload = new List<SceneRef>(loadedScenes);

        foreach (var scene in scenesToUnload)
        {
            if (scene.Index != SceneManager.GetActiveScene().buildIndex)
                UnloadSceneByIndex(scene.Index);
        }

        loadedScenes.Clear();
        persistentLoadedScenes.Clear();
    }

    /// <summary>
    /// Finds a persistent scene and sets it active.
    /// Then unloads all the non persitent ones
    /// </summary>
    public void UnloadNonPersistentScenes()
    {
        foreach (var scene in loadedScenes)
        {
            if (scene.IsActive)
            {
                SetSceneActive(scene.Index);
                break;
            }
        }

        foreach (var scene in loadedScenes)
        {
            if (!scene.IsPersistent)
                UnloadSceneByIndex(scene.Index);
        }

        loadedScenes.RemoveAll(scene => !scene.IsPersistent);
    }

    /// <summary>
    /// Save the scenes in the boot level
    /// </summary>
    public void SaveBootScenes()
    {
        foreach (var scene in bootLevel.scenes)
        {
            if (!loadedScenes.Contains(scene) && !persistentLoadedScenes.Contains(scene))
                loadedScenes.Add(scene);

            if (scene.IsPersistent && !persistentLoadedScenes.Contains(scene))
                persistentLoadedScenes.Add(scene);
        }
    }

    /// <summary>
    /// Starts coroutine to load a scene additively from a SceneRef
    /// </summary>
    public void LoadAdditiveByRef(SceneRef scene)
    {
        StartCoroutine(LoadAdditiveByRefRoutine(scene));
    }

    /// <summary>
    /// Coroutine that loads a scene additively and sets it active if needed.
    /// Also ensures duplicated AudioListeners are destroyed
    /// </summary>
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
        }
    }

    /// <summary>
    /// Starts coroutine to unload a scene by its build index
    /// </summary>
    public void UnloadSceneByIndex(int index)
    {
        StartCoroutine(UnloadSceneByIndexRoutine(index));
    }

    /// <summary>
    /// Coroutine that unloads a scene asynchronously by index
    /// </summary>
    private IEnumerator UnloadSceneByIndexRoutine(int index)
    {
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(index);
        while (!asyncUnload.isDone)
            yield return null;
    }

    /// <summary>
    /// Sets the scene with the given build index as the active scene,
    /// and updates the current and previous active SceneRefs
    /// </summary>
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

    /// <summary>
    /// Exits the application. Stops play mode if running in the Unity Editor
    /// </summary>
    public void Exit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
