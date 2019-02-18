Shader "Custom/InsideTVWall" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200
		
		ZTest Always
		ZWrite Off

        Stencil
        {
            Ref 0
            Comp always
			Pass replace
			Fail replace
        }
        ColorMask 0
     
        Pass{}
	}
}
