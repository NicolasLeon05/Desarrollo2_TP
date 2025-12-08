using UnityEngine;

public class AudioListenerService : MonoBehaviour
{
    private void Awake()
    {
        ServiceProvider.SetService(this, false);
    }
}
