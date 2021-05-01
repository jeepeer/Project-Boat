Shader "Unlit/Shader_WaterAndWaves"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,0)
        _WaveHeight("Wave Height", Range(0,3)) = 0.4
        _WaveSpeed("Wave Speed", Range(0,1)) = 0.1
        _WaveAmount("Wave Amount", Range(0,100)) = 20
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _WaveHeight;
            float _WaveSpeed;
            float _WaveAmount;

            v2f vert (appdata v)
            {
                v2f o;
                float wave = sin((v.uv.x - _Time.y * _WaveSpeed)* _WaveAmount) * 0.2 + 0.4;
                float wave2 = sin((v.uv.y - _Time.x * _WaveSpeed * 2)* _WaveAmount) * 0.2 + 0.8;
                //last 0.2 + 0.4 smooths out the tops
                v.vertex.y = wave2 * (wave * _WaveHeight);

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = v.normal;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 waterColor = _Color.rgb;
                float3 normal = normalize(i.normal);
				float3 lightDirection = _WorldSpaceLightPos0.xyz;
                float3 lightFallOff = max(0.2,dot(lightDirection, normal)); 
                float3 directDiffuseLight = (waterColor * lightFallOff);

                float wave = sin((i.uv.x - _Time.y * _WaveSpeed) * _WaveAmount) * 0.2 + 0.4;

                return float4(directDiffuseLight,1) + (wave* _WaveHeight);
            }
            ENDCG
        }
    }
}