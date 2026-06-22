Shader "Colorless/Vignette"
{
    Properties
    {
        _Color ("Color", Color) = (0, 0, 0, 1)
        [Range(0,1)] _Intensidad ("Intensidad", Float) = 0
        [Range(0,1)] _Radio ("Radio (donde empieza a oscurecer)", Float) = 0.35
        [Range(0.001,1)] _Suavidad ("Suavidad del borde", Float) = 0.35
    }
    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Transparent" "IgnoreProjector"="True" }

        Pass
        {
            ZTest Always
            ZWrite Off
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float4 _Color;
            float _Intensidad;
            float _Radio;
            float _Suavidad;

            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
            struct v2f { float4 pos : SV_POSITION; float2 uv : TEXCOORD0; };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                // distancia al centro: 0 en el centro, ~1.41 en las esquinas
                float d = distance(i.uv, float2(0.5, 0.5)) * 2.0;
                float v = smoothstep(_Radio, _Radio + _Suavidad, d);
                return float4(_Color.rgb, v * _Color.a * _Intensidad);
            }
            ENDCG
        }
    }
}
