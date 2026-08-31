Shader "Custom/URP/SoftCelGradientOutline"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _ShadowColor ("Shadow Color", Color) = (0.45,0.45,0.45,1)

        _LightThreshold ("Light Threshold", Range(0,1)) = 0.5
        _Softness ("Softness", Range(0.001,1)) = 0.25

        _GradientColorA ("Gradient Color A", Color) = (1,0.45,0.25,1)
        _GradientColorB ("Gradient Color B", Color) = (0.2,0.55,1,1)
        _GradientStrength ("Gradient Strength", Range(0,1)) = 0.5
        _GradientScale ("Gradient Scale", Range(0.01,10)) = 0.4
        _GradientOffset ("Gradient Offset", Float) = 0.5

        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth ("Outline Width", Range(0,0.1)) = 0.025
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Opaque"
            "Queue"="Geometry"
        }

        // ---------- OUTLINE PASS ----------
        Pass
        {
            Name "Outline"
            Cull Front

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            float4 _OutlineColor;
            float _OutlineWidth;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
            };

            Varyings vert (Attributes IN)
            {
                Varyings OUT;

                // Push the mesh outward along its normals
                float3 pushedPosition = IN.positionOS.xyz + IN.normalOS * _OutlineWidth;

                OUT.positionHCS = TransformObjectToHClip(pushedPosition);
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                return _OutlineColor;
            }

            ENDHLSL
        }

        // ---------- MAIN VISUAL PASS ----------
        Pass
        {
            Name "SoftCelGradient"
            Tags { "LightMode"="UniversalForward" }
            Cull Back

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            float4 _BaseColor;
            float4 _ShadowColor;

            float _LightThreshold;
            float _Softness;

            float4 _GradientColorA;
            float4 _GradientColorB;
            float _GradientStrength;
            float _GradientScale;
            float _GradientOffset;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 normalWS : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
            };

            Varyings vert (Attributes IN)
            {
                Varyings OUT;

                float3 worldPos = TransformObjectToWorld(IN.positionOS.xyz);

                OUT.positionHCS = TransformWorldToHClip(worldPos);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.positionWS = worldPos;

                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                Light mainLight = GetMainLight();

                float3 normal = normalize(IN.normalWS);
                float3 lightDir = normalize(mainLight.direction);

                float lightAmount = dot(normal, lightDir);
                lightAmount = saturate(lightAmount);

                // Soft cel transition instead of hard shadow edge
                float softLight = smoothstep(
                    _LightThreshold - _Softness,
                    _LightThreshold + _Softness,
                    lightAmount
                );

                float3 toonColor = lerp(_ShadowColor.rgb, _BaseColor.rgb, softLight);

                // World-space vertical gradient
                float gradientValue = IN.positionWS.y * _GradientScale + _GradientOffset;
                gradientValue = saturate(gradientValue);

                float3 gradientColor = lerp(
                    _GradientColorA.rgb,
                    _GradientColorB.rgb,
                    gradientValue
                );

                float3 finalColor = lerp(
                    toonColor,
                    toonColor * gradientColor,
                    _GradientStrength
                );

                return half4(finalColor, 1);
            }

            ENDHLSL
        }

        // ---------- SHADOW CASTER PASS ----------
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode"="ShadowCaster" }

            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull Back

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
            };

            Varyings vert (Attributes IN)
            {
                Varyings OUT;

                float3 positionWS = TransformObjectToWorld(IN.positionOS.xyz);

                OUT.positionHCS = TransformWorldToHClip(positionWS);

                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                return 0;
            }

            ENDHLSL
        }
    }
}