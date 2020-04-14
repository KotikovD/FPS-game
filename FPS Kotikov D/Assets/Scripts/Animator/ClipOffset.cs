using UnityEngine;


namespace FPS_Kotikov_D.Animation
{
    public class ClipOffset : StateMachineBehaviour
    {

        private Weapons _weapon;
        private GameObject _clip;
        private BoxCollider _clipBox;
        private float offset;
        private bool isSerialized = false;


        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

            // TODO clip offset

            //_weapon = animator.gameObject.GetComponent<Weapons>();
            //_clip = animator.gameObject.transform.GetComponentInChildren<AmmoClip>().gameObject;
            // _clipBox = _clip.GetComponent<BoxCollider>();
            // offset = _clipBox.bounds.size.x / _weapon.MaxCountAmmunition;
            // offset = animator.GetFloat("ClipOffsetTime");
            // offset = offset / _weapon.MaxCountAmmunition;
            // animator.SetFloat("ClipOffsetTime", offset);


            //var position = _clip.transform.position;
            //position.x -= offset;
            //_clip.transform.position = position;
            //_clip.transform.Translate(position);

        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool("Shoot", false);
        }
    }
}