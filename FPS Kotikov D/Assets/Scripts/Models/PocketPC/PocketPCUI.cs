using UnityEngine;


namespace FPS_Kotikov_D
{
    public class PocketPCUI : MonoBehaviour
    {
        

        public virtual void Switch(bool value)
        {
            enabled = value;
            gameObject.SetActive(value);
        }

       
    }
}