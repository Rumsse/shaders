Shader "Explorer/shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Area("Area", vector) = (0,0,4,4)
        _Angle("Angle", range(-3.1415,3.1415)) = 0
        _MaxIter("Iterations", range(4, 1000)) = 255
        _Color("Color", range(0, 1)) = 0.5
        _Repeat("Repeat", float) = 0.5
        _Speed("Speed", float) = 1
        _Symmetry("Symmetry", range(0, 1)) = 1
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float4 _Area;
            float _Angle;
            float _MaxIter;
            float _Color;
            float _Repeat;
            float _Speed;
            float _Symmetry;


            float2 rot(float2 p, float2 pivot, float a) //rot - nazwa float bo rotacja, float p to punkt orginalny, pivot to co wokó³ czego siê obracamy, a a to k¹t (bo angle)
            {
                float s = sin(a);
                float c = cos(a);
                p -= pivot;
                p = float2(p.x * c - p.y * s, p.x * s + p.y * c);
                p += pivot;
                return p;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv - 0.5;
                uv = abs(uv);
                uv = rot(uv, 0, 0.25 * 3.1415);
                uv = abs(uv);
                uv = lerp(i.uv - 0.5, uv, _Symmetry);

                float2 c = _Area.xy + uv * _Area.zw;
                c = rot(c, _Area.xy, _Angle);

                float r = 20;
                float r2 = r*r;

                float2 z;
                float2 zPrevious;
                float iter;

                for(iter = 0; iter < _MaxIter; iter++)
                {
                    zPrevious = rot(z, 0, _Time.y);
                    z = float2(z.x * z.x - z.y * z.y, 2 * z.x * z.y) + c;

                    if(dot(z, zPrevious) > r2) break;
                }

                if (iter >= _MaxIter) 
                {
                    return 0;
                }

                float dist = length (z);
                float fracIter = (dist - r) / (r2 - r);
                fracIter = log2(log(dist) / log(r));
                iter -= fracIter; 

                float m = sqrt(iter / _MaxIter); //jasnoœæ
                float sw = 3;
                float4 col = sin(float4 (0.3, 0.7, 0.8, 1) * m * sw);

                col = tex2D(_MainTex, float2(m * _Repeat + _Time.y * _Speed, _Color));
                float angle = atan2(z.x, z.y);
                col *= smoothstep(3, 0, fracIter);
                col *= 1 + sin(angle * 2 + _Time.y * 4) * 0,2;
                return col;
            }
            ENDCG
        }
    }
}