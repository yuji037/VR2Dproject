Shader "Custom/InsideTV" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
        Tags { "RenderType"="StencilOpaque"}
        //LOD 200
		
		//ZTest Always
		ZWrite Off
		Cull front

        Stencil
        {
            Ref 1
            Comp notequal
			Pass replace
        }
        ColorMask 0
     
        Pass{}
	}
}
