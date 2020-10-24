// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "GroundShader"
{
	Properties
	{
		_Scale("Scale", Float) = 1
		_Tex2("Tex2", 2D) = "white" {}
		_Tex3("Tex3", 2D) = "white" {}
		_Tex1("Tex1", 2D) = "white" {}
		_Splatting("Splatting", 2D) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Lambert keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float3 worldPos;
		};

		uniform sampler2D _Splatting;
		uniform sampler2D _Sampler018;
		uniform float4 _Splatting_ST;
		uniform sampler2D _Tex1;
		uniform float _Scale;
		uniform sampler2D _Tex2;
		uniform sampler2D _Tex3;

		void surf( Input i , inout SurfaceOutput o )
		{
			float3 ase_worldPos = i.worldPos;
			float2 appendResult4 = (float2(ase_worldPos.x , ase_worldPos.z));
			float4 tex2DNode13 = tex2D( _Splatting, ( ( appendResult4 * _Splatting_ST.xy ) + _Splatting_ST.zw ) );
			float2 temp_output_5_0 = ( appendResult4 * _Scale );
			float4 tex2DNode2 = tex2D( _Tex1, temp_output_5_0 );
			float4 tex2DNode9 = tex2D( _Tex2, temp_output_5_0 );
			float4 tex2DNode10 = tex2D( _Tex3, temp_output_5_0 );
			float4 weightedBlendVar21 = tex2DNode13;
			float4 weightedBlend21 = ( weightedBlendVar21.x*tex2DNode2 + weightedBlendVar21.y*tex2DNode9 + weightedBlendVar21.z*tex2DNode10 + weightedBlendVar21.w*float4( 0,0,0,0 ) );
			o.Albedo = weightedBlend21.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17500
628;75;1469;981;704.8812;453.8015;1;True;False
Node;AmplifyShaderEditor.WorldPosInputsNode;3;-924.1511,-128.2711;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DynamicAppendNode;4;-728.7739,-29.6973;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureTransformNode;18;-710.9383,-412.7752;Inherit;False;13;False;1;0;SAMPLER2D;_Sampler018;False;2;FLOAT2;0;FLOAT2;1
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-480.8663,-401.3432;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-715.151,124.729;Inherit;False;Property;_Scale;Scale;0;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;20;-282.2324,-311.3149;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-565.0525,17.48308;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;13;-140.8866,-328.8623;Inherit;True;Property;_Splatting;Splatting;5;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-125.571,-60.41962;Inherit;True;Property;_Tex1;Tex1;3;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;9;-127.1153,144.8342;Inherit;True;Property;_Tex2;Tex2;1;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;10;-127.6889,370.4286;Inherit;True;Property;_Tex3;Tex3;2;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;8;-507.3889,-208.4518;Inherit;True;Property;_MainTex;MainTex;4;0;Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.LayeredBlendNode;14;294.3957,192.2432;Inherit;False;6;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SummedBlendNode;21;347.1188,-131.8015;Inherit;False;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;608.7544,-56.44273;Float;False;True;-1;2;ASEMaterialInspector;0;0;Lambert;GroundShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;4;0;3;1
WireConnection;4;1;3;3
WireConnection;19;0;4;0
WireConnection;19;1;18;0
WireConnection;20;0;19;0
WireConnection;20;1;18;1
WireConnection;5;0;4;0
WireConnection;5;1;6;0
WireConnection;13;1;20;0
WireConnection;2;1;5;0
WireConnection;9;1;5;0
WireConnection;10;1;5;0
WireConnection;14;0;13;0
WireConnection;14;2;2;0
WireConnection;14;3;9;0
WireConnection;14;4;10;0
WireConnection;21;0;13;0
WireConnection;21;1;2;0
WireConnection;21;2;9;0
WireConnection;21;3;10;0
WireConnection;0;0;21;0
ASEEND*/
//CHKSM=66E193C9C3DAEDFC54825D79D87352704F2B7226