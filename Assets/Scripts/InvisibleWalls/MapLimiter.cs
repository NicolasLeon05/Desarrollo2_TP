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
        if (other.CompareTag("Player"))
        {
            if (spawnPoint != null)
            {
                Rigidbody playerRigidBody = other.GetComponent<Rigidbody>();
                playerRigidBody.MovePosition(spawnPoint.position + Vector3.up * 2f);
                if (playerRigidBody != null)
                    playerRigidBody.linearVelocity = Vector3.zero;
            }
            else
            {
                Debug.LogWarning("Spawn Point not assigned in MapLimiter.");
            }
        }
    }
}