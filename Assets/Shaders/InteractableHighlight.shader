Shader "Unlit/InteractableHighlight"
{
      Properties
      {
          _Color ("Color", Color) = (0, 1, 1, 1)
          _Grosor ("Grosor", Float) = 0.02
          _FresnelPower ("Fresnel Power", Float) = 2.0
      }
      SubShader
      {
          Tags { "Queue"="Transparent+1" "RenderType"="Transparent" }

          Pass
          {
              ZTest Always
              ZWrite Off
              Cull Front
              Blend SrcAlpha OneMinusSrcAlpha

              CGPROGRAM
              #pragma vertex vert
              #pragma fragment frag
              #include "UnityCG.cginc"

              float4 _Color;
              float _Grosor;
              float _FresnelPower;

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
                  v.vertex.xyz += normalize(v.normal) * _Grosor;
                  o.pos = UnityObjectToClipPos(v.vertex);
                  o.worldNormal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));
                  float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                  o.viewDir = normalize(_WorldSpaceCameraPos - worldPos);
                  return o;
              }

              float4 frag(v2f i) : SV_Target
              {
                  float fresnel = pow(1.0 - saturate(dot(normalize(i.worldNormal), normalize(i.viewDir))),
  _FresnelPower);
                  return float4(_Color.rgb, fresnel * _Color.a);
              }
              ENDCG
          }
      }
}
