Shader "Custom/OutlineGlow"
{
    Properties
    {
        _OutlineColor ("Outline Color", Color) = (1, 0.9, 0.2, 1)
        _OutlineWidth ("Outline Width", Range(0.0, 0.1)) = 0.02
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" "Queue"="Transparent" }

        Pass
        {
            Name "Outline"
            Cull Front // renders the INSIDE of the flipped mesh, creating a rim effect
            ZWrite On

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

            float4 _OutlineColor;
            float _OutlineWidth;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                // push vertices outward along their normal to create the outline shell
                float3 offsetPos = IN.positionOS.xyz + IN.normalOS * _OutlineWidth;
                OUT.positionHCS = TransformObjectToHClip(offsetPos);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                return _OutlineColor;
            }
            ENDHLSL
        }
    }
}
