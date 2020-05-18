using UnityEngine;


namespace FPS_Kotikov_D
{
    [CreateAssetMenu(fileName = "CameraShaker", menuName = "CreateScriptableObjects/CameraShaker")]
    public sealed class CameraShakeSO : ScriptableObject
    {


        #region Fields

        [Header("Shake animation settings for camera")]
        public float Duration;
        public int Vibrato;
        [Range(0, 10)] public float Strength;
        [Range(0f, 90f)] public float Randomness;

        #endregion


    }
}