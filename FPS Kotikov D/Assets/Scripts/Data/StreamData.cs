using System.Collections.Generic;
using System.IO;


namespace FPS_Kotikov_D.Data
{
	public sealed class StreamData : IData<SerializableGameObject>
	{
		public void Save(Dictionary<int, SerializableGameObject> data, string path = null)
		{
			if (path == null) return;
			using (var sw = new StreamWriter(path))
			{
				for (int i = 0; i < data.Count; i++)
				{
					sw.WriteLine(data[i].Name);
					sw.WriteLine(data[i].IsEnable);
				}
			}
		}

		public SerializableGameObject Load(string path = null)
		{
			var result = new SerializableGameObject();

			using (var sr = new StreamReader(path))
			{
				while (!sr.EndOfStream)
				{
					result.Name = sr.ReadLine();
					result.IsEnable = sr.ReadLine().TryBool();
				}
			}
			return result;
		}
	}
}