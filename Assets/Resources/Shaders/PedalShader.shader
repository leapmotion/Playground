Shader "Custom/PedalShader" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "Queue" = "Transparent" }
    Pass {
      Blend One One
      SetTexture [_MainTex] { combine texture }
    }
	} 
	FallBack "Diffuse"
}
