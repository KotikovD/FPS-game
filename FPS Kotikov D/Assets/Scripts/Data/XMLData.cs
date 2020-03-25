using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;


namespace FPS_Kotikov_D.Data
{
    public sealed class XMLData : IData<SerializableGameObject>
    {
        public void Save(Dictionary<int, SerializableGameObject> data, string path = "")
        {
            var xmlDoc = new XmlDocument();

            XmlNode rootNode = xmlDoc.CreateElement("GameScene");
            xmlDoc.AppendChild(rootNode);

            foreach (var obj in data.Values)
            {
                var name = CheckName(obj.Name);

                XmlNode block = xmlDoc.CreateElement(name);
                rootNode.AppendChild(block);

                var element = xmlDoc.CreateElement("Name");
                element.SetAttribute("value", name);
                block.AppendChild(element);

                element = xmlDoc.CreateElement("PosX");
                element.SetAttribute("value", obj.Pos.X.ToString());
                element.SetAttribute("X", obj.Pos.X.ToString());
                block.AppendChild(element);

                element = xmlDoc.CreateElement("PosY");
                element.SetAttribute("value", obj.Pos.Y.ToString());
                block.AppendChild(element);

                element = xmlDoc.CreateElement("PosZ");
                element.SetAttribute("value", obj.Pos.Z.ToString());
                block.AppendChild(element);

                element = xmlDoc.CreateElement("IsEnable");
                element.SetAttribute("value", obj.IsEnable.ToString());
                block.AppendChild(element);

                //element.
            }


            XmlNode userNode = xmlDoc.CreateElement("Info");

            var attribute = xmlDoc.CreateAttribute("Unity");
            attribute.Value = Application.unityVersion;
            userNode.Attributes.Append(attribute);
            userNode.InnerText = "System Language: " + Application.systemLanguage;

            rootNode.AppendChild(userNode);

            xmlDoc.Save(path);
        }

        public SerializableGameObject Load(string path = "")
        {
            var result = new SerializableGameObject();
            if (!File.Exists(path)) return result;
            using (var reader = new XmlTextReader(path))
            {
                while (reader.Read())
                {
                    var key = Crypto.CryptoXOR("Name");
                    if (reader.IsStartElement(key))
                    {
                        result.Name = Crypto.CryptoXOR(reader.GetAttribute("value"));
                    }
                    key = "PosX";
                    if (reader.IsStartElement(key))
                    {
                        result.Pos.X = reader.GetAttribute("value").TrySingle();
                    }
                    key = "PosY";
                    if (reader.IsStartElement(key))
                    {
                        result.Pos.Y = reader.GetAttribute("value").TrySingle();
                    }
                    key = "PosZ";
                    if (reader.IsStartElement(key))
                    {
                        result.Pos.Z = reader.GetAttribute("value").TrySingle();
                    }
                    key = "IsEnable";
                    if (reader.IsStartElement(key))
                    {
                        result.IsEnable = reader.GetAttribute("value").TryBool();
                    }
                }
            }

            return result;
        }

        private string CheckName(string sourceName)
        {
            sourceName = sourceName.Replace('(', '_');
            sourceName = sourceName.Replace(')', '_');
            return  sourceName;
        }
    }
    

                
}

