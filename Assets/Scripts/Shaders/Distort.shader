Shader "Unlit/Distort"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _frq("Frequency", Float) = 1
        _amp("Amplitude", Float) = 1
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #define PI 3.14159
            #define TWOPI 6.28318
            #define HALFPI 1.570796

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _frq,_amp;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float rand(float t){
                return frac(sin(t*5143.234)*1234.342);
            }
            float rand2(float t){
                float i=floor(t);
                return lerp(rand(i),rand(i+1),frac(t));
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 center=float2(.5,.5);
                float2 pst=i.uv*2-1;
                float theta=(atan2(pst.y,pst.x)+PI)/TWOPI;
                float distortion=clamp(rand2(_frq*theta),0.01,1)*_amp+1;
                i.uv=(i.uv-center)*distortion+center;
                //i.uv.x=clamp(i.uv.x,0.01,1);
                //i.uv.y=clamp(i.uv.y,0.01,1);
                
                fixed4 col=tex2D(_MainTex, i.uv)*step(0,i.uv.x)*step(0,i.uv.y)*step(i.uv.x,1)*step(i.uv.y,1);
                return col;
            }
            ENDCG
        }
    }
}
