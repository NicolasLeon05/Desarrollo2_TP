using UnityEngine;

public class MapLimiter : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    /// <summary>
    /// Takes the player back to the spawn point and removes it's linear velocity.
    /// Called when player exits the map limits
    /// </summary>
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