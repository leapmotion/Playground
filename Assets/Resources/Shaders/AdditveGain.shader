Shader "LeapMotion/AdditiveGain" {
  Properties {
    _MainTex("Texture", 2D) = "white" {}
    _Color("Color", Color) = (1.0, 1.0, 1.0, 1.0)
    _Gain("Gain", Range(0.0, 5000.0)) = 1.0
  }
  SubShader {
    Tags { "Queue" = "Transparent" }

    CGPROGRAM
#pragma surface surf Lambert alpha
      struct Input {
        float2 uv_MainTex;
        float3 worldRefl;
        float3 viewDir;
        INTERNAL_DATA
      };

    sampler2D _MainTex;
    float4 _Color;
    float _Gain;

    void surf (Input IN, inout SurfaceOutput o) {
      o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * _Gain;
      o.Emission = tex2D(_MainTex, IN.uv_MainTex).rgb * _Color.rgb * _Gain;
      o.Alpha = tex2D(_MainTex, IN.uv_MainTex).a * _Color.a;
    }
    ENDCG
  }
  Fallback "VertexLit"
}
