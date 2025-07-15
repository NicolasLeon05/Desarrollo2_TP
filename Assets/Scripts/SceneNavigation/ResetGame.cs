using UnityEngine;

public class ResetGame : MonoBehaviour
{
    /// <summary>
    /// When the player enters the trigger, plays a sound, disables the object,
    /// and calls ResetGame() from GameManager
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        SoundManager.Instance.PlaySound(SoundType.UnlockLevel, 0.3f);

        gameObject.SetActive(false);
        GameManager.Instance.ResetGame();
    }
}
