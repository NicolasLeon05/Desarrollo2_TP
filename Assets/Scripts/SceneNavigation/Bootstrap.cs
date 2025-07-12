using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("BOOTSTRAP LOADED");
        SceneController.Instance.LoadDefaultScene();
    }
}
