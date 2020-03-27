using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;

namespace FPS_Kotikov_D.Data
{
    public sealed class XMLData : IData<SerializableGameObject>
    {


        public void Save(Dictionary<int, SerializableGameObject> data, string path = "")
        {
            var gameScene = new XElement("GameScene");

            foreach (var obj in data.Values)
            {
                XAttribute x = new XAttribute("PosX", obj.Pos.X);
                XAttribute y = new XAttribute("PosY", obj.Pos.Y);
                XAttribute z = new XAttribute("PosZ", obj.Pos.Z);

                XElement block = new XElement("Instance", obj.Name, x, y, z);
                gameScene.Add(block);
            }
            var xmlDoc = new XDocument(gameScene);
            File.WriteAllText(path, xmlDoc.ToString());
        }

        public SerializableGameObject[] Load(string path = "")
        {
            SerializableGameObject[] result = new SerializableGameObject[100];
            if (!File.Exists(path)) return result;
            XElement gameScene = XDocument.Parse(File.ReadAllText(path)).Element("GameScene");

            int i = 0;
            foreach (XElement instance in gameScene.Elements("Instance"))
            {
                var objData = new SerializableGameObject
                {
                    Pos = new SerializableVector3(
                        instance.Attribute("PosX").Value.TrySingle(),
                        instance.Attribute("PosY").Value.TrySingle(),
                        instance.Attribute("PosZ").Value.TrySingle()),
                    Name = instance.Value,
                    IsEnable = true //TODO получение параметра
                };
                result.SetValue(objData, i);
                i++;
            }
            return result;
        }


    }



}

