using UnityEngine;

public class MapLimiter : MonoBehaviour
{
    [SerializeField] private string invisibleWallTag = "MapLimit";
    [SerializeField] private Transform spawnPoint;

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(invisibleWallTag))
        {
            if (spawnPoint != null)
            {
                transform.position = spawnPoint.position;
                Rigidbody rigidBody = GetComponent<Rigidbody>();
                if (rigidBody != null)
                    rigidBody.linearVelocity = Vector3.zero;
            }
            else
            {
                Debug.LogWarning("Spawn Point not assigned in MapLimiter.");
            }
        }
    }
}