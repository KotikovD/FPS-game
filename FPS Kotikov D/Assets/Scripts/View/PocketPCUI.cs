using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

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