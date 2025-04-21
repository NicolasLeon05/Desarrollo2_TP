using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Player : MonoBehaviour
{
    ForceRequest constantForceRequest;
    ForceRequest instantForceRequest;

    private Rigidbody _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    public void RequestInstantForce(ForceRequest forceRequest)
    {
        instantForceRequest = forceRequest;
    }

    public void RequestConstantForce(ForceRequest forceRequest)
    {
        constantForceRequest = forceRequest;
    }

    private void FixedUpdate()
    {
        if (constantForceRequest != null)
            _rigidBody.AddForce(constantForceRequest.direction * constantForceRequest.force, ForceMode.Force);

        if (instantForceRequest != null)
        {
            _rigidBody.AddForce(instantForceRequest.direction * instantForceRequest.force, ForceMode.Impulse);
            instantForceRequest = null;
        }
    }
}
