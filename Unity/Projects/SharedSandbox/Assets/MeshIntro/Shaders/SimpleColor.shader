Shader "RC3/SimpleColor"
{
	Properties
	{
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex VS_Main
			#pragma fragment FS_Main
			
			#include "UnityCG.cginc"

			/*
			Data types
			*/

			struct VS_Input
			{
				float4 position : POSITION;
				float4 color : COLOR;
			};

			struct FS_Input
			{
				float4 position : POSITION;
				float4 color : COLOR;
			};

			/*
			Main
			*/

			FS_Input VS_Main(VS_Input v)
			{
				FS_Input f;
				f.position = UnityObjectToClipPos(v.position);
				f.color = v.color;
				return f;
			}

			float4 FS_Main(FS_Input f) : COLOR
			{
				return f.color;
			}

			ENDCG
		}
	}
}
