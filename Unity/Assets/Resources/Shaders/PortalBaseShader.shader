// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "PortalBaseShader"
{
	Properties
	{
		_speed("speed", Float) = -2
		_Amplitude("Amplitude", Float) = 1
		_Opacity("Opacity", Float) = 3
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Tint("Tint", Color) = (1,1,1,1)
		_RotationSpeed("RotationSpeed", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard alpha:premul keepalpha noshadow nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform float _speed;
		uniform float _Amplitude;
		uniform sampler2D _TextureSample0;
		uniform float _RotationSpeed;
		uniform float _Opacity;
		uniform float4 _Tint;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float temp_output_4_0 = length( (float2( -1,-1 ) + (v.texcoord.xy - float2( 0,0 )) * (float2( 1,1 ) - float2( -1,-1 )) / (float2( 1,1 ) - float2( 0,0 ))) );
			float mulTime9 = _Time.y * _speed;
			float temp_output_22_0 = ( 1.0 - temp_output_4_0 );
			float3 appendResult7 = (float3(0.0 , ( ( sin( ( ( temp_output_4_0 * 20.0 ) + mulTime9 ) ) * _Amplitude ) * temp_output_22_0 ) , 0.0));
			v.vertex.xyz += appendResult7;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float mulTime32 = _Time.y * _RotationSpeed;
			float cos30 = cos( mulTime32 );
			float sin30 = sin( mulTime32 );
			float2 rotator30 = mul( i.uv_texcoord - float2( 0.5,0.5 ) , float2x2( cos30 , -sin30 , sin30 , cos30 )) + float2( 0.5,0.5 );
			float temp_output_4_0 = length( (float2( -1,-1 ) + (i.uv_texcoord - float2( 0,0 )) * (float2( 1,1 ) - float2( -1,-1 )) / (float2( 1,1 ) - float2( 0,0 ))) );
			float temp_output_15_0 = (0.0 + (temp_output_4_0 - 1.0) * (_Opacity - 0.0) / (0.0 - 1.0));
			o.Emission = ( tex2D( _TextureSample0, rotator30 ) * temp_output_15_0 * _Tint ).rgb;
			o.Alpha = ( temp_output_15_0 * i.vertexColor.a );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17500
696;75;1286;666;2518.762;735.6533;3.478858;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-1192.778,-40.68845;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;3;-941.7779,-191.6884;Inherit;True;5;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;1,1;False;3;FLOAT2;-1,-1;False;4;FLOAT2;1,1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LengthOpNode;4;-608.778,-111.6885;Inherit;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-448.8414,453.3574;Inherit;False;Property;_speed;speed;0;0;Create;True;0;0;False;0;-2;-4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;9;-318.4148,402.0195;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-481.5641,157.3646;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;20;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-602.7786,-208.2053;Inherit;False;Property;_RotationSpeed;RotationSpeed;6;0;Create;True;0;0;False;0;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;13;-126.9381,256.3304;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;6;54.97467,283.6884;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;145.0148,553.2585;Inherit;False;Property;_Amplitude;Amplitude;1;0;Create;True;0;0;False;0;1;0.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;31;-672.7433,-374.2239;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;32;-448.7786,-224.2053;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;292.0915,406.182;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;22;-110.514,598.1151;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;30;-207.2146,-346.3752;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-21.23047,83.74579;Inherit;False;Property;_Opacity;Opacity;3;0;Create;True;0;0;False;0;3;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;18;96.57266,-301.151;Inherit;True;Property;_TextureSample0;Texture Sample 0;4;0;Create;True;0;0;False;0;-1;None;cdf821d9bb7c64cc0885f1b517b9452d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;29;479.3088,-382.2102;Inherit;False;Property;_Tint;Tint;5;0;Create;True;0;0;False;0;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;470.5339,371.8672;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;15;130.6269,-61.87967;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;23;661.2265,82.31595;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;922.3907,101.9033;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;26;832.2889,340.8684;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;932.8374,-56.10113;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleTimeNode;27;652.0857,552.4116;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;19;195.5706,687.6912;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;7;665.1197,267.5464;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-38.37531,834.7362;Inherit;False;Property;_pow;pow;2;0;Create;True;0;0;False;0;2;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1346.976,-82.73643;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;PortalBaseShader;False;False;False;False;False;False;True;True;True;True;True;True;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Premultiply;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;3;1;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;1;0
WireConnection;4;0;3;0
WireConnection;9;0;12;0
WireConnection;5;0;4;0
WireConnection;13;0;5;0
WireConnection;13;1;9;0
WireConnection;6;0;13;0
WireConnection;32;0;33;0
WireConnection;11;0;6;0
WireConnection;11;1;14;0
WireConnection;22;0;4;0
WireConnection;30;0;31;0
WireConnection;30;2;32;0
WireConnection;18;1;30;0
WireConnection;17;0;11;0
WireConnection;17;1;22;0
WireConnection;15;0;4;0
WireConnection;15;4;16;0
WireConnection;24;0;15;0
WireConnection;24;1;23;4
WireConnection;26;0;27;0
WireConnection;28;0;18;0
WireConnection;28;1;15;0
WireConnection;28;2;29;0
WireConnection;19;0;22;0
WireConnection;19;1;20;0
WireConnection;7;1;17;0
WireConnection;0;2;28;0
WireConnection;0;9;24;0
WireConnection;0;11;7;0
ASEEND*/
//CHKSM=CDBB7C854C2040E8AD0ACED68634C6118EA1A67A