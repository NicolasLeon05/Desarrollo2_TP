using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(LineRenderer))]
public class GrappleHook : MonoBehaviour
{
    [SerializeField] private float shootForce = 40f;

    private Rigidbody rb;
    private LineRenderer lineRenderer;

    private Transform player;
    private GrappleSystem system;
    private Collider col;

    public void Init(GrappleSystem system, Vector3 startPos, Vector3 direction, Transform player)
    {
        this.system = system;
        this.player = player;

        rb = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
        col = GetComponent<Collider>();

        col.enabled = false;

        transform.position = startPos;
        transform.forward = direction;

        StartCoroutine(EnableColliderNextFrame());

        rb.AddForce(direction.normalized * shootForce, ForceMode.Impulse);
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
        if (system.IsHookable(other.gameObject))
        {
            rb.useGravity = false;
            rb.isKinematic = true;

            system.OnHookAttached(this);
        }
    }
}
