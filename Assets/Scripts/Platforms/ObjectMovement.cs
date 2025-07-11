using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    private enum MovementType
    {
        Vertical,
        HorizontalX,
        HorizontalZ
    }

    [SerializeField] private MovementType movementType;
    [SerializeField] private float totalMovement = 2f;
    [SerializeField] private float movementSpeed = 1f;

    private Vector3 startPos;
    private Vector3 targetOffset;
    private bool goingForward = true;

    private void Awake()
    {
        startPos = transform.position;

        switch (movementType)
        {
            case MovementType.Vertical:
                targetOffset = new Vector3(0, totalMovement, 0);
                break;
            case MovementType.HorizontalX:
                targetOffset = new Vector3(totalMovement, 0, 0);
                break;
            case MovementType.HorizontalZ:
                targetOffset = new Vector3(0, 0, totalMovement);
                break;
        }
    }

    private void Update()
    {
        Vector3 targetPos = goingForward ? startPos + targetOffset : startPos;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, movementSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            goingForward = !goingForward;
        }
    }

}
