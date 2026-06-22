Shader "Unlit/InteractableHighlight"
{
    Properties
    {
        _HighlightColor ("Color", Color) = (0, 1, 1, 1)
        _FresnelPower ("Fresnel Power", Float) = 2.0
        _Intensidad ("Intensidad", Float) = 1.5
    }
    SubShader
    {
        Tags { "Queue"="Transparent+1" "RenderType"="Transparent" }

        Pass
        {
            ZTest Always
            ZWrite Off
            Cull Back
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float4 _HighlightColor;
            float _FresnelPower;
            float _Intensidad;

            struct appdata { float4 vertex : POSITION; float3 normal : NORMAL; };
            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
                float3 viewDir : TEXCOORD1;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldNormal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.viewDir = normalize(_WorldSpaceCameraPos - worldPos);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float fresnel = pow(1.0 - saturate(dot(normalize(i.worldNormal), normalize(i.viewDir))), _FresnelPower);
                return float4(_HighlightColor.rgb, fresnel * _HighlightColor.a * _Intensidad);
            }
            ENDCG
        }
    }
}
