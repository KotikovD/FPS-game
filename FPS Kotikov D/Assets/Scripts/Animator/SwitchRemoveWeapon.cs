using UnityEngine;

namespace FPS_Kotikov_D
{
    public class SwitchRemoveWeapon : StateMachineBehaviour
    {


        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.gameObject.SetActive(false);
        }


    }
}