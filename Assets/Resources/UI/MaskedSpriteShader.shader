Shader "Custom/MaskedRawImage"
{
    Properties
    {
        _MainTex("Sprite Texture", 2D) = "white" {}
    }
        SubShader
    {
        Tags { "Queue" = "Overlay" "RenderType" = "Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 clipPos : TEXCOORD1; // For RectMask2D clipping
            };

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.clipPos = o.vertex; // Store position for clipping
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // If the current pixel is outside the RectMask2D bounds, discard it
                if (i.clipPos.x < -1 || i.clipPos.x > 1 || i.clipPos.y < -1 || i.clipPos.y > 1)
                    discard;

            // Get the color from the texture
            fixed4 col = tex2D(_MainTex, i.uv);
            return col;
        }
        ENDCG
    }
    }
        Fallback "Unlit/Transparent"
}
