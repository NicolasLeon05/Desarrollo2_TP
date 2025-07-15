using UnityEngine;

public class PlaySoundEnter : StateMachineBehaviour
{
    [SerializeField] private SoundType sound;
    [SerializeField, Range(0, 1)] private float volume = 1;

    /// <summary>
    /// Called when the animation state is entered.
    /// Plays the assigned sound once using the SoundManager
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SoundManager.Instance.PlaySound(sound, volume);
    }
}
