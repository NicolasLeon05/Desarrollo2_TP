using UnityEngine;

public class PlaySoundUpdate : StateMachineBehaviour
{
    [SerializeField] private SoundType sound;
    [SerializeField, Range(0, 1)] private float volume = 1;
    [SerializeField] private float delay = 1f;
    private float timePassed = 0.0f;
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timePassed += Time.fixedDeltaTime;

        if (timePassed >= delay)
        {
            SoundManager.Instance.PlaySound(sound, volume);
            timePassed = 0.0f;
        }

        Debug.Log(timePassed);
    }
}
