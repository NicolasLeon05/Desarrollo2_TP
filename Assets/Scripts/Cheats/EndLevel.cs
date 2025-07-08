using UnityEngine;

public class EndLevel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        SoundManager.Instance.PlaySound(SoundType.UnlockLevel);
        if (other.CompareTag("Player"))
        {
            SceneController.Instance.LoadNextAdditive();
            gameObject.SetActive(false);
        }
    }
}
