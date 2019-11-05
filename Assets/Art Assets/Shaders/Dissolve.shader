Shader "Custom/Dissolve"
{
    Properties{
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
		_EmissionColor("EmissionColor", Color) = (1,1,1,1)
		_EmissionMap ("Emission Map", 2D) = "white" {}
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0
 
        _DissolvePercentage("DissolvePercentage", Range(0,1)) = 0.0
        _ShowTexture("ShowTexture", Range(0,1)) = 0.0
    }
        SubShader{
        Tags{ "RenderType" = "Opaque" }
        LOD 200
 
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows
 
                // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
 
        sampler2D _MainTex;
		sampler2D _EmissionMap;
 
    struct Input 
    {
        float2 uv_MainTex;
    };
 
    half _Glossiness;
    half _Metallic;
    half _DissolvePercentage;
    half _ShowTexture;
    fixed4 _Color;
	fixed4 _EmissionColor;
 
    void surf(Input IN, inout SurfaceOutputStandard o)
    {       
        // Albedo comes from a texture tinted by color
        half gradient = tex2D(_MainTex, IN.uv_MainTex).r;
		//half gradient = tex2D(_MainTex, IN.worldPos.rg).r;
        clip(gradient- _DissolvePercentage);
 
        fixed4 c = lerp(1, gradient, _ShowTexture) * _Color;
        o.Albedo = c.rgb;
		o.Emission = _EmissionColor;
        // Metallic and smoothness come from slider variables
        o.Metallic = _Metallic;
        o.Smoothness = _Glossiness;
        o.Alpha = c.a;

		
		half4 emission = tex2D(_EmissionMap, IN.uv_MainTex) * _EmissionColor;
		o.Emission = emission;
    }
    ENDCG
    }
        FallBack "Diffuse"
}