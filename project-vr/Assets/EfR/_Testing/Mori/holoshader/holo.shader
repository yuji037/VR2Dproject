﻿Shader "EfRchan/Holo"
{
	Properties
	{
	_Pos("AppearBorderPos", Range(-100.0, 100.0)) = 0.0
	_App("IsAppearing", Range(0.0, 1.0)) = 1.0

		_Color("Main Color", Color) = (1, 1, 1, 1)
		_ShadowColor("Shadow Color", Color) = (0.8, 0.8, 1, 1)
		_SpecularPower("Specular Power", Float) = 20
		_EdgeThickness("Outline Thickness", Float) = 1

		_MainTex("Diffuse", 2D) = "white" {}
	_FalloffSampler("Falloff Control", 2D) = "white" {}
	_RimLightSampler("RimLight Control", 2D) = "white" {}
	_SpecularReflectionSampler("Specular / Reflection Mask", 2D) = "white" {}
	_EnvMapSampler("Environment Map", 2D) = "" {}
	_NormalMapSampler("Normal Map", 2D) = "" {}

		_StencilReferenceID("Stencil ID Reference", Float) = 1
		[Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp("Stencil Comparison", Float) = 3
		[Enum(UnityEngine.Rendering.StencilOp)] _StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255
	}

		CGINCLUDE
#include "UnityCG.cginc"
#include "AutoLight.cginc"
		ENDCG

		SubShader
	{
		Tags
	{
		"RenderType" = "Opaque"
		"Queue" = "Geometry"
		"LightMode" = "ForwardBase"
	}

		LOD 450
			
		Stencil
		{
			Ref[_StencilReferenceID]
			Comp[_StencilComp]	// equal
			Pass[_StencilOp]	// keep
			ReadMask[_StencilReadMask]
			WriteMask[_StencilWriteMask]
		}

		Pass
	{
		Cull Back
		ZTest LEqual
		CGPROGRAM
#pragma multi_compile_fwdbase
#pragma target 3.0
#pragma vertex vert
#pragma fragment frag
#define ENABLE_CAST_SHADOWS
#define ENABLE_NORMAL_MAP
#define ENABLE_SPECULAR
#define ENABLE_REFLECTION
#define ENABLE_RIMLIGHT
#include "CharaMain.cg"
		ENDCG
	}

		Pass
	{
		Cull Front
		ZTest Less
		CGPROGRAM
#pragma target 3.0
#pragma vertex vert
#pragma fragment frag
#include "CharaOutline.cg"
		ENDCG
	}

	}

		SubShader
	{
		Tags
	{
		"RenderType" = "Opaque"
		"Queue" = "Geometry"
		"LightMode" = "ForwardBase"
	}

		LOD 400

		Pass
	{
		Cull Back
		ZTest LEqual
		CGPROGRAM
#pragma multi_compile_fwdbase
#pragma target 3.0
#pragma vertex vert
#pragma fragment frag
#define ENABLE_CAST_SHADOWS
#define ENABLE_SPECULAR
#define ENABLE_RIMLIGHT
#include "CharaMain.cg"
		ENDCG
	}

		Pass
	{
		Cull Front
		ZTest Less
		CGPROGRAM
#pragma target 3.0
#pragma vertex vert
#pragma fragment frag
#include "CharaOutline.cg"
		ENDCG
	}

	}

		SubShader
	{
		Tags
	{
		"RenderType" = "Opaque"
		"Queue" = "Geometry"
		"LightMode" = "ForwardBase"
	}

		LOD 300

		Pass
	{
		Cull Back
		ZTest LEqual
		CGPROGRAM
#pragma multi_compile_fwdbase
#pragma target 3.0
#pragma vertex vert
#pragma fragment frag
#define ENABLE_CAST_SHADOWS
#define ENABLE_SPECULAR
#define ENABLE_RIMLIGHT
#include "CharaMain.cg"
		ENDCG
	}

	}

		SubShader
	{
		Tags
	{
		"RenderType" = "Opaque"
		"Queue" = "Geometry"
		"LightMode" = "ForwardBase"
	}

		LOD 250

		Pass
	{
		Cull Back
		ZTest LEqual
		CGPROGRAM
#pragma multi_compile_fwdbase
#pragma target 3.0
#pragma vertex vert
#pragma fragment frag
#define ENABLE_CAST_SHADOWS
#define ENABLE_RIMLIGHT
#include "CharaMain.cg"
		ENDCG
	}

	}

		SubShader
	{
		Tags
	{
		"RenderType" = "Opaque"
		"Queue" = "Geometry"
	}

		LOD 200

		Pass
	{
		Cull Back
		ZTest LEqual
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "Unlit.cg"
		ENDCG
	}

	}

		FallBack "Diffuse"
}
