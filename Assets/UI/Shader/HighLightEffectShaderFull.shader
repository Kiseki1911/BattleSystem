Shader "Custom/HighLightSpriteEffectShaderFull"
{
  Properties
  {
    _MainTex ("Texture", 2D) = "white" {}
    _Tex2("HighLight", 2D) ="white" {}
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
      sampler2D _Tex2;
      fixed4 frag (v2f i) : SV_Target
      {
        fixed4 colMain = tex2D(_MainTex,i.uv);
        fixed4 col = tex2D(_Tex2, i.uv);
        // float k = 1 - floor(col.x * col.y * col.z);
        float k = ceil(col.a);
        float base = step((sin(i.uv.y+i.uv.x*0.5 + _Time.w) + 0.99), 0);
        // col.rgb = base * k + col.rgb;
        // col.a = base * k + col.a;
        return float4(colMain.rgb + base * k * float3(1,1,1) + k * 0.3, colMain.a);
      }
      ENDCG
    }
  }
}
