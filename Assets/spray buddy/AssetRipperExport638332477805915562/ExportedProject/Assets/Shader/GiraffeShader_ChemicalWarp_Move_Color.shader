Shader "GiraffeShader/ChemicalWarp_Move_Color" {
	Properties {
		_MainTex ("Main Tex", 2D) = "white" {}
		_BumpMap ("Normal Map", 2D) = "bump" {}
		_Distortion ("Distortion", Range(0, 10000)) = 10
		_RefractAmount ("Refract Amount", Range(0, 1)) = 0.5
		_WaveSpeed ("WaveSpeed", Range(-2, 2)) = 1
		_AdditionColor ("AdditionColor", Vector) = (1,1,1,1)
		_LevelOfWaterOffsetScale ("LevelOfWaterOffsetScale", Range(-1, 1)) = 0
		_LevelOfWaterX ("LevelOfWaterX", Float) = 0
		_LevelOfWaterY ("LevelOfWaterY", Float) = 0
		_LevelOfWaterZ ("LevelOfWaterZ", Float) = 0
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
}