using UnityEngine;
namespace Common
{
	class Constant{
		public const float PI = 3.141592f;
		public static readonly float window_rate = Screen.height / 400.0f;
	}
	public enum Enemy{
		normal,spin,boss
	}
}