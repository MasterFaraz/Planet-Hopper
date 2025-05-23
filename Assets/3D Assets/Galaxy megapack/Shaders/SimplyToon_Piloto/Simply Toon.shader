// Made with Amplify Shader Editor v1.9.6.3
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Piloto Studio/Simply Toon"
{
	Properties
	{
		_MainTex("Base Color", 2D) = "white" {}
		[HDR]_RimColor("Rim Color", Color) = (0,0.5549643,1,0)
		_RimOffset("Rim Offset", Float) = 0.24
		_RimFalloff("Rim Falloff", Vector) = (0,0,0,0)
		_RimShadow("Rim Shadow", Range( 0 , 1)) = 0
		_Dimming("Dimming", Range( 0 , 1)) = 0.75
		_BandingBias("Banding Bias", Float) = 2
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityCG.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
		#pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
		#pragma multi_compile _ _FORWARD_PLUS
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float4 uv_texcoord;
			float3 worldNormal;
			float3 worldPos;
			float4 vertexColor : COLOR;
		};

		uniform float _Dimming;
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform float _BandingBias;
		uniform float _RimShadow;
		uniform float2 _RimFalloff;
		uniform float _RimOffset;
		uniform float4 _RimColor;


struct Gradient
{
	int type;
	int colorsLength;
	int alphasLength;
	float4 colors[8];
	float2 alphas[8];
	};


Gradient NewGradient(int type, int colorsLength, int alphasLength, 
		float4 colors0, float4 colors1, float4 colors2, float4 colors3, float4 colors4, float4 colors5, float4 colors6, float4 colors7,
		float2 alphas0, float2 alphas1, float2 alphas2, float2 alphas3, float2 alphas4, float2 alphas5, float2 alphas6, float2 alphas7)
{
	Gradient g;
	g.type = type;
	g.colorsLength = colorsLength;
	g.alphasLength = alphasLength;
	g.colors[ 0 ] = colors0;
	g.colors[ 1 ] = colors1;
	g.colors[ 2 ] = colors2;
	g.colors[ 3 ] = colors3;
	g.colors[ 4 ] = colors4;
	g.colors[ 5 ] = colors5;
	g.colors[ 6 ] = colors6;
	g.colors[ 7 ] = colors7;
	g.alphas[ 0 ] = alphas0;
	g.alphas[ 1 ] = alphas1;
	g.alphas[ 2 ] = alphas2;
	g.alphas[ 3 ] = alphas3;
	g.alphas[ 4 ] = alphas4;
	g.alphas[ 5 ] = alphas5;
	g.alphas[ 6 ] = alphas6;
	g.alphas[ 7 ] = alphas7;
	return g;
}


float4 SampleGradient( Gradient gradient, float time )
{
	float3 color = gradient.colors[0].rgb;
	UNITY_UNROLL
	for (int c = 1; c < 8; c++)
	{
	float colorPos = saturate((time - gradient.colors[c-1].w) / ( 0.00001 + (gradient.colors[c].w - gradient.colors[c-1].w)) * step(c, (float)gradient.colorsLength-1));
	color = lerp(color, gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), gradient.type));
	}
	#ifndef UNITY_COLORSPACE_GAMMA
	color = half3(GammaToLinearSpaceExact(color.r), GammaToLinearSpaceExact(color.g), GammaToLinearSpaceExact(color.b));
	#endif
	float alpha = gradient.alphas[0].x;
	UNITY_UNROLL
	for (int a = 1; a < 8; a++)
	{
	float alphaPos = saturate((time - gradient.alphas[a-1].y) / ( 0.00001 + (gradient.alphas[a].y - gradient.alphas[a-1].y)) * step(a, (float)gradient.alphasLength-1));
	alpha = lerp(alpha, gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), gradient.type));
	}
	return float4(color, alpha);
}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float4 tex2DNode111 = tex2D( _MainTex, uv_MainTex );
			Gradient gradient117 = NewGradient( 1, 3, 2, float4( 0.754717, 0.754717, 0.754717, 0.5335928 ), float4( 0.8490566, 0.8490566, 0.8490566, 0.862974 ), float4( 1, 1, 1, 0.9558862 ), 0, 0, 0, 0, 0, float2( 1, 0 ), float2( 1, 1 ), 0, 0, 0, 0, 0, 0 );
			float3 ase_worldNormal = i.worldNormal;
			float3 ase_normWorldNormal = normalize( ase_worldNormal );
			float3 worldNormal85 = ase_normWorldNormal;
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult98 = dot( worldNormal85 , ase_worldlightDir );
			float mainLight86 = _BandingBias;
			float3 _Vector1 = float3(0,0,0);
			float3 temp_output_89_0 = _Vector1;
			float3 break101 = temp_output_89_0;
			float temp_output_158_0 = ( i.uv_texcoord.z + 1.0 );
			o.Albedo = ( ( _Dimming * ( ( tex2DNode111 * SampleGradient( gradient117, ( ( (dotResult98*0.26 + 0.5) * max( max( mainLight86 , 0.0 ) , 0.0 ) ) + max( max( break101.x , break101.y ) , break101.z ) ) ) ) + float4( temp_output_89_0 , 0.0 ) ) ) * i.vertexColor * temp_output_158_0 ).rgb;
			float lerpResult109 = lerp( 1.0 , 100.0 , _RimShadow);
			float3 ase_worldViewDir = Unity_SafeNormalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float dotResult103 = dot( ase_worldViewDir , worldNormal85 );
			float smoothstepResult114 = smoothstep( _RimFalloff.x , _RimFalloff.y , ( 1.0 - saturate( ( dotResult103 + _RimOffset ) ) ));
			o.Emission = ( i.vertexColor * ( lerpResult109 * smoothstepResult114 * _RimColor * tex2DNode111 ) * temp_output_158_0 ).rgb;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float4 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
				half4 color : COLOR0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xyzw = customInputData.uv_texcoord;
				o.customPack1.xyzw = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.color = v.color;
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xyzw;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				surfIN.vertexColor = IN.color;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19603
Node;AmplifyShaderEditor.CommentaryNode;81;-3438.521,126.0212;Inherit;False;559.7151;313.7492;;2;138;85;Normals;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldNormalVector;138;-3359.181,174.7088;Inherit;True;True;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;148;-3254.107,-112.8287;Inherit;False;Property;_BandingBias;Banding Bias;6;0;Create;True;0;0;0;False;0;False;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;83;-2509.482,920.2213;Inherit;False;1483;654.4785;;15;137;132;131;127;122;118;116;114;109;106;103;99;93;90;150;Rim Color;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;86;-3082.169,-96.41857;Inherit;False;mainLight;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;85;-3121.806,176.0212;Inherit;False;worldNormal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;106;-2459.482,1126.7;Float;False;World;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;137;-2492.482,1291.7;Inherit;True;85;worldNormal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;87;-2651.482,-169.6129;Inherit;True;85;worldNormal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;88;-2517.354,442.0768;Inherit;False;86;mainLight;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;103;-2203.482,1207.7;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;99;-2203.482,1319.7;Float;False;Property;_RimOffset;Rim Offset;2;0;Create;True;0;0;0;False;0;False;0.24;0.58;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;91;-2549.354,266.0768;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.FunctionNode;89;-2414.482,-204.3004;Inherit;False;SRP Additional Light;-1;;18;6c86746ad131a0a408ca599df5f40861;8,212,0,6,1,9,1,23,0,24,0,142,0,168,0,154,0;6;2;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;15;FLOAT3;0,0,0;False;14;FLOAT3;1,1,1;False;18;FLOAT;0.5;False;32;FLOAT4;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;108;-2549.354,58.077;Inherit;True;85;worldNormal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BreakToComponentsNode;107;-2326.551,442.0768;Inherit;False;FLOAT;1;0;FLOAT;0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleAddOpNode;93;-1979.481,1207.7;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;98;-2245.357,122.0766;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;101;-2151.585,-56.88834;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMaxOpNode;102;-2065.59,439.5322;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;90;-1851.481,1207.7;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;104;-1909.356,458.0767;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;105;-1891.818,-58.23917;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;94;-2084.357,122.0766;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0.26;False;2;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;127;-1778.321,1082.832;Inherit;False;Property;_RimShadow;Rim Shadow;4;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;150;-1621.804,995.0319;Inherit;False;Constant;_Float1;Float 1;6;0;Create;True;0;0;0;False;0;False;100;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;132;-1707.481,1207.7;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;122;-1787.481,1287.7;Inherit;False;Property;_RimFalloff;Rim Falloff;3;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;-1717.356,122.0766;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;96;-1735.584,-40.88821;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;109;-1451.481,1031.7;Inherit;False;3;0;FLOAT;1;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;114;-1515.481,1207.7;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;-0.11;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;116;-1515.481,1367.7;Inherit;False;Property;_RimColor;Rim Color;1;1;[HDR];Create;True;0;0;0;False;0;False;0,0.5549643,1,0;1.498039,1.498039,1.498039,1;True;True;0;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SimpleAddOpNode;133;-1485.719,75.22994;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GradientNode;117;-1531.481,-57.30045;Inherit;False;1;3;2;0.754717,0.754717,0.754717,0.5335928;0.8490566,0.8490566,0.8490566,0.862974;1,1,1,0.9558862;1,0;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.SamplerNode;111;-1796.405,-412.6855;Inherit;True;Property;_MainTex;Base Color;0;0;Create;False;0;0;0;False;0;False;-1;None;70c9529d91fe35349bd48d06513fb028;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;118;-1195.481,1191.7;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GradientSampleNode;112;-1307.481,-56.30045;Inherit;True;2;0;OBJECT;;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;152;-101.0345,1224.77;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;113;-877.8829,-400.0163;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;126;-594.4814,-451.3005;Inherit;False;Property;_Dimming;Dimming;5;0;Create;True;0;0;0;False;0;False;0.75;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;151;-69.32932,1214.424;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;121;-527.2615,-215.2277;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;159;-465.753,626.3489;Inherit;False;0;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;134;-283.6254,-442.3395;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;154;-472.3899,443.938;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;153;-68.15305,985.575;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;158;-220.5013,715.5699;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;131;-1807.69,965.2213;Inherit;False;86;mainLight;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;155;-3.062012,673.1316;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;156;-81.6852,-440.0697;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;162;434.1303,-386.5932;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Piloto Studio/Simply Toon;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;17;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;16;FLOAT4;0,0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;86;0;148;0
WireConnection;85;0;138;0
WireConnection;103;0;106;0
WireConnection;103;1;137;0
WireConnection;89;11;87;0
WireConnection;107;0;88;0
WireConnection;93;0;103;0
WireConnection;93;1;99;0
WireConnection;98;0;108;0
WireConnection;98;1;91;0
WireConnection;101;0;89;0
WireConnection;102;0;107;0
WireConnection;90;0;93;0
WireConnection;104;0;102;0
WireConnection;105;0;101;0
WireConnection;105;1;101;1
WireConnection;94;0;98;0
WireConnection;132;0;90;0
WireConnection;92;0;94;0
WireConnection;92;1;104;0
WireConnection;96;0;105;0
WireConnection;96;1;101;2
WireConnection;109;1;150;0
WireConnection;109;2;127;0
WireConnection;114;0;132;0
WireConnection;114;1;122;1
WireConnection;114;2;122;2
WireConnection;133;0;92;0
WireConnection;133;1;96;0
WireConnection;118;0;109;0
WireConnection;118;1;114;0
WireConnection;118;2;116;0
WireConnection;118;3;111;0
WireConnection;112;0;117;0
WireConnection;112;1;133;0
WireConnection;152;0;118;0
WireConnection;113;0;111;0
WireConnection;113;1;112;0
WireConnection;151;0;152;0
WireConnection;121;0;113;0
WireConnection;121;1;89;0
WireConnection;134;0;126;0
WireConnection;134;1;121;0
WireConnection;153;0;151;0
WireConnection;158;0;159;3
WireConnection;155;0;154;0
WireConnection;155;1;153;0
WireConnection;155;2;158;0
WireConnection;156;0;134;0
WireConnection;156;1;154;0
WireConnection;156;2;158;0
WireConnection;162;0;156;0
WireConnection;162;2;155;0
ASEEND*/
//CHKSM=8D6CF37E36A67D07FBCDBDE8482EAC9F16DE6927