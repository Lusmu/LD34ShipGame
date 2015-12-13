Shader "Custom/UnderWater" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque"}
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf UnderWater fullforwardshadows vertex:vert

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float depth;
		};

		half _Shininess;
		fixed4 _Color;
		
		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.depth = 1 + 0.1 * clamp(mul(_Object2World, v.vertex).y, -10, 0);
		}

		half4 LightingUnderWater(SurfaceOutput s, half3 lightDir, half atten) {
			half NdotL = dot(s.Normal, lightDir);
			half4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * (NdotL * atten);
			c.a = s.Alpha;
			return c;
		}

		void surf (Input IN, inout SurfaceOutput o) 
		{
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb * IN.depth;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
