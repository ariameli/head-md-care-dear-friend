Shader "Custom/URP/Monitor Glossy Screen"
{
    Properties
    {
        _BaseMap ("Screen Texture", 2D) = "white" {}
        _BaseColor ("Base Tint", Color) = (1,1,1,1)

        _GlossColor ("Gloss Color", Color) = (0.8, 1.0, 1.0, 1)
        _GlossStrength ("Gloss Strength", Range(0, 1)) = 0.22
        _GlossWidth ("Gloss Width", Range(0.01, 1)) = 0.18
        _GlossPosition ("Gloss Position", Range(-1, 2)) = 0.45
        _GlossAngle ("Gloss Angle", Range(-3.14, 3.14)) = -0.7

        _FresnelStrength ("Edge Shine Strength", Range(0, 1)) = 0.12
        _FresnelPower ("Edge Shine Power", Range(0.5, 8)) = 3.0
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Opaque"
            "Queue"="Geometry"
        }

        Pass
        {
            Name "MonitorGloss"

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float3 viewDirWS : TEXCOORD2;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float4 _BaseColor;

                float4 _GlossColor;
                float _GlossStrength;
                float _GlossWidth;
                float _GlossPosition;
                float _GlossAngle;

                float _FresnelStrength;
                float _FresnelPower;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                float3 positionWS = TransformObjectToWorld(IN.positionOS.xyz);

                OUT.positionHCS = TransformWorldToHClip(positionWS);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.viewDirWS = GetWorldSpaceViewDir(positionWS);

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float4 baseTex = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);
                float3 color = baseTex.rgb * _BaseColor.rgb;

                // Rotate UVs to create diagonal gloss band
                float2 centeredUV = IN.uv - 0.5;

                float s = sin(_GlossAngle);
                float c = cos(_GlossAngle);

                float2 rotatedUV;
                rotatedUV.x = centeredUV.x * c - centeredUV.y * s;
                rotatedUV.y = centeredUV.x * s + centeredUV.y * c;

                // Soft diagonal light streak
                float glossBand = 1.0 - smoothstep(
                    0.0,
                    _GlossWidth,
                    abs(rotatedUV.x - (_GlossPosition - 0.5))
                );

                // Fake view-angle shine
                float3 normalWS = normalize(IN.normalWS);
                float3 viewDirWS = normalize(IN.viewDirWS);

                float fresnel = pow(1.0 - saturate(dot(normalWS, viewDirWS)), _FresnelPower);

                float glossAmount = glossBand * _GlossStrength;
                float fresnelAmount = fresnel * _FresnelStrength;

                color += _GlossColor.rgb * glossAmount;
                color += _GlossColor.rgb * fresnelAmount;

                return half4(color, baseTex.a);
            }

            ENDHLSL
        }
    }
}