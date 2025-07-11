using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    private enum RotationAxis
    {
        X,
        Y,
        Z
    }

    [SerializeField] private RotationAxis axis = RotationAxis.Y;
    [SerializeField] private float rotationSpeed = 90f;
    private Vector3 rotationVector;

    private void Awake()
    {
        if (axis == RotationAxis.X)
            rotationVector = Vector3.right;
        else if (axis == RotationAxis.Y)
            rotationVector = Vector3.up;
        else if (axis == RotationAxis.Z)
            rotationVector = Vector3.forward;
        else
            rotationVector = Vector3.up;
    }

    private void FixedUpdate()
    {
        transform.Rotate(rotationVector, rotationSpeed * Time.deltaTime, Space.Self);
    }

}
