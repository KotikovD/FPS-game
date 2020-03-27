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


        public List<GameObject> PrefubsForSave = new List<GameObject>();


        public List<GameObject> GetGameObjcets()
        {
            return PrefubsForSave;
        }


        public void DestroyGameObjects()
        {
            foreach (var obj in PrefubsForSave)
            {
                if (obj == null) continue;
                Destroy(obj);  
            }
            PrefubsForSave.Clear();
        }



        public void InstantiateGameObjects(SerializableGameObject[] objData)
        {
            foreach (var obj in objData)
            {

                if (obj.Name == null) continue;
                var newObj = Instantiate(Resources.Load<GameObject>(obj.Name), obj.Pos, Quaternion.identity);
                newObj.name = obj.Name;

            }

        }


    }

}