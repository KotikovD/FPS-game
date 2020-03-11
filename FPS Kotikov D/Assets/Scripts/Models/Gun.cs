namespace FPS_Kotikov_D
{
    public sealed class Gun : Weapons
    {


        #region Methods

        public override void Fire()
        {
            if (!CanFire) return;
            if (IsReloading) return;

            if (CurrentAmmunition > 0)
            {
                var temAmmunition = Instantiate(Ammunition, _bulletSpawn.position, _bulletSpawn.rotation);
                temAmmunition.AddForce(_bulletSpawn.forward * _force);

                CanFire = false;
                Invoke(nameof(ReadyShoot), _rechargeTime);

                CurrentAmmunition--;

                if (CurrentAmmunition == 0)
                    if (CountClips > 0)
                        ReloadClip();
            }


            
        }

        #endregion


    }
}