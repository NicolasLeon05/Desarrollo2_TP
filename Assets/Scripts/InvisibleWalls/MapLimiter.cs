using UnityEngine;

public class MapLimiter : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    private void OnTriggerExit(Collider other)
    {
        if (spawnPoint != null)
        {
            other.transform.position = spawnPoint.position;
            Rigidbody playerRigidBody = other.GetComponent<Rigidbody>();
            if (playerRigidBody != null)
                playerRigidBody.linearVelocity = Vector3.zero;
        }
        else
        {
            Debug.LogWarning("Spawn Point not assigned in MapLimiter.");
        }
    }
}