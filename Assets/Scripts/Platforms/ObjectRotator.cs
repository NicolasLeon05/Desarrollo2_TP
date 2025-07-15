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

    /// <summary>
    /// Sets the rotation vector based on the axys the object will rotate in
    /// </summary>
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

    /// <summary>
    /// Rotates the object in the designated axys at the designated speed
    /// </summary>
    private void Update()
    {
        transform.Rotate(rotationVector, rotationSpeed * Time.deltaTime, Space.Self);
    }

}
