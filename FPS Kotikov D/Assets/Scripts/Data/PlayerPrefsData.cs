using System.Collections.Generic;
using UnityEngine;


namespace FPS_Kotikov_D.Data
{
	public class PlayerPrefsData : IData<SerializableGameObject>
	{
		public void Save(Dictionary<int, SerializableGameObject> data, string path = null)
		{

			for (int i = 0; i < data.Count; i++)
			{
				PlayerPrefs.SetString("Name", data[i].Name);
				PlayerPrefs.SetFloat("PosX", data[i].Pos.X);
				PlayerPrefs.SetString("IsEnable", data[i].IsEnable.ToString());
			}
			PlayerPrefs.Save();
		}

		public SerializableGameObject Load(string path = null)
		{
			var result = new SerializableGameObject();

			var key = "Name";
			if (PlayerPrefs.HasKey(key))
			{
				result.Name = PlayerPrefs.GetString(key);
			}

			key = "PosX";
			if (PlayerPrefs.HasKey(key))
			{
				result.Pos.X = PlayerPrefs.GetFloat(key);
			}

			key = "IsEnable";
			if (PlayerPrefs.HasKey(key))
			{
				result.IsEnable = PlayerPrefs.GetString(key).TryBool();
			}
			return result;
		}

		public void Clear()
		{
			PlayerPrefs.DeleteAll();
		}
	}
}