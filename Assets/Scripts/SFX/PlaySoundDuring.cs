using UnityEngine;

public class PlaySoundDuring : StateMachineBehaviour
{

    [SerializeField] private SoundType sound;
    [SerializeField, Range(0, 1)] private float volume = 1;
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AudioManager.PlayEffect(sound, volume);
    }

    
}
