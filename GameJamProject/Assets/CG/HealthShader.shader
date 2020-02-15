Shader "Custom/HealthShader"
{
	Properties
	{
		//選択色
		_Color("Color", Color) = (1,1,1,1)
		_R("R",Range(0.0,1.0)) = 1.0
	}
		SubShader
	{
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			struct VertexInput {
				float4 pos:  POSITION;    // 3D座標
			};

			//
			struct VertexOutput {
				float4 sv_pos:    SV_POSITION; // 2D座標
			};


			//頂点シェーダー
			VertexOutput vert(VertexInput input) {
				VertexOutput output; //フラグメントシェーダーに渡す構造体の宣言
				output.sv_pos = UnityObjectToClipPos(input.pos); //3D座標を2D座標に変換する
				return output;  //フラグメントシェーダーに渡す
			}

			//フラグメントシェーダー
			float4 frag() : SV_Target{
				return float4(1.0, 1.0, 1.0, 1.0); //赤色を返す
			}
			ENDCG
		}
	}
}
