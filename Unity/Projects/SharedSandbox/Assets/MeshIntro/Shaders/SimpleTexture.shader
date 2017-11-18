Shader "RC3/SimpleTexture"
{
	Properties
	{
		_mainTex("Texture", 2D) = "white" { }
	}
	
	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex VS_Main
			#pragma fragment FS_Main

			#include "UnityCG.cginc"

			/*
			Variables
			*/

			sampler2D _mainTex;

			/*
			Data types
			*/

			struct VS_Input
			{
				float4 position : POSITION;
				float2 uv0 : TEXCOORD0;
			};

			struct FS_Input
			{
				float4 position : POSITION;
				float2 uv0 : TEXCOORD0;
			};

			/*
			Main
			*/

			FS_Input VS_Main(VS_Input v)
			{
				FS_Input f;
				f.position = UnityObjectToClipPos(v.position);
				f.uv0 = v.uv0;
				return f;
			}

			float4 FS_Main(FS_Input f) : COLOR
			{
				return tex2D(_mainTex, f.uv0);
			}

			ENDCG
		}
	}
}
