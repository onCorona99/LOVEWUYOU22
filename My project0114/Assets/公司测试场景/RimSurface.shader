Shader "Custom/RimLightShader"
{
    Properties
    {
        _MainTexture("主纹理",2D)=""{}
        _RimColor("发光颜色",Color)=(1,0,0,1)
        _RimPower("发光强度",Range(0,1))=1
    }
    SubShader
    {

        CGPROGRAM
        #pragma surface surf Standard Lambert
        
        sampler2D _MainTexture;
        
        fixed4 _RimColor;
        fixed _RimPower;
        
        struct Input
        {
            float3 viewDir;
            fixed2 uv_MainTexture;

        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            //设置纹理
            o.Albedo = tex2D(_MainTexture,IN.uv_MainTexture);
            //发光系数
            float rimRate = 1 - dot(normalize(IN.viewDir),normalize(o.Normal));
            //发光
            o.Emission = _RimColor * rimRate * _RimPower;
        }
      
        ENDCG
    }
    FallBack "Diffuse"
}

