using UnityEngine;

[CreateAssetMenu(fileName = "SceneRef", menuName = "ScriptableObjects/SceneRef")]
public class SceneRef : ScriptableObject
{
    [SerializeField] private int index;
    [SerializeField] private bool isActive;
    [SerializeField] private bool isPersistent;

    public int Index { get => index; }
    public bool IsActive { get => isActive; }
    public bool IsPersistent { get => isPersistent; }
}
