Shader "Maldan/MainShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _DiffPower ("Diff Power", Range(0, 1)) = 0.5
        _Brightness ("Brightness", Range(0, 2)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "LightMode"="ForwardBase" }
        LOD 200
        
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"    
                
            float4 _Color;
            float _DiffPower;
            float _Brightness;
            
            struct appdata
            {
                float4 vertex : POSITION;
                float4 normal : NORMAL;
                float4 color : COLOR;
            };
    
            struct v2f {
                float4 vertex : SV_POSITION;
                fixed4 diff : COLOR0;
                fixed4 color : COLOR1;
            };
            
            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                
                float3 lightDir = WorldSpaceLightDir(v.vertex);
                half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                half nl = max(0, dot(worldNormal, lightDir.xyz)) * 0.5 + 0.5;
                o.diff = nl * _LightColor0;
                o.color = v.color;
                    
                return o;
            }
                
            fixed4 frag(v2f i) : SV_TARGET {
                float4 sex = lerp(float4(1, 1, 1, 1), float4(((i.diff * 0.5) + 0.5)), _DiffPower);
                float4 outColor = _Color * sex * i.color * float4(_Brightness, _Brightness, _Brightness, 1);
                return outColor;
            }
            
            ENDCG
        }
    }
    FallBack "Diffuse"
}
