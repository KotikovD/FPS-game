using UnityEngine;


namespace FPS_Kotikov_D
{
    public class AnimatorDisabler : StateMachineBehaviour
    {

        override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
        {
            animator.enabled = false;
            Debug.Log("animator false");
        }

    }
}