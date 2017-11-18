// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

/*
Unity shader reference
https://docs.unity3d.com/455/Documentation/Manual/SL-Reference.html

CG functions reference
http://developer.download.nvidia.com/CgTutorial/cg_tutorial_appendix_e.html
*/

Shader "Custom/Normal"
{
	Properties
	{
		_Color1("Color1", Color) = (0.0,0.0,0.0,0.0)
		_Color2("Color2", Color) = (1.0,1.0,1.0,1.0)
	}
	
	SubShader
	{
		Pass
		{
			Tags{"RenderType"="Opaque"}
			LOD 200

			CGPROGRAM

				#pragma vertex VS_Main
				#pragma fragment FS_Main
				#include "UnityCG.cginc"

				/*
				Data structures
				*/

				struct VS_Input
				{
					float4 pos : POSITION;
					float3 norm : NORMAL;
				};
			
				struct FS_Input
				{
					float4 pos : POSITION;
					float4 col : COLOR;
				};

				/*
				Variables
				*/

				uniform float4 _Color1;
				uniform float4 _Color2;
			
				/*
				Shader programs
				*/

				// Vertex shader
				FS_Input VS_Main(VS_Input v)
				{
					float3 norm = normalize(mul(unity_ObjectToWorld, float4(v.norm, 0.0)).xyz); // ensures rigid transform to world space
					float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
					float t = dot(norm, lightDir) * 0.5 + 0.5;
			
					FS_Input f;
					f.pos = UnityObjectToClipPos(v.pos);
					f.col = lerp(_Color1, _Color2, t);

					return f;
				}
			
				// Fragment shader
				float4 FS_Main(FS_Input f) : COLOR
				{
					return f.col;
				}
			
			ENDCG
		}
	}
}