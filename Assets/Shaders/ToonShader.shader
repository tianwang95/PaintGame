// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/ToonShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_TintColor ("Tint Color", Color) = (0.5, 0.5, 0.5, 1)
		_SpecColor("Specular Color", Color) = (1, 1, 1, 1)
		_OutlineColor("Outline Color", Color) = (0, 0, 0, 1)
		_DiffuseThreshold("Diffuse Threshold", Range(-1.0, 1.0)) = 0.0
		_Shininess("Shininess", Range(0.0, 100)) = 0.0
//		_MainTex ("Albedo (RGB)", 2D) = "white" {}
//		_TintTex ("Tint (RGB)", 2D) = "gray" {}
//		_HighlightTex("Highlight (RGB)", 2D) = "white" {}
//		_ShininessTex("Shininess", 2D) = "black {}
		_NormalsThickness ("Push Normals", Range(0.0, 0.2)) = 0.1
		_ScaleThickness ("Push Scale", Range(0.0, 0.2)) = 0.0
	}
	SubShader {
		Pass { //outline pass
			Cull Front

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			uniform float _NormalsThickness;
			uniform float _ScaleThickness;
			uniform float4 _OutlineColor;

			struct vertexInput {
				float4 vertex: POSITION;
				float3 normal: NORMAL;
			};

			struct vertexOutput {
				float4 vertex: SV_POSITION;
				float3 normal: NORMAL;
			};

			vertexOutput vert(vertexInput input)
			{
				vertexOutput output;
				float scale = 1.0 + _ScaleThickness;
				output.vertex = mul(UNITY_MATRIX_MVP, input.vertex * float4(scale,scale,scale, 1.0) + float4(input.normal, 0.0) * _NormalsThickness);
				return output;
			}

			float4 frag(void) : COLOR
			{
				return _OutlineColor;
			}
			ENDCG
		}

		Pass { //actual pass
			Tags {"LightMode" = "ForwardBase"}
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase

			#include "UnityCG.cginc"
			#include "AutoLight.cginc"

			uniform half4 _LightColor0;
			uniform half4 _Color;
			uniform half4 _TintColor;
			uniform half4 _SpecColor;
			uniform half _DiffuseThreshold;
			uniform float _Shininess;

			struct vertexInput {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct vertexOutput {
				float4 pos : SV_POSITION;
				float3 worldPos : TEXCOORD0;
				float3 normalDir : TEXCOORD1;
				LIGHTING_COORDS(2, 3)
			};

			vertexOutput vert(vertexInput input)
			{
				float4x4 modelMatrix = unity_ObjectToWorld;
				float4x4 modelMatrixInverse = unity_WorldToObject;

				vertexOutput output;
				output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
				output.worldPos = mul(modelMatrix, input.vertex).xyz;
				output.normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
				TRANSFER_VERTEX_TO_FRAGMENT(output);
				return output;
			}

			half4 frag(vertexOutput input) : COLOR
			{
				half3 normalDir = normalize(input.normalDir);
				half3 viewDir = normalize(_WorldSpaceCameraPos - input.worldPos);
				half3 lightDir;
				half attenuation;

				if (0.0 == _WorldSpaceLightPos0.w) {
					attenuation = round(LIGHT_ATTENUATION(input));
					lightDir = normalize(_WorldSpaceLightPos0.xyz);
				} else {
					half3 vectorToLightSource = _WorldSpaceLightPos0.xyz - input.worldPos;
					lightDir = normalize(vectorToLightSource);
					attenuation = 1.0 / length(vectorToLightSource); //linear attenuation ?
				}
				//compute diffuse
				half3 color = _Color.rgb * _TintColor.rgb; //unlit color
				half nDotL = dot(normalDir, lightDir);
				if (attenuation * nDotL > _DiffuseThreshold) { //in the light
					color = _Color.rgb * attenuation * _LightColor0.rgb;
				}
				float specIntensity = attenuation * pow(max(0.0, dot(reflect(-lightDir, normalDir), viewDir)), _Shininess);
				if (nDotL > 0.0 && specIntensity > 0.5) { //in the light
					color = attenuation * _SpecColor.a * _LightColor0.rgb * _SpecColor.rgb + (1.0 - attenuation * _SpecColor.a) * color;
				}
				return half4(color, 1.0);
			}
			ENDCG
		}


		Pass { //actual pass
			Tags {"LightMode" = "ForwardAdd"}
			Blend One One
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform half4 _LightColor0;
			uniform half4 _Color;
			uniform half4 _TintColor;
			uniform half4 _SpecColor;
			uniform half _DiffuseThreshold;
			uniform float _Shininess;

			struct vertexInput {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct vertexOutput {
				float4 pos : SV_POSITION;
				float3 worldPos : TEXCOORD0;
				float3 normalDir : TEXCOORD1;
			};

			vertexOutput vert(vertexInput input)
			{
				float4x4 modelMatrix = unity_ObjectToWorld;
				float4x4 modelMatrixInverse = unity_WorldToObject;

				vertexOutput output;
				output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
				output.worldPos = mul(modelMatrix, input.vertex).xyz;
				output.normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
				return output;
			}

			half4 frag(vertexOutput input) : COLOR
			{
				half3 normalDir = normalize(input.normalDir);
				half3 viewDir = normalize(_WorldSpaceCameraPos - input.worldPos);
				half3 lightDir;
				half attenuation;

				if (0.0 == _WorldSpaceLightPos0.w) {
					attenuation = 1.0;
					lightDir = normalize(_WorldSpaceLightPos0.xyz);
				} else {
					half3 vectorToLightSource = _WorldSpaceLightPos0.xyz - input.worldPos;
					lightDir = normalize(vectorToLightSource);
					attenuation = min(1.0, 1.0 / length(vectorToLightSource)); //linear attenuation ?
				}
				//compute diffuse
				half3 color = half3(0.0, 0.0, 0.0); //unlit color
				half nDotL = dot(normalDir, lightDir);
				if (attenuation * nDotL > _DiffuseThreshold) { //in the light
					color = _Color.rgb * attenuation * _LightColor0.rgb;
				}
				float specIntensity = attenuation * pow(max(0.0, dot(reflect(-lightDir, normalDir), viewDir)), _Shininess);
				if (nDotL > 0.0 && specIntensity > 0.5) { //in the light
					color = attenuation * _SpecColor.a * _LightColor0.rgb * _SpecColor.rgb + (1.0 - attenuation * _SpecColor.a) * color;
				}
				return half4(color, 1.0);
			}
			ENDCG
		}

	}
	FallBack "Diffuse"
}