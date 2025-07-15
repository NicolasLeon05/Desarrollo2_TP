using UnityEngine;

public class PlatformFollower : MonoBehaviour
{
    private Transform currentPlatform;
    private Vector3 lastPlatformPosition;

    /// <summary>
    /// Updates the player's position based on the platform's movement delta,
    /// if currently standing on a moving platform
    /// </summary>
    private void Update()
    {
        if (currentPlatform != null)
        {
            Vector3 platformDelta = currentPlatform.position - lastPlatformPosition;
            transform.position += platformDelta;
            lastPlatformPosition = currentPlatform.position;
        }
    }

    /// <summary>
    /// When colliding with a moving platform, stores its transform and position
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("MovingPlataform"))
        {
            currentPlatform = collision.transform;
            lastPlatformPosition = currentPlatform.position;
        }
    }

    /// <summary>
    /// Stops following the platform once the collision ends
    /// </summary>
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("MovingPlataform"))
        {
            currentPlatform = null;
        }
    }
}
