using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace FPS_Kotikov_D.Data
{
    public sealed class SaveDataRepository : BaseController
    {
        private readonly IData<SerializableGameObject> _data;

        private const string _folderName = "DataSave";
        private const string _fileName = "SavesNew2.txt";
        private readonly string _path;

        private Dictionary<int, SerializableGameObject> _saveObjects = new Dictionary<int, SerializableGameObject>();
        private SerializableObjects _serializableGameObjects;


        public SaveDataRepository()
        {
            _data = new XMLData();
            _path = Path.Combine(Application.dataPath, _folderName);
            _serializableGameObjects = Object.FindObjectOfType<SerializableObjects>();
        }

        public void Save()
        {

            if (!Directory.Exists(Path.Combine(_path)))
                Directory.CreateDirectory(_path);
            var objectsForSave = _serializableGameObjects.GetGameObjcets();

            int counter = 0;
            for (int i = 0; i < objectsForSave.Count; i++)
            {
                if (objectsForSave[i] == null) continue;
                var objData = new SerializableGameObject
                {
                    Pos = objectsForSave[i].transform.position,
                    Name = objectsForSave[i].name,
                    IsEnable = true //TODO получение параметра
                };
                var index = counter == 0 ? 0 : 1;
                _saveObjects.Add(_saveObjects.Count + index, objData);
            }
            counter = _saveObjects.Count;

            _data.Save(_saveObjects, Path.Combine(_path, _fileName));
        }



        public void Load()
        {
            var filePath = Path.Combine(_path, _fileName);
            if (!File.Exists(filePath)) return;

            _serializableGameObjects.DestroyGameObjects();
            _serializableGameObjects.InstantiateGameObjects(_data.Load(filePath));
        }
    }
}