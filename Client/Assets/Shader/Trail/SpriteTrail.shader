Shader "Custom/SpriteTrail"
{
    Properties
    {
        _Color ("TrailColor", Color) = (1,1,1,1)
        _MainTex ("SpriteTexture", 2D) = "white" {}
    }

    SubShader
    {
    

       Pass{
            Blend SrcAlpha OneMinusSrcAlpha
            Cull off
            CGPROGRAM
            #pragma vertex VS_MAIN
            #pragma fragment PS_MAIN
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"

            float4 _Color;
            sampler2D  _MainTex;

            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(float4,    _ColorIns )
            UNITY_INSTANCING_BUFFER_END(Props)


            struct VS_IN {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;

                //UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct VS_OUT {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;

                //UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            VS_OUT VS_MAIN(VS_IN V_IN)
            {
                VS_OUT o;
                o.pos = UnityObjectToClipPos(V_IN.pos);
                o.uv = V_IN.uv;
                return o;
            }


            struct PS_IN
            {   
	            float4		vPosition : SV_POSITION;
	            float2		vTexUV : TEXCOORD0;
            };


            float4 PS_MAIN(PS_IN  In_ps) : SV_Target
            {
                float4 pixelColor = tex2D(_MainTex,In_ps.vTexUV);
                if(pixelColor.a == 0)
                    discard;
                float4 co = UNITY_ACCESS_INSTANCED_PROP(Props, _ColorIns);
                pixelColor += co;
                pixelColor.a = co.a;
                return (0,0,0,1);
            }

            ENDCG
            }
    }
    FallBack "Diffuse"
}
