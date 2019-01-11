Shader "Custom/RingShader" {
	SubShader{
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
		Pass{
		CGPROGRAM

		#pragma target 5.0

#pragma vertex vert
#pragma geometry geom
#pragma fragment frag

#include "UnityCG.cginc"

#define PI 3.1415926535

		struct VSOut {
		float4 pos : SV_POSITION;
		float4 col : COLOR;
		float id : TEXCOORD0;
	};

	struct Ring
	{
		float3 pos;
		float4 rotate;
		float3 scale;
		float innerPercentage;
		float fanAngle;
		float4 col;
	};

	StructuredBuffer<Ring> Rings;

	VSOut vert(uint id : SV_VertexID)
	{
		VSOut output;
		output.pos = float4(Rings[id].pos, 1);
		output.col = Rings[id].col;
		output.id = id;

		return output;
	}

	float3 qTransform(float4 q, float3 v) {
		return v + 2.0 * cross(cross(v, q.xyz) - q.w * v, q.xyz);
	}

	[maxvertexcount(64)]
	void geom(point VSOut input[1], inout TriangleStream<VSOut> outStream)
	{
		VSOut output;
		float4 pos = input[0].pos;
		float4 col = input[0].col;
		float id = input[0].id;
		float3 scale = Rings[id].scale;
		float4 rotate = Rings[id].rotate;

		for (int i = 0; i < 32; i++)
		{
			float angle = i / 31.0f * Rings[id].fanAngle;
			float3 posOffset = float3(cos(angle) * scale.x, sin(angle) * scale.y, 0);

			output.col = col;
			output.id = id;
			{
				float3 ringPosOffset = qTransform(rotate, posOffset);
				output.pos = mul(UNITY_MATRIX_VP, pos + float4(ringPosOffset, 0));

				outStream.Append(output);
			}
			{
				float3 ringPosOffset = qTransform(rotate, posOffset * Rings[id].innerPercentage);
				output.pos = mul(UNITY_MATRIX_VP, pos + float4(ringPosOffset, 0));

				outStream.Append(output);
			}

		}

		outStream.RestartStrip();
	}

	fixed4 frag(VSOut i) : COLOR
	{
		return i.col;
	}

		ENDCG
	}
	}
}