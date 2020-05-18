using UnityEngine;


namespace FPS_Kotikov_D.Animation
{
    public class CreateEmptyClip : StateMachineBehaviour
    {

        private AmmunitionClip newClip;

        private void Awake()
        {
            newClip = Resources.Load<AmmunitionClip>("AmmunitionClips/BulletClip");
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool("Shoot", false);
            var oldClip = animator.GetComponentInChildren<AmmunitionClip>();
            newClip.CountClips = 0;
            Instantiate(newClip, oldClip.transform.position, oldClip.transform.rotation);
        }

    }
}