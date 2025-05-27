using UnityEngine;

public class PlataformFollower : MonoBehaviour
{
    private Transform currentPlatform;
    private Vector3 lastPlatformPosition;

    void Update()
    {
        if (currentPlatform != null)
        {
            Vector3 platformDelta = currentPlatform.position - lastPlatformPosition;
            transform.position += platformDelta;
            lastPlatformPosition = currentPlatform.position;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("MovingPlataform"))
        {
            currentPlatform = collision.transform;
            lastPlatformPosition = currentPlatform.position;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("MovingPlataform"))
        {
            currentPlatform = null;
        }
    }
}
