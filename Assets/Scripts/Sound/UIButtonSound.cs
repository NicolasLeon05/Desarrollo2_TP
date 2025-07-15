using UnityEngine;

public class UIButtonSound : MonoBehaviour
{
    [SerializeField] private SoundType sound;
    [SerializeField, Range(0, 1)] private float volume = 1f;

    /// <summary>
    /// Plays a UI sound using the SoundManager with the selected volume and type
    /// </summary>
    public void PlaySound()
    {
        SoundManager.Instance.PlaySound(sound, volume);
    }
}