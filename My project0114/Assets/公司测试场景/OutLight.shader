Shader "Custom/OutLight"
{
    Properties{
        _SpecularGlass("SpecularGlassStrength", Range(0,64)) = 32
        _ObjColor("ObjColor", color) = (1,1,1,1)
        _RimColor("RimColor", color) = (1,1,1,1)
        _RimStrength("RimStrength", Range(0.0001,3.0)) = 0.1

        _AtmoColor("Atmosphere Color", Color) = (0, 0.4, 1.0, 1)
        _Size("Size", Float) = 0.1
        _OutLightPow("LightPow", Float) = 5
        _OutLightStrength("Strength", Float) = 15
    }
    SubShader
    {
        Tags 
        { 
            "Queue" = "Transparent"       
        }

        Pass  //pass2 实现OutLight
        {
            Name "AtmosphereBase"
            Tags{ "LightMode" = "Always" }
            
            Cull front
            // Blend SrcAlpha One
            Blend One OneMinusSrcAlpha
            ZWrite off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            uniform float _Size;
            uniform float4 _AtmoColor;
            uniform float _OutLightPow;
            uniform float _OutLightStrength;

            struct v2f
            {
                float3 normal : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata_base v)
            {
                v2f o;
                v.vertex.xyz += v.normal * _Size;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = v.normal;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }
            
            fixed4 frag(v2f i) : COLOR
            {
                i.normal = normalize(i.normal);
                //视角观察方向
                float3 viewDir = normalize(i.worldPos - _WorldSpaceCameraPos.xyz);

                float4 color = _AtmoColor;

                color.a = pow(saturate(dot(viewDir, i.normal)), _OutLightPow);
                color.a *= _OutLightStrength*dot(viewDir, i.normal);
                return color;
            }
            ENDCG
        }
    }
}
