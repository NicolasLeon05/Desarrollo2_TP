using UnityEngine;

public class GoalPoint : MonoBehaviour
{
    private void Awake()
    {
        ServiceProvider.SetService(this, true);
    }
}
