Shader "Unlit/Shader_ToonShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,0)
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

            v2f vert (appdata v)
            {
                v2f o;
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

                float3 lightFallOff = max(0,dot(lightDirection, normal)); 
                lightFallOff = step(0.1,lightFallOff);
                float3 directDiffuseLight = (waterColor * lightFallOff);

                float3 ambientLight = float3(0.2, 0.1,0.8);
                float3 combinedLight = ambientLight + directDiffuseLight;

                float3 finalLight = (combinedLight * waterColor);
                return float4(finalLight,0);
            }
            ENDCG
        }
    }
}
