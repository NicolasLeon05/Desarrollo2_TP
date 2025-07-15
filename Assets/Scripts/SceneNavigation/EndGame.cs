using UnityEngine;

public class EndGame : MonoBehaviour
{

    /// <summary>
    /// When the player enters the trigger, plays a sound, disables the object,
    /// and triggers the Victort Event
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        SoundManager.Instance.PlaySound(SoundType.UnlockLevel, 0.3f);
        gameObject.SetActive(false);

        GameEvents.TriggerVictory();
    }
}
