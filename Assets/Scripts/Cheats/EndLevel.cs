using UnityEngine;

public class EndLevel : MonoBehaviour
{
    [SerializeField] private Level levelToLoad;

    /// <summary>
    /// Called when a level's goal is reached.
    /// Plays the corresponding sound and loads the next level additively
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        SoundManager.Instance.PlaySound(SoundType.UnlockLevel, 0.3f);

        SceneController sceneController;
        ServiceProvider.TryGetService(out sceneController);
        sceneController.AddLevel(levelToLoad);
        gameObject.SetActive(false);
    }
}