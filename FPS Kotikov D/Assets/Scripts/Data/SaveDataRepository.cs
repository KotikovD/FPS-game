using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace FPS_Kotikov_D.Data
{
    public sealed class SaveDataRepository
    {
        private readonly IData<SerializableGameObject> _data;

        private const string _folderName = "DataSave";
        private const string _fileName = "Saves.txt";
        private readonly string _path;

        private Dictionary<int, SerializableGameObject> _saveObjects = new Dictionary<int, SerializableGameObject>();



        public SaveDataRepository()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                _data = new PlayerPrefsData();
            }
            else
            {
                _data = new XMLData();
            }
            _path = Path.Combine(Application.dataPath, _folderName);

        }

        public void Save()
        {
            if (!Directory.Exists(Path.Combine(_path)))
                Directory.CreateDirectory(_path);

            AddSaveObjects(GameObject.FindGameObjectsWithTag("Player"));
            AddSaveObjects(GameObject.FindGameObjectsWithTag("PickUps"));
            AddSaveObjects(GameObject.FindGameObjectsWithTag("Enemy"));
            
            _data.Save(_saveObjects, Path.Combine(_path, _fileName));
        }

        private void AddSaveObjects(GameObject[] objects)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                Debug.Log(_saveObjects.Count);
                var obj = new SerializableGameObject
                {
                    Pos = objects[i].transform.position,
                    Name = objects[i].name,
                    IsEnable = true
                };

                var a = _saveObjects.Count == 0 ? 0 : 1;

                _saveObjects.Add(_saveObjects.Count + a, obj);
            }
        }


        public void Load()
        {
            //var file = Path.Combine(_path, _fileName);
            //if (!File.Exists(file)) return;
            //var newPlayer = _data.Load(file);
            //MainController.Instance.Player.position = newPlayer.Pos;
            //MainController.Instance.Player.name = newPlayer.Name;
            //MainController.Instance.Player.gameObject.SetActive(newPlayer.IsEnable);

            //Debug.Log(newPlayer);
        }
    }
}