﻿
// das sind einfach kleine Mathematische berechnungen die ich nicht stöndig in allen klassen haben möchte
// Tau ist z.B. pi*2,    inverslerp ändert die t value des lerps
using UnityEngine;

public static class MathLibary 
{

	// pi*2, circle constante
	public const float TAU = 6.28318530718f;

	// füge eine Winkel ein und erhalte einen normalizierten vector. Super einfach
	public static Vector2 GetUnitVectorByAngle( float angRad ) 
	{
		return new Vector2(
			Mathf.Cos( angRad ),
			Mathf.Sin( angRad )
		);
	}

	// Gebe a und b rein und du kriegst die Prozente eines lerps
	// (t value)
	// v ist in dieser range
	static float InverseLerp( float a, float b, float v ) 
	{
		return (v - a) / (b - a);
	}

	// Remaps eine komplette Value range zu einer anderen. Wie beim shader. Inverslep und lerp
	public static float Remap( float iMin, float iMax, float oMin, float oMax, float v ) 
	{
		float t = InverseLerp(iMin, iMax, v);
		return Mathf.LerpUnclamped( oMin, oMax, t );
	}
	public static float RemapClamped( float iMin, float iMax, float oMin, float oMax, float v ) 
	{
		float t = InverseLerp(iMin, iMax, v);
		return Mathf.Lerp( oMin, oMax, t );
	}

}
