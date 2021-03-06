﻿Shader "Custom/Water" {
	Properties
	{
		_Color ("Color", Color) = (1, 1, 1, 1)
		_MainTex("Color (RGB) Alpha (A)", 2D) = "white" {}
		_SpecColor("Specular Color", Color) = (0.5, 0.5, 0.5, 0)
		_Shininess("Shininess", Range(0.01, 1)) = 0.078125
		_TextureScale("Texture Scale", Float) = 5
	}
	SubShader
	{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent"  }

		Blend One One

		CGPROGRAM
		#pragma surface surf BlinnPhong vertex:vert alpha

		sampler2D _MainTex;
		//float _WaveHeight;
		//half _WaveScale;
		float _TextureScale;
		fixed4 _Color;
		half _Shininess;
		half _WaveTime;


		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			float3 worldPos = mul(_Object2World, v.vertex).xyz;
			//v.vertex.y = sin(_WaveTime + worldPos.x + worldPos.z) * _WaveHeight;
			v.vertex.y = sin(_WaveTime + worldPos.x);
			o.worldPos = worldPos;
		}

		void surf(Input IN, inout SurfaceOutput o) 
		{
			fixed4 tex = tex2D(_MainTex, float2(IN.worldPos.x, IN.worldPos.z) * _TextureScale);
			o.Albedo = tex.rgb * _Color.rgb;
			o.Gloss = tex.a;
			o.Alpha = tex.a * _Color.a;
			o.Specular = _Shininess * clamp(1 - IN.worldPos.y,0,1);
		}

		ENDCG
	}
	Fallback "Diffuse"
}