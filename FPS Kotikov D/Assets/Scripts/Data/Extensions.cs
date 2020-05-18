using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


namespace FPS_Kotikov_D.Data
{
	public static partial class Extensions
	{
		public static Bounds GrowBounds(this Bounds a, Bounds b)
		{
			var max = Vector3.Max(a.max, b.max);
			var min = Vector3.Min(a.min, b.min);

			a = new Bounds((max + min) * 0.5f, max - min);

			return a;
		}

		public static bool TryBool(this string self)
		{
			return Boolean.TryParse(self, out var res) && res;
		}

		public static float TrySingle(this string self)
		{
			if (Single.TryParse(self, out var res))
			{
				return res;
			}
			return 0;
		}

		public static string PathCombine(this string self, string path)
		{
			return Path.Combine(self, path);
		}

		public static string Format(this string self, params object[] args)
		{
			return String.Format(self, args);
		}

		public static Vector3 MultiplyX(this Vector3 v, float val)
		{
			v = new Vector3(val * v.x, v.y, v.z);
			return v;
		}

		public static Vector3 MultiplyY(this Vector3 v, float val)
		{
			v = new Vector3(v.x, val * v.y, v.z);
			return v;
		}

		public static Vector3 MultiplyZ(this Vector3 v, float val)
		{
			v = new Vector3(v.x, v.y, val * v.z);
			return v;
		}

		public static T Random<T>(this List<T> list)
		{
			var val = list[UnityEngine.Random.Range(0, list.Count)];

			return val;
		}

		public static Color SetColorAlpha(this Color c, float alpha)
		{
			return new Color(c.r, c.g, c.b, alpha);
		}

		public static T DeepCopy<T>(this T self)
		{
			if (!typeof(T).IsSerializable)
			{
				throw new ArgumentException("Type must be iserializable");
			}
			if (ReferenceEquals(self, null))
			{
				return default(T);
			}

			var formatter = new BinaryFormatter();
			using (var stream = new MemoryStream())
			{
				formatter.Serialize(stream, self);
				stream.Seek(0, SeekOrigin.Begin);
				return (T)formatter.Deserialize(stream);
			}
		}
	}
}