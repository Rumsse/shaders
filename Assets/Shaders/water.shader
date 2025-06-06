Shader "Explorer/water"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        [Enum(UnityEngine.Rendering.BlendMode)]
        _SrcFactor("Src Factor", Float) = 5
        [Enum(UnityEngine.Rendering.BlendMode)]
        _DstFactor("Dst Factor", Float) = 10
        [Enum(UnityEngine.Rendering.BlendOp)]
        _Opp("Operation", Float) = 0

        _UVTex("UV Texture", 2D) = "white" {}

        _DistortionIntensity("Distortion Intensity", Float) = 0
        _DistortionAnimation("Distortion Animation", Vector) = (0,0,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Blend [_SrcFactor] [_DstFactor]
        BlendOp [_Opp]
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
                float4 vertex : SV_POSITION;
                float4 uv1_uv2 : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _UVTex;
            float4 _UVTex_ST;

            float _DistortionIntensity;
            float4 _DistortionAnimation;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv1_uv2.xy = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv1_uv2.zw = TRANSFORM_TEX(v.uv, _UVTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 uvTex = tex2D(_UVTex, i.uv1_uv2.zw);
                fixed4 mainTex = tex2D(_MainTex, i.uv1_uv2.xy + _Time.xx * _DistortionAnimation.xy + uvTex * _DistortionIntensity);

                fixed3 color = mainTex.rgb;
                fixed alpha = 1;
                return fixed4(color,alpha);
            }
            ENDCG
        }
    }
}
