Shader "Hidden/NoiseFx"
{
    Properties
    {
        _MainTex   ("Base",      2D)    = ""{}
        _GlitchTex ("Glitch",    2D)    = ""{}
        _BufferTex ("Buffer",    2D)    = ""{}
        _Intensity ("Intensity", Float) = 1
    }
    
    CGINCLUDE

    #include "UnityCG.cginc"
    
    sampler2D _MainTex;
    sampler2D _GlitchTex;
    sampler2D _BufferTex;
    float _Intensity;

    float4 frag(v2f_img i) : SV_Target 
    {
        float4 glitch = tex2D(_GlitchTex, i.uv);

        float thresh = 1.001 - _Intensity * 1.001;
        float w_d = step(thresh, pow(glitch.z, 2.5)); // Displacement glitch
        float w_b = step(thresh, pow(glitch.w, 2.5)); // Buffer glitch
        float w_c = step(thresh, pow(glitch.z, 2)); // Color glitch

        // Displacement.
        float2 uv = i.uv + glitch.xy * w_d;
        float4 source = tex2D(_MainTex, uv);

        // Mix with a buffer.
        float3 color = lerp(source, tex2D(_BufferTex, uv), w_b).rgb;

        // Shuffle color components.
        color = lerp(color, color - source.bbg + color.grr, w_c);

        return float4(color, source.a);
    }

    ENDCG 
    
    Subshader
    {
        Pass
        {
            ZTest Always Cull Off ZWrite Off
            Fog { Mode off }      
            CGPROGRAM
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma vertex vert_img
            #pragma fragment frag
            ENDCG
        }
    }
}
