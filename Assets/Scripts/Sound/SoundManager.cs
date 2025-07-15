using UnityEngine;

public enum SoundType
{
    SelectButton,
    ClickButton,
    Run,
    Jump,
    Land,
    Dash,
    UnlockLevel
}


[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList;
    [SerializeField] private SceneRef sceneToDestroyListenerFrom;
    private AudioSource audioSource;

    public static SoundManager Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the SoundManager.
    /// Destroys duplicates and persists across scenes
    /// </summary>
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Gets the AudioSource component used to play sounds
    /// </summary>
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Plays a one-shot sound from the sound list based on the given type and volume
    /// </summary>
    public void PlaySound(SoundType sound, float volume = 1)
    {
        audioSource.PlayOneShot(soundList[(int)sound], volume);
    }

    /// <summary>
    /// Destroys all AudioListeners in the scene except for the one
    /// belonging to the scene defined as the reference
    /// </summary>
    public void DestroyDuplicatedAudioListeners()
    {
        AudioListener[] listeners = FindObjectsByType<AudioListener>(FindObjectsSortMode.None);
        if (listeners.Length > 1)
        {
            foreach (AudioListener listener in listeners)
            {
                if (listener.gameObject.scene.buildIndex != sceneToDestroyListenerFrom.Index)
                    Destroy(listener);
            }
        }
    }
}
