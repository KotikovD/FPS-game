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
            // TODO Modify all with Nodes
            var gameScene = new XElement("GameScene");

            foreach (var obj in data.Values)
            {
                XAttribute x = new XAttribute("PosX", obj.Pos.X);
                XAttribute y = new XAttribute("PosY", obj.Pos.Y);
                XAttribute z = new XAttribute("PosZ", obj.Pos.Z);

                XAttribute Qx = new XAttribute("QuaX", obj.Rot.X);
                XAttribute Qy = new XAttribute("QuaY", obj.Rot.Y);
                XAttribute Qz = new XAttribute("QuaZ", obj.Rot.Z);
                XAttribute Qw = new XAttribute("QuaW", obj.Rot.W);


                XElement block = new XElement("Instance", obj.Name, x, y, z, Qx, Qy, Qz, Qw);


                if (obj.Name == "Player")
                {
                    block.SetAttributeValue("CurrentHP", obj.SPlayer.CurrentHp);

                    if (obj.SPlayer.Guns.Length > 0)
                        foreach (var gun in obj.SPlayer.Guns)
                        {
                            block.SetAttributeValue(gun.WeaponName, gun.Ammunition.ToString());
                        }
                }



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

                    Rot = new SerializableQuaternion(
                        instance.Attribute("QuaX").Value.TrySingle(),
                        instance.Attribute("QuaY").Value.TrySingle(),
                        instance.Attribute("QuaZ").Value.TrySingle(),
                        instance.Attribute("QuaW").Value.TrySingle()),

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

