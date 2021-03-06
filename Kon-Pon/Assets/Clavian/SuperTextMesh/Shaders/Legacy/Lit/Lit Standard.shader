//Copyright (c) 2016-2018 Kai Clavier [kaiclavier.com] Do Not Distribute
Shader "Super Text Mesh/Legacy/Lit/Standard" { 
	Properties {
		_MainTex ("Font Texture", 2D) = "white" {}
		_MaskTex ("Mask Texture", 2D) = "white" {}
        [HideInInspector] _Cutoff ("Cutoff", Range(0,1)) = 0.0001
	}
    
	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Lighting On Cull Off ZWrite Off Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM
        #include "../STMStandard.cginc"
        #pragma surface surf Standard alphatest:_Cutoff addshadow noforwardadd
        ENDCG
	}
}