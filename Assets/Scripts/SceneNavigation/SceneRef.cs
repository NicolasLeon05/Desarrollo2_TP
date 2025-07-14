using UnityEngine;

[CreateAssetMenu(fileName = "SceneRef", menuName = "ScriptableObjects/SceneRef")]
public class SceneRef : ScriptableObject
{
    [SerializeField] private int index;
    [SerializeField] private bool isActive;
    [SerializeField] private bool isPersistent;

    public int SceneIndex { get => index; }
    public bool IsSceneActive { get => isActive; }
    public bool IsScenePersistent { get => isPersistent; }
}
