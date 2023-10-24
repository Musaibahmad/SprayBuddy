Shader "Paint in 3D/P3D Solid" {
	Properties {
		[NoScaleOffset] _MainTex ("Albedo (RGB) Alpha (A)", 2D) = "white" {}
		[NoScaleOffset] [Normal] _BumpMap ("Normal (RGBA)", 2D) = "bump" {}
		[NoScaleOffset] _MetallicGlossMap ("Metallic (R) Occlusion (G) Smoothness (B)", 2D) = "white" {}
		[NoScaleOffset] _EmissionMap ("Emission (RGB)", 2D) = "white" {}
		_Color ("Color", Vector) = (1,1,1,1)
		_BumpScale ("Normal Map Strength", Range(0, 5)) = 1
		_Metallic ("Metallic", Range(0, 1)) = 0
		_GlossMapScale ("Smoothness", Range(0, 1)) = 1
		_Emission ("Emission", Vector) = (0,0,0,1)
		_Tiling ("Tiling", Float) = 1
		[NoScaleOffset] _AlbedoTex ("Secondary Albedo (RGB~A) Premultiplied", 2D) = "black" {}
		[NoScaleOffset] _OpacityTex ("Secondary Opacity (R~A) Premultiplied", 2D) = "black" {}
		[NoScaleOffset] _NormalTex ("Secondary Normal (RG~A) Premultiplied", 2D) = "black" {}
		[NoScaleOffset] _MetallicTex ("Secondary Metallic (R~A) Premultiplied", 2D) = "black" {}
		[NoScaleOffset] _OcclusionTex ("Secondary Occlusion (R~A) Premultiplied", 2D) = "black" {}
		[NoScaleOffset] _SmoothnessTex ("Secondary Smoothness (R~A) Premultiplied", 2D) = "black" {}
		[NoScaleOffset] _EmissionTex ("Secondary Emission (RGB~A) Premultiplied", 2D) = "black" {}
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Standard"
}