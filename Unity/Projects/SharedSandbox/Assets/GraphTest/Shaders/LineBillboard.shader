
/*
Unity CG shader reference
https://docs.unity3d.com/455/Documentation/Manual/SL-Reference.html

Unity CG struct semantics
https://docs.unity3d.com/Manual/SL-VertexProgramInputs.html

CG functions reference
http://developer.download.nvidia.com/CgTutorial/cg_tutorial_appendix_e.html

Geometry shader tutorial
https://takinginitiative.wordpress.com/2011/01/12/directx10-tutorial-9-the-geometry-shader/
*/

Shader "Custom/LineBillboard"
{
	Properties
	{
		_color("Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_distance("Distance", Float) = 0.0
		_feather("Feather", Range(0.0, 5.0)) = 1.0
		_width("Width", Range(0.0,10.0)) = 1.0
	}

	SubShader
	{
		Pass
		{
			//Tags{ "Queue" = "Transparent" "RenderType" = "Opaque" "IgnoreProjector" = "True" }
			//Blend SrcAlpha OneMinusSrcAlpha // typical transparency
			//LOD 200

			Tags{ "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM

			#pragma target 5.0
			#pragma vertex VS_Main
			#pragma geometry GS_Main
			#pragma fragment FS_Main

			#include "UnityCG.cginc"

			/*
			Variables
			*/

			float4 _color;
			float _distance;
			float _feather;
			float _width;

			/*
			Data types
			*/

			struct VS_Input
			{
				float4 pos : POSITION;
				float2 uv0 : TEXCOORD0;
			};

			struct GS_Input
			{
				float4 pos : POSITION;
				float2 uv0 : TEXCOORD0;
			};

			struct FS_Input
			{
				float4 pos : POSITION;
				float2 uv0 : TEXCOORD0;
			};

			/*
			Helper functions
			*/



			/*
			Main
			*/

			GS_Input VS_Main(VS_Input v)
			{
				GS_Input g;
				g.pos = mul(unity_ObjectToWorld, v.pos);
				g.uv0 = v.uv0;
				return g;
			}


			[maxvertexcount(4)]
			void GS_Main(line GS_Input g[2], inout TriangleStream<FS_Input> stream)
			{
				float3 cam = _WorldSpaceCameraPos;

				float3 p0 = g[0].pos;
				float3 p1 = g[1].pos;

				float3 x = p1 - p0;
				float3 y = cross(p0 - cam, x);

				// offset vectors
				x = normalize(x) * _width;
				y = normalize(y) * _width;

				// append to stream
				FS_Input f;
				float4x4 vp = UNITY_MATRIX_VP;

				f.uv0 = g[0].uv0;

				f.pos = mul(vp, float4(p0 - x - y, 1.0));
				stream.Append(f);

				f.pos = mul(vp, float4(p0 + x - y, 1.0));
				stream.Append(f);

				f.uv0 = g[1].uv0;

				f.pos = mul(vp, float4(p1 + x + y, 1.0));
				stream.Append(f);

				f.pos = mul(vp, float4(p1 - x + y, 1.0));
				stream.Append(f);
			}


			float4 FS_Main(FS_Input f) : COLOR
			{
				//float t = smoothstep(_distance - _feather, _distance + _feather, f.uv0);
				//return float4(_color.xyz, t);
				return _color;
			}

			ENDCG
		}
	}
}
