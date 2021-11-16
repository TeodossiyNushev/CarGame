// EasyRoads3D v3 Shader
// Blends two textures, the first on uv1, the second on UV4


Shader "EasyRoads3D/Rock Blend" {
    Properties {
		
		[Header(Main texture)]
        _MainTex ("Albedo", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _BumpMap ("Normal", 2D) = "bump" {}
        _Metallic ("Metallic", Range(0.0, 1.0)) = 0.0 
        _Glossiness ("Smoothness", Range(0.0, 1.0)) = 0.5  

		[Space][Header(UV 4)]
		_SecondTex ("Albedo", 2D) = "white" {}

    }

    SubShader {
        Tags {
            "SplatCount" = "3"
            "Queue" = "AlphaTest"
            "RenderType" = "TransparentCutout"
        }
    //    Offset -1, -1

        CGPROGRAM
        #pragma surface surf Standard  fullforwardshadows alphatest:_Cutoff
   //		#pragma surface surf Standard  fullforwardshadows Lambert decal:blend
        #pragma target 3.0
        #pragma multi_compile_fog
        #pragma exclude_renderers gles
        #include "UnityPBSLighting.cginc"

        half _depthThresh;

        uniform sampler2D _Control;

        sampler2D _MainTex, _SecondTex, _maskTex;
        sampler2D _BumpMap2, _BumpMap;

        half _Metallic1;

        half _Glossiness1;

		half _Metallic;
		half _Glossiness;

		half _Metallic2;
		half _Glossiness2;

        half _NormalStrengh;

        struct Input {
            float3 worldPos;
            float3 worldNormal;
            float2 uv_MainTex : TEXCOORD0;
            float2 uv4_SecondTex : TEXCOORD1;

            float4 color : COLOR;

            INTERNAL_DATA
        };

        fixed4 _Color, _Color2;
        half _Threshold;

        void surf (Input IN, inout SurfaceOutputStandard o) {

            fixed4 alb = 0.0f;
			float4 c = IN.color;

			float4 _main = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			float4 _main1 = tex2D (_MainTex, IN.uv4_SecondTex) * _Color;
			
            fixed4 nrm = tex2D(_BumpMap, IN.uv_MainTex);
            fixed4 nrm1 = tex2D(_BumpMap, IN.uv4_SecondTex);
			
			// Mesh has vertex color red info, red value will alays be one, green 0..1, so lerp on green color
            o.Normal = UnpackNormal(lerp(nrm, nrm1, c.r));
            o.Albedo = lerp(_main.rgb, _main1.rgb, c.r);
			//o.Albedo = _main.rgb;//c.rgb;
            o.Alpha = 1;
            o.Smoothness = _Glossiness;
            o.Metallic = _Metallic;
        }
        ENDCG
    }

    Fallback "Standard"
}
