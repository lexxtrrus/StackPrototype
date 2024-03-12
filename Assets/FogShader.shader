Shader "Custom/FogShader"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,0)
        _FogDensity ("Fog Density", Range(0.0, 1.0)) = 0.5
        _NoiseTex ("Noise Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            sampler2D _NoiseTex;
            float4 _Color;
            float _FogDensity;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag(v2f i) : SV_TARGET
            {
                half4 noise = tex2D(_NoiseTex, i.uv);
                return saturate(_Color * (1.0 - i.pos.z * _FogDensity * noise.r));
            }
            ENDCG
        }
    }
}