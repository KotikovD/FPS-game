using UnityEngine;


namespace FPS_Kotikov_D
{

    public class AmmunitionClip : BaseObjectScene
    {

        
        [SerializeField] private AmmunitionType _typeAmunition;
        [SerializeField] private int _countClips = 2;

        private void OnCollisionEnter(Collision collision)
        {
            var tempObj = collision.gameObject.GetComponent<Player>();

            if (tempObj != null)
            {
                foreach (Weapons weapon in ServiceLocator.Resolve<Inventory>().Weapons)
                {
                    if (_typeAmunition.Equals(weapon.Ammunition.Type))
                    {
                        for (int i = 0; i < _countClips; i++)
                        {
                            weapon.AddClip(new Clip { CountAmmunition = weapon.MaxCountAmmunition });
                            DestroyAmmunitionClip();
                        }
                        
                    }
                }

            }
        }


        #region Methods

        private void DestroyAmmunitionClip()
        {
            Destroy(gameObject);
        }

        #endregion
    }
}