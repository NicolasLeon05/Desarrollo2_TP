using UnityEngine;

[CreateAssetMenu(fileName = "SceneAssetsReference", menuName = "ScriptableObjects/SceneAssetsReference")]
public class SceneAssets : ScriptableObject
{
    [SerializeField] private int bootSceneIndex;
    [SerializeField] private int menusSceneIndex;
    [SerializeField] private int level1SceneIndex;
    [SerializeField] private int level2SceneIndex;
    [SerializeField] private int level3SceneIndex;
    [SerializeField] private int level4SceneIndex;

    public int BootScene { get => bootSceneIndex; }
    public int MenusScene { get => menusSceneIndex; }
    public int Level1Scene { get => level1SceneIndex; }
    public int Level2Scene { get => level2SceneIndex; }
    public int Level3Scene { get => level3SceneIndex; }
    public int Level4Scene { get => level4SceneIndex; }

#if UNITYEDITOR
    [SerializeField] private SceneAsset bootScene;
    [SerializeField] private SceneAsset menusScene;
    [SerializeField] private SceneAsset level1Scene;
    [SerializeField] private SceneAsset level2Scene;
    [SerializeField] private SceneAsset level3Scene;
    [SerializeField] private SceneAsset level4Scene;

    private void OnValidate()
    {
        bootSceneIndex = SceneController.Instance.GetIndex(bootScene);
        menusSceneIndex = SceneController.Instance.GetIndex(menusScene);
        level1SceneIndex = SceneController.Instance.GetIndex(level1Scene);
        level2SceneIndex = SceneController.Instance.GetIndex(level2Scene);
        level3SceneIndex = SceneController.Instance.GetIndex(level3Scene);
        level4SceneIndex = SceneController.Instance.GetIndex(level4Scene);
    }
#endif
}
