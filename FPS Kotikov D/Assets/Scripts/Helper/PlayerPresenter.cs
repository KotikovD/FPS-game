using UnityEngine;


namespace FPS_Kotikov_D
{
    public static class PlayerPresenter 
    {


        public static Player Player;

        public static Vector3 Position
        {
            get => Player.transform.position;
        }


    }
}