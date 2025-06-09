using UnityEngine;

public class MapLimiter : MonoBehaviour
{
    [SerializeField] string invisibleWallTag = "InvisibleWall";

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(invisibleWallTag))
        {
           
        }
    }
}
