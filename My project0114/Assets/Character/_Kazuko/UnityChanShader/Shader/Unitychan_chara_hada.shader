Shader "UnityChan/Skin"
{
	Properties
	{
		_Color ("Main Color", Color) = (1, 1, 1, 1)
		_ShadowColor ("Shadow Color", Color) = (0.8, 0.8, 1, 1)
		_EdgeThickness ("Outline Thickness", Float) = 1
		_DepthBias  ("Outline Depth Bias", Float) = 0.00012
		
		_MainTex ("Diffuse", 2D) = "white" {}
		_FalloffSampler ("Falloff Control", 2D) = "white" {}
		_RimLightSampler ("RimLight Control", 2D) = "white" {}

		_DissolveTex("溶解噪声图",2D) = "white"{}
		_DissolveThreshold("溶解阈值",Range(0.0,1)) = 0

		_RampTex ("渐变纹理 用于溶解",2D) = ""{}

	}

	SubShader
	{
		Tags
		{
			"RenderType"="Opaque"
			"Queue"="Geometry"
			"LightMode"="ForwardBase"
		}
		// skin pass
		Pass
		{
			Cull Back
			ZTest LEqual
			CGPROGRAM
			#pragma multi_compile_fwdbase
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
			#include "CharaSkin.cginc"
			ENDCG
		}
		// 描边pass
		// Pass
		// {
		// 	Cull Front
		// 	ZTest Less
		// 	CGPROGRAM
		// 	#pragma target 3.0
		// 	#pragma vertex vert
		// 	#pragma fragment frag
		// 	#include "UnityCG.cginc"
		// 	#include "CharaOutline.cginc"
		// 	ENDCG
		// }
	}

	FallBack "Transparent/Cutout/Diffuse"
}
