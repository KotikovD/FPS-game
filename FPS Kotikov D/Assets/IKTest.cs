using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace FPS_Kotikov_D
{

    public class IKTest : MonoBehaviour
    {

        [SerializeField] private LayerMask _rayLayerForFoots;
        [SerializeField] private float _smoothLerpForFoots = 0.5f;
        [SerializeField] private float _raysLenth = 0.5f;

        private Transform _leftLeg;
        private Transform _rightLeg;
        private Quaternion _rightLegRotation;
        private Vector3 _rightLegPosition;
        private Quaternion _leftLegRotation;
        private Vector3 _leftLegPosition;
        private Animator _animator;

        private void Awake()
        {
            _animator = transform.GetComponentInChildren<Animator>();
            _rightLeg = _animator.GetBoneTransform(HumanBodyBones.RightFoot);
            _leftLeg = _animator.GetBoneTransform(HumanBodyBones.LeftFoot);
        }

        private void Update()
        {
            if (Time.frameCount % 2 == 0)
            {
                var posR = _rightLeg.TransformPoint(Vector3.zero);
              //  Debug.DrawRay(posR, Vector3.down * _raysLenth, Color.red);
                if (Physics.Raycast(posR, Vector3.down, out var rightHit, _raysLenth, _rayLayerForFoots))
                {
                    _rightLegRotation = Quaternion.FromToRotation(transform.up, rightHit.normal) * transform.rotation;
                    _rightLegPosition = Vector3.Lerp(_rightLeg.position, rightHit.point, _smoothLerpForFoots);
                }
            }

            if (Time.frameCount % 2 != 0)
            {
                var posL = _leftLeg.TransformPoint(Vector3.zero);
              //  Debug.DrawRay(posL, Vector3.down * _raysLenth, Color.red);
                if (Physics.Raycast(posL, Vector3.down, out var leftHit, _raysLenth, _rayLayerForFoots))
                {
                    _leftLegRotation = Quaternion.FromToRotation(transform.up, leftHit.normal) * transform.rotation;
                    _leftLegPosition = Vector3.Lerp(_leftLeg.position, leftHit.point, _smoothLerpForFoots);
                }
            }
        }

        private void OnAnimatorIK()
        {
            //var weightRightFoot = 1f;
            //var weightLeftFoot = 1f;

            var weightRightFoot = _animator.GetFloat("RightLegIK");
           var  weightLeftFoot = _animator.GetFloat("LeftLegIK");
          //  Debug.Log("weightRightFoot " + weightRightFoot);

            _animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, weightRightFoot);
            _animator.SetIKPosition(AvatarIKGoal.RightFoot, _rightLegPosition);

            _animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, weightLeftFoot);
            _animator.SetIKPosition(AvatarIKGoal.LeftFoot, _leftLegPosition);


            _animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, weightRightFoot);
            _animator.SetIKRotation(AvatarIKGoal.RightFoot, _rightLegRotation);


            _animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, weightLeftFoot);
            _animator.SetIKRotation(AvatarIKGoal.LeftFoot, _leftLegRotation);


            

        }

    }
}