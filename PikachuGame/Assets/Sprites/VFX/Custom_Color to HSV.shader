Shader "Custom/Color to HSV"
{
  Properties
  {
    _MainTex ("Texture", 2D) = "white" {}
  }
  SubShader
  {
    Tags
    { 
      "CanUseSpriteAtlas" = "true"
      "IGNOREPROJECTOR" = "true"
      "PreviewType" = "Plane"
      "QUEUE" = "Transparent"
      "RenderType" = "Transparent"
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "CanUseSpriteAtlas" = "true"
        "IGNOREPROJECTOR" = "true"
        "PreviewType" = "Plane"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      ZWrite Off
      Cull Off
      Blend SrcAlpha OneMinusSrcAlpha
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _MainTex_ST;
      uniform sampler2D _MainTex;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 color :COLOR;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_COLOR :COLOR;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_COLOR :COLOR;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float4 tmpvar_1;
          tmpvar_1.w = 1;
          tmpvar_1.xyz = in_v.vertex.xyz;
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_1));
          out_v.xlv_COLOR = in_v.color;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tmpvar_1;
          float3 shift_2;
          float4 tmpvar_3;
          tmpvar_3 = tex2D(_MainTex, in_f.xlv_TEXCOORD0);
          float3 tmpvar_4;
          tmpvar_4.x = (in_f.xlv_COLOR.x * 360);
          tmpvar_4.yz = (in_f.xlv_COLOR.yz * float2(2, 2));
          shift_2 = tmpvar_4;
          float3 tmpvar_5;
          tmpvar_5 = tmpvar_3.xyz;
          float3 RGB_6;
          RGB_6 = tmpvar_5;
          float3 RESULT_7;
          float tmpvar_8;
          tmpvar_8 = ((shift_2.z * shift_2.y) * cos(((shift_2.x * 3.141593) / 180)));
          float tmpvar_9;
          tmpvar_9 = ((shift_2.z * shift_2.y) * sin(((shift_2.x * 3.141593) / 180)));
          RESULT_7.x = ((((((0.299 * shift_2.z) + (0.701 * tmpvar_8)) + (0.168 * tmpvar_9)) * RGB_6.x) + ((((0.587 * shift_2.z) - (0.587 * tmpvar_8)) + (0.33 * tmpvar_9)) * RGB_6.y)) + ((((0.114 * shift_2.z) - (0.114 * tmpvar_8)) - (0.497 * tmpvar_9)) * RGB_6.z));
          RESULT_7.y = ((((((0.299 * shift_2.z) - (0.299 * tmpvar_8)) - (0.328 * tmpvar_9)) * RGB_6.x) + ((((0.587 * shift_2.z) + (0.413 * tmpvar_8)) + (0.035 * tmpvar_9)) * RGB_6.y)) + ((((0.114 * shift_2.z) - (0.114 * tmpvar_8)) + (0.292 * tmpvar_9)) * RGB_6.z));
          RESULT_7.z = ((((((0.299 * shift_2.z) - (0.3 * tmpvar_8)) + (1.25 * tmpvar_9)) * RGB_6.x) + ((((0.587 * shift_2.z) - (0.588 * tmpvar_8)) - (1.05 * tmpvar_9)) * RGB_6.y)) + ((((0.114 * shift_2.z) + (0.886 * tmpvar_8)) - (0.203 * tmpvar_9)) * RGB_6.z));
          float4 tmpvar_10;
          tmpvar_10.xyz = float3(RESULT_7);
          tmpvar_10.w = (tmpvar_3.w * in_f.xlv_COLOR.w);
          tmpvar_1 = tmpvar_10;
          out_f.color = tmpvar_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Sprites/Default"
}
