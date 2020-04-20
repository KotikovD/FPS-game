using FPS_Kotikov_D.Controller;
using UnityEngine;


namespace FPS_Kotikov_D
{
    public sealed class Gun : Weapons
    {

        private RaycastHit _hit;


        #region Methods

        public override void Fire()
        {
            if (CurrentAmmunition > 0)
            {
                _hit = PlayerController.Hit;
                var projectorDelay = _hit.distance / _bulletSpeed;

                var partical = Instantiate(ParticalShoot, _bulletSpawn.position, _bulletSpawn.rotation);
                var particalRB = partical.GetComponent<Rigidbody>();
                particalRB.AddForce(_bulletSpawn.forward * _bulletSpeed);
                Destroy(partical, Ammunition.TimeToDestruct);

                CanFire = false;
                Invoke(nameof(PlaceBullet), projectorDelay);
                Invoke(nameof(ReadyShoot), _rechargeTime);

                CurrentAmmunition--;
                if (CurrentAmmunition == 0)
                    ReloadClip();
            }
        }

        private void PlaceBullet()
        {
            Instantiate(Ammunition, _hit.point, _bulletSpawn.rotation);
        }

        #endregion


    }
}