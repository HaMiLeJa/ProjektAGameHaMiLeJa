using UnityEngine;

public static class ExtensionMethods
{
	//Eine methode von Unity um für alle Objekte einen Nullcheck zu machen
	public static T Ref<T>(this T obj) where T : Object
	{
		return obj == null ? null : obj;
	}
	public static float AspectRatio(this Texture texture)
	{
		return texture.width / texture.height;
	}
	// Bottom wird geclampt
	public static float AtLeast(this float v, float minVal) => Mathf.Max(v, minVal);
}
