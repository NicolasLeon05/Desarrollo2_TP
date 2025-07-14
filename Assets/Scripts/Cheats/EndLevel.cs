using UnityEngine;

public class EndLevel : MonoBehaviour
{
    [SerializeField] private Level level;
    private void OnTriggerEnter(Collider other)
    {
        SoundManager.Instance.PlaySound(SoundType.UnlockLevel, 0.3f);

        SceneController.Instance.AddLevel(level);
        gameObject.SetActive(false);
    }
}