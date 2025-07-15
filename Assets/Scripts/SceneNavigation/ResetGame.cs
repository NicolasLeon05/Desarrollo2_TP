using UnityEngine;

public class ResetGame : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        SoundManager.Instance.PlaySound(SoundType.UnlockLevel, 0.3f);

        GameManager.Instance.ResetGame();
        gameObject.SetActive(false);
    }
}
