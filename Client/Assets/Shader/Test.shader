Shader "Custom/Test"
{
        Properties{
            _MyColor ("MainColor", color) = (0,0,1,0)
            //_MainTex ("MainTexture", 2D) = "white" {}
        }


    SubShader
    {

        Pass{
            Blend SrcAlpha OneMinusSrcAlpha
            Material
            {
                Diffuse [_MyColor]
                Ambient [_MyColor]
            }
            Lighting On


            CGPROGRAM
            #pragma vertex VS_MAIN
            #pragma fragment PS_MAIN
            
            float4 _MyColor;
            sampler2D  _MyTexture;

             struct VS_IN {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct VS_OUT {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            VS_OUT VS_MAIN(VS_IN V_IN)
            {
                VS_OUT o;
                o.pos = UnityObjectToClipPos(V_IN.pos);
                o.uv = V_IN.uv;
                o.color = V_IN.color;
                return o;
            }

            struct PS_IN
            {   
	            float4		vPosition : SV_POSITION;
	            float2		vTexUV : TEXCOORD0;
                float4 color : COLOR;
            };


            float4 PS_MAIN(PS_IN  In_ps) : SV_Target
            {
                return float4(1,0,0,1);



                float4 new_color = tex2D(_MyTexture,In_ps.vTexUV) + float4(0.5f,0.5f,0.5f,0);

                if(new_color.a == 0)
                    discard;
                    new_color.a -= 0.5f;
            }

            ENDCG


            //SetTexture [_MyTexture]
            //{
            //    Combine texture * primary  double
            //}
            Color(1,1,1,1)
        }
    }
    FallBack "Diffuse"
}

