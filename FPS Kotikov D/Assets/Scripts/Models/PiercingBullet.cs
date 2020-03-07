using UnityEngine;


namespace FPS_Kotikov_D
{

    public class PiercingBullet : Ammunition
    {

        private void OnCollisionEnter(Collision collision)
        {  
            if (collision.collider.tag == "Bullet") return;

            // Вызываем функцию нанесения урона
            
            Destroy(InstanceObject);          
        }


    }
}