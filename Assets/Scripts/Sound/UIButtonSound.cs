using UnityEngine;

public class UIButtonSound : MonoBehaviour
{
    [SerializeField] private SoundType sound;
    [SerializeField, Range(0, 1)] private float volume = 1f;

    public void PlaySound()
    {
        SoundManager.Instance.PlaySound(sound, volume);
    }
}