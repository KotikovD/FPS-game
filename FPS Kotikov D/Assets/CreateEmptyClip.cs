using UnityEngine;


namespace FPS_Kotikov_D.Animation
{
    public class CreateEmptyClip : StateMachineBehaviour
    {

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var oldClip = animator.transform.GetComponentInChildren<AmmunitionClip>();
            var newClip = Resources.Load<AmmunitionClip>("AmmunitionClips/BulletClip");
            newClip.CountClips = 0;
            Instantiate(newClip, oldClip.transform.position, oldClip.transform.rotation);
            
        }

    }
}