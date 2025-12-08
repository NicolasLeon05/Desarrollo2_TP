using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(LineRenderer))]
public class GrappleHook : MonoBehaviour
{
    [SerializeField] private float shootForce = 40f;

    private Rigidbody rigidBody;
    private LineRenderer lineRenderer;

    private Transform player;
    private GrappleSystem system;
    private Collider col;

    public void Init(GrappleSystem system, Vector3 startPos, Vector3 direction, Transform player)
    {
        this.system = system;
        this.player = player;

        rigidBody = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
        col = GetComponent<Collider>();

        col.enabled = false;

        transform.position = startPos;
        transform.forward = direction;

        StartCoroutine(EnableColliderNextFrame());

        rigidBody.AddForce(direction.normalized * shootForce, ForceMode.Impulse);
    }

    private IEnumerator EnableColliderNextFrame()
    {
        yield return null;
        col.enabled = true;
    }


    private void Update()
    {
        if (lineRenderer)
        {
            lineRenderer.SetPosition(0, player.position);
            lineRenderer.SetPosition(1, transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (system.IsHookable(other.gameObject) && !other.CompareTag("Player"))
        {
            rigidBody.useGravity = false;
            rigidBody.isKinematic = true;

            system.OnHookAttached(this);
        }
    }

    public void ForceAttach()
    {
        rigidBody.useGravity = false;
        rigidBody.isKinematic = true;
    }
}
