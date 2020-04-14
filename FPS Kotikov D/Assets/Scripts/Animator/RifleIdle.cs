using UnityEngine;


namespace FPS_Kotikov_D.Animation
{
    public class RifleIdle : StateMachineBehaviour
    {

        private const float FIXDELAY = 2.0f;
        private bool isFirst = true;
        CharacterController _character;


        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!isFirst) return;

            _character = animator.transform.GetComponentInParent<CharacterController>();

            var delay = animator.GetFloat("ReloadDelay");
            if (delay - FIXDELAY > 0)
                delay = 1 / (delay - FIXDELAY);
            animator.SetFloat("ReloadDelay", delay);

            var reCharge = animator.GetFloat("ShootRecharge");

            // TODO remove magic from here
            if (reCharge > 0)
                reCharge = reCharge / 60 * 100 * 100;
            animator.SetFloat("ShootRecharge", reCharge);

            isFirst = false;
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetFloat("Magnitude", _character.velocity.normalized.magnitude);
        }

    }
}