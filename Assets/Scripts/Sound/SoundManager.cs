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
    private AudioSource audioSource;

    public static SoundManager Instance { get; private set; }

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

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(SoundType sound, float volume = 1)
    {
        if (Instance == null || Instance.audioSource == null)
        {
            Debug.Log("NO EXISTE ALGO");
            return;
        }

        audioSource.PlayOneShot(soundList[(int)sound], volume);
    }

    public void DestroyDuplicatedAudioListeners()
    {
        AudioListener[] listeners = GameObject.FindObjectsByType<AudioListener>(FindObjectsSortMode.None);
        if (listeners.Length > 1)
        {
            foreach (AudioListener listener in listeners)
            {
                if (listener.gameObject.scene.name == "Menus")
                {
                    Destroy(listener);
                }
            }
        }
    }
}
