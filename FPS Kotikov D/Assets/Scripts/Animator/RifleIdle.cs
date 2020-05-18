using UnityEngine;


namespace FPS_Kotikov_D.Animation
{
    public class RifleIdle : StateMachineBehaviour
    {

        ////private const float FIXDELAY = 2.0f;
        //private bool isFirst = true;
        //CharacterController _character;
        //private Animator _animator;


        //private void Awake()
        //{
        //    //_character = _animator.transform.GetComponentInParent<CharacterController>();

        //    //var delay = _animator.GetFloat("ReloadDelay");
        //    //if (delay - FIXDELAY > 0)
        //    //    delay = 1 / (delay - FIXDELAY);
        //    //_animator.SetFloat("ReloadDelay", delay);

        //    //var reCharge = _animator.GetFloat("ShootRecharge");

        //    //// TODO remove magic from here
        //    //if (reCharge > 0)
        //    //    reCharge = reCharge / 60 * 100 * 100;
        //    //_animator.SetFloat("ShootRecharge", reCharge);
        //}

        ////override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        ////{

        ////    if (!isFirst) return;
        ////    isFirst = false;
        ////    _animator = animator;
        ////}

        ////override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        ////{
        ////    animator.SetFloat("Magnitude", _character.velocity.normalized.magnitude);
        ////}

    }
}