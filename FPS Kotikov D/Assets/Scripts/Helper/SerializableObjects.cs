using System.Collections;
using System.Collections.Generic;
using FPS_Kotikov_D.Data;
using UnityEngine;


namespace FPS_Kotikov_D
{
    /// <summary>
    /// The helper class defines which objects will be saved using the save function.
    /// </summary>
    public class SerializableObjects : MonoBehaviour
    {

        // Filed gets data from objects with ISerializable
        public List<GameObject> PrefubsForSave = new List<GameObject>();


        public List<GameObject> CreateGameObjectsList()
        {
            PrefubsForSave.Clear();
            foreach (var obj in Object.FindObjectsOfType<BaseObjectScene>())
                //Adding objects to PrefubsForSave
                obj.SaveData();

            return PrefubsForSave;
        }

        public void DestroyStartCoroutine(SerializableGameObject[] objData)
        {
            StartCoroutine(DestroyOldGameObjects(objData));
        }

        public IEnumerator DestroyOldGameObjects(SerializableGameObject[] objData)
        {
            var objs = GameObject.FindObjectsOfType<BaseObjectScene>();
            foreach (var obj in objs)
            {
                if (obj.GetComponent<ISerializable>() != null)
                {
                    Destroy(obj.gameObject);
                    
#if UNITY_EDITOR
                    Debug.Log("Destroied " + obj.name);
#endif
                }
                yield return new WaitForSeconds(0.2f);
            }
            StopCoroutine("DestroyOldGameObjects");
            StartCoroutine(InstantiateGameObjects(objData));
        }

        public IEnumerator InstantiateGameObjects(SerializableGameObject[] objData)
        {
            foreach (var obj in objData)
            {
                if (obj.Name == null) continue;
                var newObj = Instantiate(Resources.Load<GameObject>(obj.Name), obj.Pos, obj.Rot);
                newObj.name = obj.Name;

                var player = newObj.GetComponent<Player>();
                if (player)
                {
                    player.CurrentHp = obj.SPlayer.CurrentHp;

                    var guns = player.GetComponentsInChildren<Gun>();
                    foreach (var serilizeGun in obj.SPlayer.Guns)
                    {
                        foreach (var gun in guns)
                        {
                            if (gun.name.Equals(serilizeGun.WeaponName))
                            {
                                gun.CountClips = serilizeGun.Ammunition.CountClips;
                                gun.CurrentAmmunition = serilizeGun.Ammunition.CurrentAmmunition;
                            }
                        }
                    }
                }
                yield return new WaitForSeconds(0.2f);
            }
            StopCoroutine("InstantiateGameObjects");
            // Rebooting controllers
            GameObject.FindObjectOfType<MainController>().Start();
        }


    }
}