using System;

namespace FPS_Kotikov_D.Data
{
	public static class Crypto
	{
		public static string CryptoXOR(string text, int key = 42)
		{
			var result = String.Empty;
			foreach (var simbol in text)
			{
				result += (char)(simbol ^ key);
			}
			return result;
		}
	}
}