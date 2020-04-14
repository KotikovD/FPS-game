using UnityEngine;


namespace FPS_Kotikov_D.Animation
{
    public class CreateEmptyClip : StateMachineBehaviour
    {

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var oldClip = animator.transform.GetComponentInChildren<AmmoClip>().transform;
            var newClip = Resources.Load("EmptyRifleClip");
            Instantiate(newClip, oldClip.position, oldClip.rotation);
        }

    }
}