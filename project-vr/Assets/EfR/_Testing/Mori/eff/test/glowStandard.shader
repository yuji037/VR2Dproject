Shader "Custom/GlowStandard" {
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Center("Center", Vector) = (0.0, 0.0, 0.0)
		_GridColor("GridColor", Vector) = (0.2, 0.3, 0.5)
	}
		SubShader
	{
		Tags{ "RenderType" = "Opaque" }

		CGPROGRAM
#pragma surface surf SimplePhong
#pragma glsl
#pragma target 3.0

	sampler2D _MainTex;
	sampler2D _ReflectionDepthTex;
	float4x4 _ViewProjectInverse;
	float3 _Center;
	float4 _GridColor;



	struct Input {
		float2 uv_MainTex;
		float3 worldPos;
		float4 screenPos;
	};

	float  iq_rand(float  p)
	{
		return frac(sin(p)*43758.5453);
	}

	float  _gl_mod(float  a, float  b) { return a - b * floor(a / b); }
	float2 _gl_mod(float2 a, float2 b) { return a - b * floor(a / b); }
	float3 _gl_mod(float3 a, float3 b) { return a - b * floor(a / b); }
	float4 _gl_mod(float4 a, float4 b) { return a - b * floor(a / b); }

	float Grid(float3 pos)
	{
		float grid_size = 0.4;
		float line_thickness = 0.015;

		float2 m = _gl_mod(abs(pos.xz*sign(pos.xz)), grid_size);
		float s = 0.0;
		if (m.x - line_thickness < 0.0 || m.y - line_thickness < 0.0) {
			return 1.0;
		}
		return 0.0;
	}

	float Circle(float3 pos)
	{
		float o_radius = 5.0;
		float i_radius = 4.0;
		float d = length(pos.xz);
		float c = max(o_radius - (o_radius - _gl_mod(d - _Time.y*1.5, o_radius)) - i_radius, 0.0);
		return c;
	}

	float Box(float2 p)
	{
		return max(0.5 - _gl_mod(p.y - p.x*0.4 + (_Time.y*0.3), 1.5), 0.0)*5.0;
	}

	float Hex(float2 p, float2 h)
	{
		float2 q = abs(p);
		return max(q.x - h.y,max(q.x + q.y*0.57735,q.y*1.1547) - h.x);
	}

	float HexGrid(float3 p)
	{
		float scale = 1.2;
		float2 grid = float2(0.692, 0.4) * scale;
		float radius = 0.22 * scale;

		float2 p1 = _gl_mod(p.xz, grid) - grid * 0.5;
		float c1 = Hex(p1, radius);

		float2 p2 = _gl_mod(p.xz + grid * 0.5, grid) - grid * 0.5;
		float c2 = Hex(p2, radius);
		return min(c1, c2);
	}
	float3 GuessNormal(float3 p)
	{
		const float d = 0.01;
		return normalize(float3(
			HexGrid(p + float3(d,0.0,0.0)) - HexGrid(p + float3(-d,0.0,0.0)),
			HexGrid(p + float3(0.0,  d,0.0)) - HexGrid(p + float3(0.0, -d,0.0)),
			HexGrid(p + float3(0.0,0.0,  d)) - HexGrid(p + float3(0.0,0.0, -d))));
	}




	void surf(Input IN, inout SurfaceOutput o)
	{
		float2 coord = (IN.screenPos.xy / IN.screenPos.w);

		float3 center = IN.worldPos - _Center;
		float grid_d = HexGrid(center);
		float box = Box(center);
		float grid = grid_d > 0.0 ? 1.0 : 0.0;
		float3 n = GuessNormal(center);
		n = mul(UNITY_MATRIX_VP, float4(n,0.0)).xyz;
		float circle = Circle(center);

		o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
		o.Alpha = 1.0;
		o.Emission = 0.0;
		o.Emission += _GridColor * (grid * box) * 3.0;
	}

	half4 LightingSimplePhong(SurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
	{
		half NdotL = max(0, dot(s.Normal, lightDir));
		float3 R = normalize(-lightDir + 2.0 * s.Normal * NdotL);
		float3 spec = pow(max(0, dot(R, viewDir)), 10.0);

		half4 c;
		c.rgb = s.Albedo * _LightColor0.rgb * NdotL + spec + fixed4(0.1f, 0.1f, 0.1f, 1);
		c.a = s.Alpha;
		return c;
	}


	ENDCG
	}
		FallBack "Diffuse"
}
