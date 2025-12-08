using UnityEngine;

public class GrappleSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject hookPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private LayerMask hookableMask;

    [Header("Grapple Values")]
    [SerializeField] private float pullSpeed = 30f;
    [SerializeField] private float stopDistance = 3f;
    [SerializeField] private float hookLifeTime = 6f;

    private GrappleHook currentHook;
    private bool isPulling;

    private Player player;

    private void Start()
    {
        ServiceProvider.TryGetService(out player);
        Debug.Log("HOOKABLE MASK VALUE EN START = " + hookableMask.value);
    }

    public void Fire()
    {
        if (currentHook != null)
            return;

        Transform cam = Camera.main.transform;

        GameObject go = Instantiate(hookPrefab, shootPoint.position, Quaternion.identity);

        currentHook = go.GetComponent<GrappleHook>();

        currentHook.Init(this, shootPoint.position, cam.forward, player.transform);

        isPulling = false;

        Destroy(go, hookLifeTime);
    }


    public void Cancel()
    {
        if (currentHook == null)
            return;

        Destroy(currentHook.gameObject);
        currentHook = null;
        isPulling = false;

        player.StopGrapple();
    }

    public bool IsHookable(GameObject obj)
    {
        return (hookableMask & (1 << obj.layer)) != 0;
    }

    public void OnHookAttached(GrappleHook hook)
    {
        isPulling = true;
    }

    private void FixedUpdate()
    {
        if (currentHook == null) return;

        int mask = hookableMask.value;

        Vector3 start = shootPoint.position;
        Vector3 end = currentHook.transform.position;
        Vector3 dir = (end - start).normalized;
        float dist = Vector3.Distance(start, end);

        if (Physics.Raycast(start, dir, out RaycastHit hit, dist, mask))
        {
            currentHook.transform.position = hit.point;
            currentHook.ForceAttach();
            OnHookAttached(currentHook);

        }

        if (!isPulling || currentHook == null) return;

        float playerDist = Vector3.Distance(start, currentHook.transform.position);
        if (playerDist <= stopDistance)
        {
            Cancel();
            return;
        }

        Vector3 pullDir = (currentHook.transform.position - start).normalized;

        var request = new ForceRequest
        {
            direction = pullDir,
            speed = pullSpeed,
            force = pullSpeed
        };

        player.StartGrapple(pullDir, pullSpeed);
    }

}
