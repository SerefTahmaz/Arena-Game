// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Universal Render Pipeline/Terrain/ProceduralTerrainFirstPass"
{
	Properties
	{
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[HideInInspector]_TerrainHolesTexture("_TerrainHolesTexture", 2D) = "white" {}
		[HideInInspector]_Control("Control", 2D) = "white" {}
		[HideInInspector]_Splat3("Splat3", 2D) = "white" {}
		[HideInInspector]_Splat2("Splat2", 2D) = "white" {}
		[HideInInspector]_Splat1("Splat1", 2D) = "white" {}
		[HideInInspector]_Splat0("Splat0", 2D) = "white" {}
		[HideInInspector]_Normal0("Normal0", 2D) = "white" {}
		[HideInInspector]_Normal1("Normal1", 2D) = "white" {}
		[HideInInspector]_Normal2("Normal2", 2D) = "white" {}
		[HideInInspector]_Normal3("Normal3", 2D) = "white" {}
		[HideInInspector]_Smoothness3("Smoothness3", Range( 0 , 1)) = 1
		[HideInInspector]_Smoothness1("Smoothness1", Range( 0 , 1)) = 1
		[HideInInspector]_Smoothness0("Smoothness0", Range( 0 , 1)) = 1
		[HideInInspector]_Smoothness2("Smoothness2", Range( 0 , 1)) = 1
		[HideInInspector][Gamma]_Metallic0("Metallic0", Range( 0 , 1)) = 0
		[HideInInspector][Gamma]_Metallic2("Metallic2", Range( 0 , 1)) = 0
		[HideInInspector][Gamma]_Metallic3("Metallic3", Range( 0 , 1)) = 0
		[HideInInspector][Gamma]_Metallic1("Metallic1", Range( 0 , 1)) = 0
		[HideInInspector]_Mask2("_Mask2", 2D) = "white" {}
		[HideInInspector]_Mask0("_Mask0", 2D) = "white" {}
		[HideInInspector]_Mask1("_Mask1", 2D) = "white" {}
		[HideInInspector]_Mask3("_Mask3", 2D) = "white" {}
		[ASEBegin]_Specular3("Specular3", Color) = (0,0,0,0)
		_Specular1("Specular1", Color) = (0,0,0,0)
		_Specular0("Specular0", Color) = (0,0,0,0)
		_Specular2("Specular2", Color) = (0,0,0,0)
		_TriOffset("TriOffset", Vector) = (0,0,0,0)
		_TriUVRotation("TriUVRotation", Float) = 0
		_TriplanarMask("Triplanar Mask", 2D) = "white" {}
		_TempTriplanarAlbedo("Temp Triplanar Albedo", 2D) = "white" {}
		_TriTiling("TriTiling", Float) = 0.1
		_TriplanarNormal("Triplanar Normal", 2D) = "bump" {}
		_TempTopAlbedo("Temp Top Albedo", 2D) = "white" {}
		_TopMask("Top Mask", 2D) = "white" {}
		_TopNormal("Top Normal", 2D) = "bump" {}
		[IntRange]_WorldtoObjectSwitch("World to Object Switch", Range( 0 , 1)) = 0
		_CoverageAmount("Coverage Amount", Range( -1 , 1)) = 0
		[ASEEnd]_CoverageFalloff("Coverage Falloff", Range( 0.01 , 2)) = 0.5
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

		[HideInInspector]_QueueOffset("_QueueOffset", Float) = 0
        [HideInInspector]_QueueControl("_QueueControl", Float) = -1
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
		//_TransmissionShadow( "Transmission Shadow", Range( 0, 1 ) ) = 0.5
		//_TransStrength( "Trans Strength", Range( 0, 50 ) ) = 1
		//_TransNormal( "Trans Normal Distortion", Range( 0, 1 ) ) = 0.5
		//_TransScattering( "Trans Scattering", Range( 1, 50 ) ) = 2
		//_TransDirect( "Trans Direct", Range( 0, 1 ) ) = 0.9
		//_TransAmbient( "Trans Ambient", Range( 0, 1 ) ) = 0.1
		//_TransShadow( "Trans Shadow", Range( 0, 1 ) ) = 0.5
		//_TessPhongStrength( "Tess Phong Strength", Range( 0, 1 ) ) = 0.5
		//_TessValue( "Tess Max Tessellation", Range( 1, 32 ) ) = 16
		//_TessMin( "Tess Min Distance", Float ) = 10
		//_TessMax( "Tess Max Distance", Float ) = 25
		//_TessEdgeLength ( "Tess Edge length", Range( 2, 50 ) ) = 16
		//_TessMaxDisp( "Tess Max Displacement", Float ) = 25
	}

	SubShader
	{
		LOD 0

		
		
		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" "Queue"="Geometry-100" }
		Cull Back
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		AlphaToMask Off
		
		HLSLINCLUDE
		#pragma target 3.0

		#pragma prefer_hlslcc gles
		#pragma exclude_renderers d3d11_9x 

		#ifndef ASE_TESS_FUNCS
		#define ASE_TESS_FUNCS
		float4 FixedTess( float tessValue )
		{
			return tessValue;
		}
		
		float CalcDistanceTessFactor (float4 vertex, float minDist, float maxDist, float tess, float4x4 o2w, float3 cameraPos )
		{
			float3 wpos = mul(o2w,vertex).xyz;
			float dist = distance (wpos, cameraPos);
			float f = clamp(1.0 - (dist - minDist) / (maxDist - minDist), 0.01, 1.0) * tess;
			return f;
		}

		float4 CalcTriEdgeTessFactors (float3 triVertexFactors)
		{
			float4 tess;
			tess.x = 0.5 * (triVertexFactors.y + triVertexFactors.z);
			tess.y = 0.5 * (triVertexFactors.x + triVertexFactors.z);
			tess.z = 0.5 * (triVertexFactors.x + triVertexFactors.y);
			tess.w = (triVertexFactors.x + triVertexFactors.y + triVertexFactors.z) / 3.0f;
			return tess;
		}

		float CalcEdgeTessFactor (float3 wpos0, float3 wpos1, float edgeLen, float3 cameraPos, float4 scParams )
		{
			float dist = distance (0.5 * (wpos0+wpos1), cameraPos);
			float len = distance(wpos0, wpos1);
			float f = max(len * scParams.y / (edgeLen * dist), 1.0);
			return f;
		}

		float DistanceFromPlane (float3 pos, float4 plane)
		{
			float d = dot (float4(pos,1.0f), plane);
			return d;
		}

		bool WorldViewFrustumCull (float3 wpos0, float3 wpos1, float3 wpos2, float cullEps, float4 planes[6] )
		{
			float4 planeTest;
			planeTest.x = (( DistanceFromPlane(wpos0, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[0]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.y = (( DistanceFromPlane(wpos0, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[1]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.z = (( DistanceFromPlane(wpos0, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[2]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.w = (( DistanceFromPlane(wpos0, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[3]) > -cullEps) ? 1.0f : 0.0f );
			return !all (planeTest);
		}

		float4 DistanceBasedTess( float4 v0, float4 v1, float4 v2, float tess, float minDist, float maxDist, float4x4 o2w, float3 cameraPos )
		{
			float3 f;
			f.x = CalcDistanceTessFactor (v0,minDist,maxDist,tess,o2w,cameraPos);
			f.y = CalcDistanceTessFactor (v1,minDist,maxDist,tess,o2w,cameraPos);
			f.z = CalcDistanceTessFactor (v2,minDist,maxDist,tess,o2w,cameraPos);

			return CalcTriEdgeTessFactors (f);
		}

		float4 EdgeLengthBasedTess( float4 v0, float4 v1, float4 v2, float edgeLength, float4x4 o2w, float3 cameraPos, float4 scParams )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;
			tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
			tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
			tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
			tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			return tess;
		}

		float4 EdgeLengthBasedTessCull( float4 v0, float4 v1, float4 v2, float edgeLength, float maxDisplacement, float4x4 o2w, float3 cameraPos, float4 scParams, float4 planes[6] )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;

			if (WorldViewFrustumCull(pos0, pos1, pos2, maxDisplacement, planes))
			{
				tess = 0.0f;
			}
			else
			{
				tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
				tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
				tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
				tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			}
			return tess;
		}
		#endif //ASE_TESS_FUNCS

		ENDHLSL

		UsePass "Hidden/Nature/Terrain/Utilities/PICKING"
	UsePass "Hidden/Nature/Terrain/Utilities/SELECTION"

		Pass
		{
			
			Name "Forward"
			Tags { "LightMode"="UniversalForward" }
			
			Blend One Zero, One Zero
			ColorMask RGBA
			

			HLSLPROGRAM

			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define ASE_FINAL_COLOR_ALPHA_MULTIPLY 1
			#define _SPECULAR_SETUP 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 999999
			#define ASE_USING_SAMPLING_MACROS 1


			#pragma multi_compile _ _SCREEN_SPACE_OCCLUSION
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS _ADDITIONAL_OFF
			#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
			
			#pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
			#pragma multi_compile _ SHADOWS_SHADOWMASK

			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile _ DYNAMICLIGHTMAP_ON

			#pragma multi_compile _ _REFLECTION_PROBE_BLENDING
			#pragma multi_compile _ _REFLECTION_PROBE_BOX_PROJECTION
			#pragma multi_compile _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
			#pragma multi_compile _ _LIGHT_LAYERS
			
			#pragma multi_compile _ _LIGHT_COOKIES
			#pragma multi_compile _ _CLUSTERED_RENDERING

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS SHADERPASS_FORWARD

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#if defined(UNITY_INSTANCING_ENABLED) && defined(_TERRAIN_INSTANCED_PERPIXEL_NORMAL)
			    #define ENABLE_TERRAIN_PERPIXEL_NORMAL
			#endif

			#define ASE_NEEDS_VERT_TANGENT
			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_FRAG_POSITION
			#define ASE_NEEDS_FRAG_WORLD_NORMAL
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_VERT_POSITION
			#pragma multi_compile_instancing
			#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap forwardadd
			#pragma multi_compile_local __ _ALPHATEST_ON
			#pragma shader_feature_local _MASKMAP


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 lightmapUVOrVertexSH : TEXCOORD0;
				half4 fogFactorAndVertexLight : TEXCOORD1;
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				float4 shadowCoord : TEXCOORD2;
				#endif
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
				#if defined(ASE_NEEDS_FRAG_SCREEN_POSITION)
				float4 screenPos : TEXCOORD6;
				#endif
				#if defined(DYNAMICLIGHTMAP_ON)
				float2 dynamicLightmapUV : TEXCOORD7;
				#endif
				float4 ase_texcoord8 : TEXCOORD8;
				float4 ase_texcoord9 : TEXCOORD9;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _Control_ST;
			float4 _Specular1;
			float4 _Specular0;
			float4 _TerrainHolesTexture_ST;
			float4 _Specular2;
			float4 _Splat3_ST;
			float4 _Splat2_ST;
			float4 _Specular3;
			float4 _Splat0_ST;
			float4 _Splat1_ST;
			float2 _TriOffset;
			float _Smoothness0;
			float _Smoothness1;
			float _Metallic3;
			float _Smoothness3;
			float _Metallic2;
			float _TriTiling;
			float _TriUVRotation;
			float _WorldtoObjectSwitch;
			float _CoverageAmount;
			float _CoverageFalloff;
			float _Metallic1;
			float _Metallic0;
			float _Smoothness2;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			float4 _MaskMapRemapOffset2;
			float4 _MaskMapRemapScale2;
			float4 _MaskMapRemapScale0;
			float4 _MaskMapRemapOffset3;
			float4 _MaskMapRemapOffset1;
			float4 _MaskMapRemapOffset0;
			float4 _MaskMapRemapScale3;
			float4 _MaskMapRemapScale1;
			TEXTURE2D(_Mask1);
			SAMPLER(sampler_Mask1);
			TEXTURE2D(_Mask2);
			SAMPLER(sampler_Mask2);
			TEXTURE2D(_Mask3);
			SAMPLER(sampler_Mask3);
			TEXTURE2D(_Mask0);
			SAMPLER(sampler_Mask0);
			TEXTURE2D(_Control);
			SAMPLER(sampler_Control);
			float _LayerHasMask0;
			float _LayerHasMask1;
			float _LayerHasMask2;
			float _LayerHasMask3;
			TEXTURE2D(_Splat0);
			TEXTURE2D(_Splat1);
			TEXTURE2D(_Splat2);
			TEXTURE2D(_Splat3);
			SAMPLER(sampler_linear_repeat_aniso4);
			float4 _DiffuseRemapScale0;
			float4 _DiffuseRemapScale1;
			float4 _DiffuseRemapScale2;
			float4 _DiffuseRemapScale3;
			TEXTURE2D(_TerrainHolesTexture);
			SAMPLER(sampler_TerrainHolesTexture);
			TEXTURE2D(_TempTriplanarAlbedo);
			TEXTURE2D(_TempTopAlbedo);
			SAMPLER(sampler_TempTopAlbedo);
			TEXTURE2D(_Normal0);
			TEXTURE2D(_Normal1);
			TEXTURE2D(_Normal2);
			TEXTURE2D(_Normal3);
			TEXTURE2D(_TriplanarNormal);
			TEXTURE2D(_TopNormal);
			SAMPLER(sampler_TopNormal);
			TEXTURE2D(_TriplanarMask);
			TEXTURE2D(_TopMask);
			#ifdef UNITY_INSTANCING_ENABLED//ASE Terrain Instancing
				TEXTURE2D(_TerrainHeightmapTexture);//ASE Terrain Instancing
				TEXTURE2D( _TerrainNormalmapTexture);//ASE Terrain Instancing
				SAMPLER(sampler_TerrainNormalmapTexture);//ASE Terrain Instancing
			#endif//ASE Terrain Instancing
			UNITY_INSTANCING_BUFFER_START( Terrain )//ASE Terrain Instancing
				UNITY_DEFINE_INSTANCED_PROP( float4, _TerrainPatchInstanceData )//ASE Terrain Instancing
			UNITY_INSTANCING_BUFFER_END( Terrain)//ASE Terrain Instancing
			CBUFFER_START( UnityTerrain)//ASE Terrain Instancing
				#ifdef UNITY_INSTANCING_ENABLED//ASE Terrain Instancing
					float4 _TerrainHeightmapRecipSize;//ASE Terrain Instancing
					float4 _TerrainHeightmapScale;//ASE Terrain Instancing
				#endif//ASE Terrain Instancing
			CBUFFER_END//ASE Terrain Instancing


			VertexInput ApplyMeshModification( VertexInput v )
			{
			#ifdef UNITY_INSTANCING_ENABLED
				float2 patchVertex = v.vertex.xy;
				float4 instanceData = UNITY_ACCESS_INSTANCED_PROP( Terrain, _TerrainPatchInstanceData );
				float2 sampleCoords = ( patchVertex.xy + instanceData.xy ) * instanceData.z;
				float height = UnpackHeightmap( _TerrainHeightmapTexture.Load( int3( sampleCoords, 0 ) ) );
				v.vertex.xz = sampleCoords* _TerrainHeightmapScale.xz;
				v.vertex.y = height* _TerrainHeightmapScale.y;
				#ifdef ENABLE_TERRAIN_PERPIXEL_NORMAL
					v.ase_normal = float3(0, 1, 0);
				#else
					v.ase_normal = _TerrainNormalmapTexture.Load(int3(sampleCoords, 0)).rgb* 2 - 1;
				#endif
				v.texcoord.xy = sampleCoords* _TerrainHeightmapRecipSize.zw;
			#endif
				return v;
			}
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				v = ApplyMeshModification(v);
				float3 localCalculateTangentsSRP76_g11 = ( ( v.ase_tangent.xyz * v.ase_normal * 0.0 ) );
				{
				v.ase_tangent.xyz = cross ( v.ase_normal, float3( 0, 0, 1 ) );
				v.ase_tangent.w = -1;
				}
				float3 TangetsAlpha72 = localCalculateTangentsSRP76_g11;
				
				o.ase_texcoord8.xy = v.texcoord.xy;
				o.ase_texcoord9 = v.vertex;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord8.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = TangetsAlpha72;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float3 positionVS = TransformWorldToView( positionWS );
				float4 positionCS = TransformWorldToHClip( positionWS );

				VertexNormalInputs normalInput = GetVertexNormalInputs( v.ase_normal, v.ase_tangent );

				o.tSpace0 = float4( normalInput.normalWS, positionWS.x);
				o.tSpace1 = float4( normalInput.tangentWS, positionWS.y);
				o.tSpace2 = float4( normalInput.bitangentWS, positionWS.z);

				#if defined(LIGHTMAP_ON)
				OUTPUT_LIGHTMAP_UV( v.texcoord1, unity_LightmapST, o.lightmapUVOrVertexSH.xy );
				#endif

				#if defined(DYNAMICLIGHTMAP_ON)
				o.dynamicLightmapUV.xy = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
				#endif

				#if !defined(LIGHTMAP_ON)
				OUTPUT_SH( normalInput.normalWS.xyz, o.lightmapUVOrVertexSH.xyz );
				#endif

				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					o.lightmapUVOrVertexSH.zw = v.texcoord;
					o.lightmapUVOrVertexSH.xy = v.texcoord * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif

				half3 vertexLight = VertexLighting( positionWS, normalInput.normalWS );
				#ifdef ASE_FOG
					half fogFactor = ComputeFogFactor( positionCS.z );
				#else
					half fogFactor = 0;
				#endif
				o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
				
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				VertexPositionInputs vertexInput = (VertexPositionInputs)0;
				vertexInput.positionWS = positionWS;
				vertexInput.positionCS = positionCS;
				o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				
				o.clipPos = positionCS;
				#if defined(ASE_NEEDS_FRAG_SCREEN_POSITION)
				o.screenPos = ComputeScreenPos(positionCS);
				#endif
				return o;
			}
			
			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_tangent = v.ase_tangent;
				o.texcoord = v.texcoord;
				o.texcoord1 = v.texcoord1;
				o.texcoord2 = v.texcoord2;
				
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_tangent = patch[0].ase_tangent * bary.x + patch[1].ase_tangent * bary.y + patch[2].ase_tangent * bary.z;
				o.texcoord = patch[0].texcoord * bary.x + patch[1].texcoord * bary.y + patch[2].texcoord * bary.z;
				o.texcoord1 = patch[0].texcoord1 * bary.x + patch[1].texcoord1 * bary.y + patch[2].texcoord1 * bary.z;
				o.texcoord2 = patch[0].texcoord2 * bary.x + patch[1].texcoord2 * bary.y + patch[2].texcoord2 * bary.z;
				
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			#if defined(ASE_EARLY_Z_DEPTH_OPTIMIZE)
				#define ASE_SV_DEPTH SV_DepthLessEqual  
			#else
				#define ASE_SV_DEPTH SV_Depth
			#endif

			half4 frag ( VertexOutput IN 
						#ifdef ASE_DEPTH_WRITE_ON
						,out float outputDepth : ASE_SV_DEPTH
						#endif
						 ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif

				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					float2 sampleCoords = (IN.lightmapUVOrVertexSH.zw / _TerrainHeightmapRecipSize.zw + 0.5f) * _TerrainHeightmapRecipSize.xy;
					float3 WorldNormal = TransformObjectToWorldNormal(normalize(SAMPLE_TEXTURE2D(_TerrainNormalmapTexture, sampler_TerrainNormalmapTexture, sampleCoords).rgb * 2 - 1));
					float3 WorldTangent = -cross(GetObjectToWorldMatrix()._13_23_33, WorldNormal);
					float3 WorldBiTangent = cross(WorldNormal, -WorldTangent);
				#else
					float3 WorldNormal = normalize( IN.tSpace0.xyz );
					float3 WorldTangent = IN.tSpace1.xyz;
					float3 WorldBiTangent = IN.tSpace2.xyz;
				#endif
				float3 WorldPosition = float3(IN.tSpace0.w,IN.tSpace1.w,IN.tSpace2.w);
				float3 WorldViewDirection = _WorldSpaceCameraPos.xyz  - WorldPosition;
				float4 ShadowCoords = float4( 0, 0, 0, 0 );
				#if defined(ASE_NEEDS_FRAG_SCREEN_POSITION)
				float4 ScreenPos = IN.screenPos;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
					ShadowCoords = IN.shadowCoord;
				#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
					ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
				#endif
	
				WorldViewDirection = SafeNormalize( WorldViewDirection );

				float2 uv_Control = IN.ase_texcoord8.xy * _Control_ST.xy + _Control_ST.zw;
				float4 tex2DNode5_g11 = SAMPLE_TEXTURE2D( _Control, sampler_Control, uv_Control );
				float dotResult20_g11 = dot( tex2DNode5_g11 , float4(1,1,1,1) );
				float SplatWeight22_g11 = dotResult20_g11;
				float localSplatClip74_g11 = ( SplatWeight22_g11 );
				float SplatWeight74_g11 = SplatWeight22_g11;
				{
				#if !defined(SHADER_API_MOBILE) && defined(TERRAIN_SPLAT_ADDPASS)
				clip(SplatWeight74_g11 == 0.0f ? -1 : 1);
				#endif
				}
				float4 SplatControl26_g11 = ( tex2DNode5_g11 / ( localSplatClip74_g11 + 0.001 ) );
				float4 appendResult55_g11 = (float4(_Metallic0 , _Metallic1 , _Metallic2 , _Metallic3));
				float localComputeMasks130_g11 = ( 0.0 );
				float4 masks0130_g11 = float4( 0,0,0,0 );
				float4 masks1130_g11 = float4( 0,0,0,0 );
				float4 masks2130_g11 = float4( 0,0,0,0 );
				float4 masks3130_g11 = float4( 0,0,0,0 );
				float4 appendResult135_g11 = (float4(_LayerHasMask0 , _LayerHasMask1 , _LayerHasMask2 , _LayerHasMask3));
				float4 layerHasMask136_g11 = appendResult135_g11;
				float4 hasMask130_g11 = layerHasMask136_g11;
				float2 uv_Splat0 = IN.ase_texcoord8.xy * _Splat0_ST.xy + _Splat0_ST.zw;
				float2 uvMask0130_g11 = uv_Splat0;
				float2 uv_Splat1 = IN.ase_texcoord8.xy * _Splat1_ST.xy + _Splat1_ST.zw;
				float2 uvMask1130_g11 = uv_Splat1;
				float2 uv_Splat2 = IN.ase_texcoord8.xy * _Splat2_ST.xy + _Splat2_ST.zw;
				float2 uvMask2130_g11 = uv_Splat2;
				float2 uv_Splat3 = IN.ase_texcoord8.xy * _Splat3_ST.xy + _Splat3_ST.zw;
				float2 uvMask3130_g11 = uv_Splat3;
				SamplerState SamplerS130_g11 = sampler_linear_repeat_aniso4;
				{
				masks0130_g11 = 0.5h;
				masks1130_g11 = 0.5h;
				masks2130_g11 = 0.5h;
				masks3130_g11 = 0.5h;
				#ifdef _MASKMAP
				masks0130_g11 = lerp(masks0130_g11, SAMPLE_TEXTURE2D(_Mask0, SamplerS130_g11, uvMask0130_g11), hasMask130_g11.x);
				masks1130_g11 = lerp(masks1130_g11, SAMPLE_TEXTURE2D(_Mask1, SamplerS130_g11, uvMask1130_g11), hasMask130_g11.y);
				masks2130_g11 = lerp(masks2130_g11, SAMPLE_TEXTURE2D(_Mask2, SamplerS130_g11, uvMask2130_g11), hasMask130_g11.z);
				masks3130_g11 = lerp(masks3130_g11, SAMPLE_TEXTURE2D(_Mask3, SamplerS130_g11, uvMask3130_g11), hasMask130_g11.w);
				#endif
				masks0130_g11 *= _MaskMapRemapScale0.rgba;
				masks0130_g11 += _MaskMapRemapOffset0.rgba;
				masks1130_g11 *= _MaskMapRemapScale1.rgba;
				masks1130_g11 += _MaskMapRemapOffset1.rgba;
				masks2130_g11 *= _MaskMapRemapScale2.rgba;
				masks2130_g11 += _MaskMapRemapOffset2.rgba;
				masks3130_g11 *= _MaskMapRemapScale3.rgba;
				masks3130_g11 += _MaskMapRemapOffset3.rgba;
				}
				float4 mask0138_g11 = masks0130_g11;
				float4 mask1139_g11 = masks1130_g11;
				float4 mask2140_g11 = masks2130_g11;
				float4 mask3141_g11 = masks3130_g11;
				float4 appendResult168_g11 = (float4((mask0138_g11).x , (mask1139_g11).x , (mask2140_g11).x , (mask3141_g11).x));
				float4 maskMetallic169_g11 = appendResult168_g11;
				float4 lerpResult202_g11 = lerp( appendResult55_g11 , maskMetallic169_g11 , layerHasMask136_g11);
				float dotResult53_g11 = dot( SplatControl26_g11 , lerpResult202_g11 );
				float temp_output_527_56 = dotResult53_g11;
				float4 appendResult33_g11 = (float4(1.0 , 1.0 , 1.0 , _Smoothness0));
				float4 tex2DNode4_g11 = SAMPLE_TEXTURE2D( _Splat0, sampler_linear_repeat_aniso4, uv_Splat0 );
				float4 appendResult258_g11 = (float4(( (SplatControl26_g11).rrr * (_DiffuseRemapScale0).rgb ) , 1.0));
				float4 tintLayer0253_g11 = appendResult258_g11;
				float4 appendResult36_g11 = (float4(1.0 , 1.0 , 1.0 , _Smoothness1));
				float4 tex2DNode3_g11 = SAMPLE_TEXTURE2D( _Splat1, sampler_linear_repeat_aniso4, uv_Splat1 );
				float4 appendResult261_g11 = (float4(( (SplatControl26_g11).ggg * (_DiffuseRemapScale1).rgb ) , 1.0));
				float4 tintLayer1254_g11 = appendResult261_g11;
				float4 appendResult39_g11 = (float4(1.0 , 1.0 , 1.0 , _Smoothness2));
				float4 tex2DNode6_g11 = SAMPLE_TEXTURE2D( _Splat2, sampler_linear_repeat_aniso4, uv_Splat2 );
				float4 appendResult263_g11 = (float4(( (SplatControl26_g11).bbb * (_DiffuseRemapScale2).rgb ) , 1.0));
				float4 tintLayer2255_g11 = appendResult263_g11;
				float4 appendResult42_g11 = (float4(1.0 , 1.0 , 1.0 , _Smoothness3));
				float4 tex2DNode7_g11 = SAMPLE_TEXTURE2D( _Splat3, sampler_linear_repeat_aniso4, uv_Splat3 );
				float4 appendResult265_g11 = (float4(( (SplatControl26_g11).aaa * (_DiffuseRemapScale3).rgb ) , 1.0));
				float4 tintLayer3256_g11 = appendResult265_g11;
				float4 weightedBlendVar9_g11 = float4(1,1,1,1);
				float4 weightedBlend9_g11 = ( weightedBlendVar9_g11.x*( appendResult33_g11 * tex2DNode4_g11 * tintLayer0253_g11 ) + weightedBlendVar9_g11.y*( appendResult36_g11 * tex2DNode3_g11 * tintLayer1254_g11 ) + weightedBlendVar9_g11.z*( appendResult39_g11 * tex2DNode6_g11 * tintLayer2255_g11 ) + weightedBlendVar9_g11.w*( appendResult42_g11 * tex2DNode7_g11 * tintLayer3256_g11 ) );
				float4 MixDiffuse28_g11 = weightedBlend9_g11;
				float4 temp_output_60_0_g11 = MixDiffuse28_g11;
				float4 localClipHoles100_g11 = ( temp_output_60_0_g11 );
				float2 uv_TerrainHolesTexture = IN.ase_texcoord8.xy * _TerrainHolesTexture_ST.xy + _TerrainHolesTexture_ST.zw;
				float holeClipValue99_g11 = SAMPLE_TEXTURE2D( _TerrainHolesTexture, sampler_TerrainHolesTexture, uv_TerrainHolesTexture ).r;
				float Hole100_g11 = holeClipValue99_g11;
				{
				#ifdef _ALPHATEST_ON
				clip(Hole100_g11 == 0.0f ? -1 : 1);
				#endif
				}
				float4 AlbedoLayer67 = ( temp_output_527_56 * localClipHoles100_g11 );
				float mTriTiling455 = _TriTiling;
				float2 appendResult352 = (float2(IN.ase_texcoord9.xyz.y , IN.ase_texcoord9.xyz.z));
				float mTriUVRotation493 = _TriUVRotation;
				float cos491 = cos( mTriUVRotation493 );
				float sin491 = sin( mTriUVRotation493 );
				float2 rotator491 = mul( ( mTriTiling455 * appendResult352 ) - float2( 0,0 ) , float2x2( cos491 , -sin491 , sin491 , cos491 )) + float2( 0,0 );
				float2 mTriOffset503 = _TriOffset;
				float2 TriplanarUV_1415 = ( 1.0 - ( rotator491 + mTriOffset503 ) );
				float3 temp_output_275_0 = abs( mul( GetWorldToObjectMatrix(), float4( WorldNormal , 0.0 ) ).xyz );
				float dotResult276 = dot( temp_output_275_0 , float3(1,1,1) );
				float3 BlendComponents278 = ( temp_output_275_0 / dotResult276 );
				float TriCom_1480 = BlendComponents278.x;
				float2 appendResult354 = (float2(IN.ase_texcoord9.xyz.x , IN.ase_texcoord9.xyz.z));
				float cos495 = cos( mTriUVRotation493 );
				float sin495 = sin( mTriUVRotation493 );
				float2 rotator495 = mul( ( mTriTiling455 * appendResult354 ) - float2( 0,0 ) , float2x2( cos495 , -sin495 , sin495 , cos495 )) + float2( 0,0 );
				float2 TriplanarUV_2416 = ( 1.0 - ( rotator495 + mTriOffset503 ) );
				float TriCom_2479 = BlendComponents278.y;
				float2 appendResult351 = (float2(IN.ase_texcoord9.xyz.x , IN.ase_texcoord9.xyz.y));
				float cos496 = cos( mTriUVRotation493 );
				float sin496 = sin( mTriUVRotation493 );
				float2 rotator496 = mul( ( mTriTiling455 * appendResult351 ) - float2( 0,0 ) , float2x2( cos496 , -sin496 , sin496 , cos496 )) + float2( 0,0 );
				float2 TriplanarUV_3417 = ( 1.0 - ( rotator496 + mTriOffset503 ) );
				float TriCom_3477 = BlendComponents278.z;
				float WorldObjectSwitch321 = _WorldtoObjectSwitch;
				float3 lerpResult353 = lerp( WorldPosition , IN.ase_texcoord9.xyz , WorldObjectSwitch321);
				float3 break361 = lerpResult353;
				float2 appendResult365 = (float2(break361.x , break361.z));
				float2 TriplanarTopUV418 = ( ( mTriTiling455 * appendResult365 ) + mTriOffset503 );
				float3 lerpResult342 = lerp( WorldNormal , mul( GetWorldToObjectMatrix(), float4( WorldNormal , 0.0 ) ).xyz , WorldObjectSwitch321);
				float3 temp_cast_11 = (_CoverageFalloff).xxx;
				float TriplanarColorWeight419 = pow( saturate( ( lerpResult342 + _CoverageAmount ) ) , temp_cast_11 ).y;
				float4 lerpResult461 = lerp( ( ( ( SAMPLE_TEXTURE2D( _TempTriplanarAlbedo, sampler_linear_repeat_aniso4, TriplanarUV_1415 ) * TriCom_1480 ) + ( SAMPLE_TEXTURE2D( _TempTriplanarAlbedo, sampler_linear_repeat_aniso4, TriplanarUV_2416 ) * TriCom_2479 ) ) + ( SAMPLE_TEXTURE2D( _TempTriplanarAlbedo, sampler_linear_repeat_aniso4, TriplanarUV_3417 ) * TriCom_3477 ) ) , SAMPLE_TEXTURE2D( _TempTopAlbedo, sampler_TempTopAlbedo, TriplanarTopUV418 ) , TriplanarColorWeight419);
				float4 TempTriAlbedo463 = lerpResult461;
				float4 temp_output_59_0_g11 = SplatControl26_g11;
				float FirstLayerControl66 = temp_output_59_0_g11.x;
				float4 lerpResult81 = lerp( AlbedoLayer67 , TempTriAlbedo463 , FirstLayerControl66);
				
				float4 weightedBlendVar8_g11 = temp_output_59_0_g11;
				float4 weightedBlend8_g11 = ( weightedBlendVar8_g11.x*SAMPLE_TEXTURE2D( _Normal0, sampler_linear_repeat_aniso4, uv_Splat0 ) + weightedBlendVar8_g11.y*SAMPLE_TEXTURE2D( _Normal1, sampler_linear_repeat_aniso4, uv_Splat1 ) + weightedBlendVar8_g11.z*SAMPLE_TEXTURE2D( _Normal2, sampler_linear_repeat_aniso4, uv_Splat2 ) + weightedBlendVar8_g11.w*SAMPLE_TEXTURE2D( _Normal3, sampler_linear_repeat_aniso4, uv_Splat3 ) );
				float3 temp_output_61_0_g11 = UnpackNormalScale( weightedBlend8_g11, 1.0 );
				float3 NormalLayer68 = temp_output_61_0_g11;
				float3 lerpResult448 = lerp( ( ( ( UnpackNormalScale( SAMPLE_TEXTURE2D( _TriplanarNormal, sampler_linear_repeat_aniso4, TriplanarUV_1415 ), 1.0f ) * TriCom_1480 ) + ( UnpackNormalScale( SAMPLE_TEXTURE2D( _TriplanarNormal, sampler_linear_repeat_aniso4, TriplanarUV_2416 ), 1.0f ) * TriCom_2479 ) ) + ( UnpackNormalScale( SAMPLE_TEXTURE2D( _TriplanarNormal, sampler_linear_repeat_aniso4, TriplanarUV_3417 ), 1.0f ) * TriCom_3477 ) ) , UnpackNormalScale( SAMPLE_TEXTURE2D( _TopNormal, sampler_TopNormal, TriplanarTopUV418 ), 1.0f ) , TriplanarColorWeight419);
				float3 TriNormal451 = lerpResult448;
				float3 lerpResult84 = lerp( NormalLayer68 , TriNormal451 , FirstLayerControl66);
				
				float SpecularLayer76 = 0.0;
				float4 temp_cast_15 = (SpecularLayer76).xxxx;
				float4 lerpResult413 = lerp( ( ( ( SAMPLE_TEXTURE2D( _TriplanarMask, sampler_linear_repeat_aniso4, TriplanarUV_1415 ) * TriCom_1480 ) + ( SAMPLE_TEXTURE2D( _TriplanarMask, sampler_linear_repeat_aniso4, TriplanarUV_2416 ) * TriCom_2479 ) ) + ( SAMPLE_TEXTURE2D( _TriplanarMask, sampler_linear_repeat_aniso4, TriplanarUV_3417 ) * TriCom_3477 ) ) , SAMPLE_TEXTURE2D( _TopMask, sampler_linear_repeat_aniso4, TriplanarTopUV418 ) , TriplanarColorWeight419);
				float4 TriSpecular429 = lerpResult413;
				float4 lerpResult87 = lerp( temp_cast_15 , TriSpecular429 , FirstLayerControl66);
				
				float4 appendResult205_g11 = (float4(_Smoothness0 , _Smoothness1 , _Smoothness2 , _Smoothness3));
				float4 appendResult206_g11 = (float4(tex2DNode4_g11.a , tex2DNode3_g11.a , tex2DNode6_g11.a , tex2DNode7_g11.a));
				float4 defaultSmoothness210_g11 = ( appendResult205_g11 * appendResult206_g11 );
				float4 appendResult158_g11 = (float4((mask0138_g11).w , (mask1139_g11).w , (mask2140_g11).w , (mask3141_g11).w));
				float4 maskSmoothness149_g11 = appendResult158_g11;
				float4 lerpResult215_g11 = lerp( defaultSmoothness210_g11 , maskSmoothness149_g11 , layerHasMask136_g11);
				float dotResult216_g11 = dot( lerpResult215_g11 , SplatControl26_g11 );
				float4 weightedBlendVar295_g11 = float4(1,1,1,1);
				float4 weightedBlend295_g11 = ( weightedBlendVar295_g11.x*_Specular0 + weightedBlendVar295_g11.y*_Specular1 + weightedBlendVar295_g11.z*_Specular2 + weightedBlendVar295_g11.w*_Specular3 );
				float mLayerSmootness515 = ( dotResult216_g11 * weightedBlend295_g11.r );
				float TriSmoothness428 = lerpResult413.a;
				float lerpResult90 = lerp( mLayerSmootness515 , TriSmoothness428 , FirstLayerControl66);
				
				float AlphaLayer71 = SplatWeight22_g11;
				
				float3 Albedo = lerpResult81.rgb;
				float3 Normal = lerpResult84;
				float3 Emission = 0;
				float3 Specular = lerpResult87.rgb;
				float Metallic = 0;
				float Smoothness = lerpResult90;
				float Occlusion = 1;
				float Alpha = AlphaLayer71;
				float AlphaClipThreshold = 0.5;
				float AlphaClipThresholdShadow = 0.5;
				float3 BakedGI = 0;
				float3 RefractionColor = 1;
				float RefractionIndex = 1;
				float3 Transmission = 1;
				float3 Translucency = 1;
				#ifdef ASE_DEPTH_WRITE_ON
				float DepthValue = 0;
				#endif
				
				#ifdef _CLEARCOAT
				float CoatMask = 0;
				float CoatSmoothness = 0;
				#endif


				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				InputData inputData = (InputData)0;
				inputData.positionWS = WorldPosition;
				inputData.viewDirectionWS = WorldViewDirection;
				

				#ifdef _NORMALMAP
					#if _NORMAL_DROPOFF_TS
					inputData.normalWS = TransformTangentToWorld(Normal, half3x3( WorldTangent, WorldBiTangent, WorldNormal ));
					#elif _NORMAL_DROPOFF_OS
					inputData.normalWS = TransformObjectToWorldNormal(Normal);
					#elif _NORMAL_DROPOFF_WS
					inputData.normalWS = Normal;
					#endif
					inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);
				#else
					inputData.normalWS = WorldNormal;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
					inputData.shadowCoord = ShadowCoords;
				#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
					inputData.shadowCoord = TransformWorldToShadowCoord(inputData.positionWS);
				#else
					inputData.shadowCoord = float4(0, 0, 0, 0);
				#endif


				#ifdef ASE_FOG
					inputData.fogCoord = IN.fogFactorAndVertexLight.x;
				#endif

				inputData.vertexLighting = IN.fogFactorAndVertexLight.yzw;
				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					float3 SH = SampleSH(inputData.normalWS.xyz);
				#else
					float3 SH = IN.lightmapUVOrVertexSH.xyz;
				#endif

				#if defined(DYNAMICLIGHTMAP_ON)
				inputData.bakedGI = SAMPLE_GI(IN.lightmapUVOrVertexSH.xy, IN.dynamicLightmapUV.xy, SH, inputData.normalWS);
				#else
				inputData.bakedGI = SAMPLE_GI( IN.lightmapUVOrVertexSH.xy, SH, inputData.normalWS );
				#endif

				#ifdef _ASE_BAKEDGI
					inputData.bakedGI = BakedGI;
				#endif
				
				inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(IN.clipPos);
				inputData.shadowMask = SAMPLE_SHADOWMASK(IN.lightmapUVOrVertexSH.xy);

				#if defined(DEBUG_DISPLAY)
					#if defined(DYNAMICLIGHTMAP_ON)
						inputData.dynamicLightmapUV = IN.dynamicLightmapUV.xy;
					#endif

					#if defined(LIGHTMAP_ON)
						inputData.staticLightmapUV = IN.lightmapUVOrVertexSH.xy;
					#else
						inputData.vertexSH = SH;
					#endif
				#endif

				SurfaceData surfaceData;
				surfaceData.albedo              = Albedo;
				surfaceData.metallic            = saturate(Metallic);
				surfaceData.specular            = Specular;
				surfaceData.smoothness          = saturate(Smoothness),
				surfaceData.occlusion           = Occlusion,
				surfaceData.emission            = Emission,
				surfaceData.alpha               = saturate(Alpha);
				surfaceData.normalTS            = Normal;
				surfaceData.clearCoatMask       = 0;
				surfaceData.clearCoatSmoothness = 1;


				#ifdef _CLEARCOAT
					surfaceData.clearCoatMask       = saturate(CoatMask);
					surfaceData.clearCoatSmoothness = saturate(CoatSmoothness);
				#endif

				#ifdef _DBUFFER
					ApplyDecalToSurfaceData(IN.clipPos, surfaceData, inputData);
				#endif

				half4 color = UniversalFragmentPBR( inputData, surfaceData);

				#ifdef _TRANSMISSION_ASE
				{
					float shadow = _TransmissionShadow;

					Light mainLight = GetMainLight( inputData.shadowCoord );
					float3 mainAtten = mainLight.color * mainLight.distanceAttenuation;
					mainAtten = lerp( mainAtten, mainAtten * mainLight.shadowAttenuation, shadow );
					half3 mainTransmission = max(0 , -dot(inputData.normalWS, mainLight.direction)) * mainAtten * Transmission;
					color.rgb += Albedo * mainTransmission;

					#ifdef _ADDITIONAL_LIGHTS
						int transPixelLightCount = GetAdditionalLightsCount();
						for (int i = 0; i < transPixelLightCount; ++i)
						{
							Light light = GetAdditionalLight(i, inputData.positionWS);
							float3 atten = light.color * light.distanceAttenuation;
							atten = lerp( atten, atten * light.shadowAttenuation, shadow );

							half3 transmission = max(0 , -dot(inputData.normalWS, light.direction)) * atten * Transmission;
							color.rgb += Albedo * transmission;
						}
					#endif
				}
				#endif

				#ifdef _TRANSLUCENCY_ASE
				{
					float shadow = _TransShadow;
					float normal = _TransNormal;
					float scattering = _TransScattering;
					float direct = _TransDirect;
					float ambient = _TransAmbient;
					float strength = _TransStrength;

					Light mainLight = GetMainLight( inputData.shadowCoord );
					float3 mainAtten = mainLight.color * mainLight.distanceAttenuation;
					mainAtten = lerp( mainAtten, mainAtten * mainLight.shadowAttenuation, shadow );

					half3 mainLightDir = mainLight.direction + inputData.normalWS * normal;
					half mainVdotL = pow( saturate( dot( inputData.viewDirectionWS, -mainLightDir ) ), scattering );
					half3 mainTranslucency = mainAtten * ( mainVdotL * direct + inputData.bakedGI * ambient ) * Translucency;
					color.rgb += Albedo * mainTranslucency * strength;

					#ifdef _ADDITIONAL_LIGHTS
						int transPixelLightCount = GetAdditionalLightsCount();
						for (int i = 0; i < transPixelLightCount; ++i)
						{
							Light light = GetAdditionalLight(i, inputData.positionWS);
							float3 atten = light.color * light.distanceAttenuation;
							atten = lerp( atten, atten * light.shadowAttenuation, shadow );

							half3 lightDir = light.direction + inputData.normalWS * normal;
							half VdotL = pow( saturate( dot( inputData.viewDirectionWS, -lightDir ) ), scattering );
							half3 translucency = atten * ( VdotL * direct + inputData.bakedGI * ambient ) * Translucency;
							color.rgb += Albedo * translucency * strength;
						}
					#endif
				}
				#endif

				#ifdef _REFRACTION_ASE
					float4 projScreenPos = ScreenPos / ScreenPos.w;
					float3 refractionOffset = ( RefractionIndex - 1.0 ) * mul( UNITY_MATRIX_V, float4( WorldNormal,0 ) ).xyz * ( 1.0 - dot( WorldNormal, WorldViewDirection ) );
					projScreenPos.xy += refractionOffset.xy;
					float3 refraction = SHADERGRAPH_SAMPLE_SCENE_COLOR( projScreenPos.xy ) * RefractionColor;
					color.rgb = lerp( refraction, color.rgb, color.a );
					color.a = 1;
				#endif

				#ifdef ASE_FINAL_COLOR_ALPHA_MULTIPLY
					color.rgb *= color.a;
				#endif

				#ifdef ASE_FOG
					#ifdef TERRAIN_SPLAT_ADDPASS
						color.rgb = MixFogColor(color.rgb, half3( 0, 0, 0 ), IN.fogFactorAndVertexLight.x );
					#else
						color.rgb = MixFog(color.rgb, IN.fogFactorAndVertexLight.x);
					#endif
				#endif

				#ifdef ASE_DEPTH_WRITE_ON
					outputDepth = DepthValue;
				#endif

				return color;
			}

			ENDHLSL
		}

		UsePass "Hidden/Nature/Terrain/Utilities/PICKING"
	UsePass "Hidden/Nature/Terrain/Utilities/SELECTION"

		Pass
		{
			
			Name "ShadowCaster"
			Tags { "LightMode"="ShadowCaster" }

			ZWrite On
			ZTest LEqual
			AlphaToMask Off
			ColorMask 0

			HLSLPROGRAM
			
			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define ASE_FINAL_COLOR_ALPHA_MULTIPLY 1
			#define _SPECULAR_SETUP 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 999999
			#define ASE_USING_SAMPLING_MACROS 1

			
			#pragma vertex vert
			#pragma fragment frag

			#pragma multi_compile _ _CASTING_PUNCTUAL_LIGHT_SHADOW

			#define SHADERPASS SHADERPASS_SHADOWCASTER

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_VERT_POSITION
			#pragma multi_compile_instancing
			#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap forwardadd
			#pragma multi_compile_local __ _ALPHATEST_ON
			#pragma shader_feature_local _MASKMAP


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _Control_ST;
			float4 _Specular1;
			float4 _Specular0;
			float4 _TerrainHolesTexture_ST;
			float4 _Specular2;
			float4 _Splat3_ST;
			float4 _Splat2_ST;
			float4 _Specular3;
			float4 _Splat0_ST;
			float4 _Splat1_ST;
			float2 _TriOffset;
			float _Smoothness0;
			float _Smoothness1;
			float _Metallic3;
			float _Smoothness3;
			float _Metallic2;
			float _TriTiling;
			float _TriUVRotation;
			float _WorldtoObjectSwitch;
			float _CoverageAmount;
			float _CoverageFalloff;
			float _Metallic1;
			float _Metallic0;
			float _Smoothness2;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			float4 _MaskMapRemapOffset2;
			float4 _MaskMapRemapScale2;
			float4 _MaskMapRemapScale0;
			float4 _MaskMapRemapOffset3;
			float4 _MaskMapRemapOffset1;
			float4 _MaskMapRemapOffset0;
			float4 _MaskMapRemapScale3;
			float4 _MaskMapRemapScale1;
			TEXTURE2D(_Mask1);
			SAMPLER(sampler_Mask1);
			TEXTURE2D(_Mask2);
			SAMPLER(sampler_Mask2);
			TEXTURE2D(_Mask3);
			SAMPLER(sampler_Mask3);
			TEXTURE2D(_Mask0);
			SAMPLER(sampler_Mask0);
			TEXTURE2D(_Control);
			SAMPLER(sampler_Control);
			#ifdef UNITY_INSTANCING_ENABLED//ASE Terrain Instancing
				TEXTURE2D(_TerrainHeightmapTexture);//ASE Terrain Instancing
				TEXTURE2D( _TerrainNormalmapTexture);//ASE Terrain Instancing
				SAMPLER(sampler_TerrainNormalmapTexture);//ASE Terrain Instancing
			#endif//ASE Terrain Instancing
			UNITY_INSTANCING_BUFFER_START( Terrain )//ASE Terrain Instancing
				UNITY_DEFINE_INSTANCED_PROP( float4, _TerrainPatchInstanceData )//ASE Terrain Instancing
			UNITY_INSTANCING_BUFFER_END( Terrain)//ASE Terrain Instancing
			CBUFFER_START( UnityTerrain)//ASE Terrain Instancing
				#ifdef UNITY_INSTANCING_ENABLED//ASE Terrain Instancing
					float4 _TerrainHeightmapRecipSize;//ASE Terrain Instancing
					float4 _TerrainHeightmapScale;//ASE Terrain Instancing
				#endif//ASE Terrain Instancing
			CBUFFER_END//ASE Terrain Instancing


			VertexInput ApplyMeshModification( VertexInput v )
			{
			#ifdef UNITY_INSTANCING_ENABLED
				float2 patchVertex = v.vertex.xy;
				float4 instanceData = UNITY_ACCESS_INSTANCED_PROP( Terrain, _TerrainPatchInstanceData );
				float2 sampleCoords = ( patchVertex.xy + instanceData.xy ) * instanceData.z;
				float height = UnpackHeightmap( _TerrainHeightmapTexture.Load( int3( sampleCoords, 0 ) ) );
				v.vertex.xz = sampleCoords* _TerrainHeightmapScale.xz;
				v.vertex.y = height* _TerrainHeightmapScale.y;
				#ifdef ENABLE_TERRAIN_PERPIXEL_NORMAL
					v.ase_normal = float3(0, 1, 0);
				#else
					v.ase_normal = _TerrainNormalmapTexture.Load(int3(sampleCoords, 0)).rgb* 2 - 1;
				#endif
				v.ase_texcoord.xy = sampleCoords* _TerrainHeightmapRecipSize.zw;
			#endif
				return v;
			}
			

			float3 _LightDirection;
			float3 _LightPosition;

			VertexOutput VertexFunction( VertexInput v )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				v = ApplyMeshModification(v);
				float3 localCalculateTangentsSRP76_g11 = ( ( v.ase_tangent.xyz * v.ase_normal * 0.0 ) );
				{
				v.ase_tangent.xyz = cross ( v.ase_normal, float3( 0, 0, 1 ) );
				v.ase_tangent.w = -1;
				}
				float3 TangetsAlpha72 = localCalculateTangentsSRP76_g11;
				
				o.ase_texcoord2.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = TangetsAlpha72;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif
				float3 normalWS = TransformObjectToWorldDir(v.ase_normal);


			#if _CASTING_PUNCTUAL_LIGHT_SHADOW
				float3 lightDirectionWS = normalize(_LightPosition - positionWS);
			#else
				float3 lightDirectionWS = _LightDirection;
			#endif

				float4 clipPos = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, lightDirectionWS));
			
			#if UNITY_REVERSED_Z
				clipPos.z = min(clipPos.z, UNITY_NEAR_CLIP_VALUE);
			#else
				clipPos.z = max(clipPos.z, UNITY_NEAR_CLIP_VALUE);
			#endif


				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = clipPos;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				o.clipPos = clipPos;
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_tangent = v.ase_tangent;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_tangent = patch[0].ase_tangent * bary.x + patch[1].ase_tangent * bary.y + patch[2].ase_tangent * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			#if defined(ASE_EARLY_Z_DEPTH_OPTIMIZE)
				#define ASE_SV_DEPTH SV_DepthLessEqual  
			#else
				#define ASE_SV_DEPTH SV_Depth
			#endif

			half4 frag(	VertexOutput IN 
						#ifdef ASE_DEPTH_WRITE_ON
						,out float outputDepth : ASE_SV_DEPTH
						#endif
						 ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );
				
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float2 uv_Control = IN.ase_texcoord2.xy * _Control_ST.xy + _Control_ST.zw;
				float4 tex2DNode5_g11 = SAMPLE_TEXTURE2D( _Control, sampler_Control, uv_Control );
				float dotResult20_g11 = dot( tex2DNode5_g11 , float4(1,1,1,1) );
				float SplatWeight22_g11 = dotResult20_g11;
				float AlphaLayer71 = SplatWeight22_g11;
				
				float Alpha = AlphaLayer71;
				float AlphaClipThreshold = 0.5;
				float AlphaClipThresholdShadow = 0.5;
				#ifdef ASE_DEPTH_WRITE_ON
				float DepthValue = 0;
				#endif

				#ifdef _ALPHATEST_ON
					#ifdef _ALPHATEST_SHADOW_ON
						clip(Alpha - AlphaClipThresholdShadow);
					#else
						clip(Alpha - AlphaClipThreshold);
					#endif
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif

				#ifdef ASE_DEPTH_WRITE_ON
					outputDepth = DepthValue;
				#endif
				return 0;
			}

			ENDHLSL
		}

		UsePass "Hidden/Nature/Terrain/Utilities/PICKING"
	UsePass "Hidden/Nature/Terrain/Utilities/SELECTION"

		Pass
		{
			
			Name "DepthOnly"
			Tags { "LightMode"="DepthOnly" }

			ZWrite On
			ColorMask 0
			AlphaToMask Off

			HLSLPROGRAM
			
			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define ASE_FINAL_COLOR_ALPHA_MULTIPLY 1
			#define _SPECULAR_SETUP 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 999999
			#define ASE_USING_SAMPLING_MACROS 1

			
			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS SHADERPASS_DEPTHONLY
        
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_VERT_POSITION
			#pragma multi_compile_instancing
			#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap forwardadd
			#pragma multi_compile_local __ _ALPHATEST_ON
			#pragma shader_feature_local _MASKMAP


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _Control_ST;
			float4 _Specular1;
			float4 _Specular0;
			float4 _TerrainHolesTexture_ST;
			float4 _Specular2;
			float4 _Splat3_ST;
			float4 _Splat2_ST;
			float4 _Specular3;
			float4 _Splat0_ST;
			float4 _Splat1_ST;
			float2 _TriOffset;
			float _Smoothness0;
			float _Smoothness1;
			float _Metallic3;
			float _Smoothness3;
			float _Metallic2;
			float _TriTiling;
			float _TriUVRotation;
			float _WorldtoObjectSwitch;
			float _CoverageAmount;
			float _CoverageFalloff;
			float _Metallic1;
			float _Metallic0;
			float _Smoothness2;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			float4 _MaskMapRemapOffset2;
			float4 _MaskMapRemapScale2;
			float4 _MaskMapRemapScale0;
			float4 _MaskMapRemapOffset3;
			float4 _MaskMapRemapOffset1;
			float4 _MaskMapRemapOffset0;
			float4 _MaskMapRemapScale3;
			float4 _MaskMapRemapScale1;
			TEXTURE2D(_Mask1);
			SAMPLER(sampler_Mask1);
			TEXTURE2D(_Mask2);
			SAMPLER(sampler_Mask2);
			TEXTURE2D(_Mask3);
			SAMPLER(sampler_Mask3);
			TEXTURE2D(_Mask0);
			SAMPLER(sampler_Mask0);
			TEXTURE2D(_Control);
			SAMPLER(sampler_Control);
			#ifdef UNITY_INSTANCING_ENABLED//ASE Terrain Instancing
				TEXTURE2D(_TerrainHeightmapTexture);//ASE Terrain Instancing
				TEXTURE2D( _TerrainNormalmapTexture);//ASE Terrain Instancing
				SAMPLER(sampler_TerrainNormalmapTexture);//ASE Terrain Instancing
			#endif//ASE Terrain Instancing
			UNITY_INSTANCING_BUFFER_START( Terrain )//ASE Terrain Instancing
				UNITY_DEFINE_INSTANCED_PROP( float4, _TerrainPatchInstanceData )//ASE Terrain Instancing
			UNITY_INSTANCING_BUFFER_END( Terrain)//ASE Terrain Instancing
			CBUFFER_START( UnityTerrain)//ASE Terrain Instancing
				#ifdef UNITY_INSTANCING_ENABLED//ASE Terrain Instancing
					float4 _TerrainHeightmapRecipSize;//ASE Terrain Instancing
					float4 _TerrainHeightmapScale;//ASE Terrain Instancing
				#endif//ASE Terrain Instancing
			CBUFFER_END//ASE Terrain Instancing


			VertexInput ApplyMeshModification( VertexInput v )
			{
			#ifdef UNITY_INSTANCING_ENABLED
				float2 patchVertex = v.vertex.xy;
				float4 instanceData = UNITY_ACCESS_INSTANCED_PROP( Terrain, _TerrainPatchInstanceData );
				float2 sampleCoords = ( patchVertex.xy + instanceData.xy ) * instanceData.z;
				float height = UnpackHeightmap( _TerrainHeightmapTexture.Load( int3( sampleCoords, 0 ) ) );
				v.vertex.xz = sampleCoords* _TerrainHeightmapScale.xz;
				v.vertex.y = height* _TerrainHeightmapScale.y;
				#ifdef ENABLE_TERRAIN_PERPIXEL_NORMAL
					v.ase_normal = float3(0, 1, 0);
				#else
					v.ase_normal = _TerrainNormalmapTexture.Load(int3(sampleCoords, 0)).rgb* 2 - 1;
				#endif
				v.ase_texcoord.xy = sampleCoords* _TerrainHeightmapRecipSize.zw;
			#endif
				return v;
			}
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				v = ApplyMeshModification(v);
				float3 localCalculateTangentsSRP76_g11 = ( ( v.ase_tangent.xyz * v.ase_normal * 0.0 ) );
				{
				v.ase_tangent.xyz = cross ( v.ase_normal, float3( 0, 0, 1 ) );
				v.ase_tangent.w = -1;
				}
				float3 TangetsAlpha72 = localCalculateTangentsSRP76_g11;
				
				o.ase_texcoord2.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = TangetsAlpha72;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;
				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float4 positionCS = TransformWorldToHClip( positionWS );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = positionCS;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				o.clipPos = positionCS;
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_tangent = v.ase_tangent;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_tangent = patch[0].ase_tangent * bary.x + patch[1].ase_tangent * bary.y + patch[2].ase_tangent * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			#if defined(ASE_EARLY_Z_DEPTH_OPTIMIZE)
				#define ASE_SV_DEPTH SV_DepthLessEqual  
			#else
				#define ASE_SV_DEPTH SV_Depth
			#endif
			half4 frag(	VertexOutput IN 
						#ifdef ASE_DEPTH_WRITE_ON
						,out float outputDepth : ASE_SV_DEPTH
						#endif
						 ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float2 uv_Control = IN.ase_texcoord2.xy * _Control_ST.xy + _Control_ST.zw;
				float4 tex2DNode5_g11 = SAMPLE_TEXTURE2D( _Control, sampler_Control, uv_Control );
				float dotResult20_g11 = dot( tex2DNode5_g11 , float4(1,1,1,1) );
				float SplatWeight22_g11 = dotResult20_g11;
				float AlphaLayer71 = SplatWeight22_g11;
				
				float Alpha = AlphaLayer71;
				float AlphaClipThreshold = 0.5;
				#ifdef ASE_DEPTH_WRITE_ON
				float DepthValue = 0;
				#endif

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif
				#ifdef ASE_DEPTH_WRITE_ON
				outputDepth = DepthValue;
				#endif

				return 0;
			}
			ENDHLSL
		}
		
		UsePass "Hidden/Nature/Terrain/Utilities/PICKING"
	UsePass "Hidden/Nature/Terrain/Utilities/SELECTION"

		Pass
		{
			
			Name "Meta"
			Tags { "LightMode"="Meta" }

			Cull Off

			HLSLPROGRAM
			
			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define ASE_FINAL_COLOR_ALPHA_MULTIPLY 1
			#define _SPECULAR_SETUP 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 999999
			#define ASE_USING_SAMPLING_MACROS 1

			
			#pragma vertex vert
			#pragma fragment frag

			#pragma shader_feature _ EDITOR_VISUALIZATION

			#define SHADERPASS SHADERPASS_META

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_FRAG_POSITION
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_VERT_POSITION
			#pragma multi_compile_instancing
			#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap forwardadd
			#pragma multi_compile_local __ _ALPHATEST_ON
			#pragma shader_feature_local _MASKMAP


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 texcoord0 : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				float4 ase_tangent : TANGENT;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				#ifdef EDITOR_VISUALIZATION
				float4 VizUV : TEXCOORD2;
				float4 LightCoord : TEXCOORD3;
				#endif
				float4 ase_texcoord4 : TEXCOORD4;
				float4 ase_texcoord5 : TEXCOORD5;
				float4 ase_texcoord6 : TEXCOORD6;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _Control_ST;
			float4 _Specular1;
			float4 _Specular0;
			float4 _TerrainHolesTexture_ST;
			float4 _Specular2;
			float4 _Splat3_ST;
			float4 _Splat2_ST;
			float4 _Specular3;
			float4 _Splat0_ST;
			float4 _Splat1_ST;
			float2 _TriOffset;
			float _Smoothness0;
			float _Smoothness1;
			float _Metallic3;
			float _Smoothness3;
			float _Metallic2;
			float _TriTiling;
			float _TriUVRotation;
			float _WorldtoObjectSwitch;
			float _CoverageAmount;
			float _CoverageFalloff;
			float _Metallic1;
			float _Metallic0;
			float _Smoothness2;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			float4 _MaskMapRemapOffset2;
			float4 _MaskMapRemapScale2;
			float4 _MaskMapRemapScale0;
			float4 _MaskMapRemapOffset3;
			float4 _MaskMapRemapOffset1;
			float4 _MaskMapRemapOffset0;
			float4 _MaskMapRemapScale3;
			float4 _MaskMapRemapScale1;
			TEXTURE2D(_Mask1);
			SAMPLER(sampler_Mask1);
			TEXTURE2D(_Mask2);
			SAMPLER(sampler_Mask2);
			TEXTURE2D(_Mask3);
			SAMPLER(sampler_Mask3);
			TEXTURE2D(_Mask0);
			SAMPLER(sampler_Mask0);
			TEXTURE2D(_Control);
			SAMPLER(sampler_Control);
			float _LayerHasMask0;
			float _LayerHasMask1;
			float _LayerHasMask2;
			float _LayerHasMask3;
			TEXTURE2D(_Splat0);
			TEXTURE2D(_Splat1);
			TEXTURE2D(_Splat2);
			TEXTURE2D(_Splat3);
			SAMPLER(sampler_linear_repeat_aniso4);
			float4 _DiffuseRemapScale0;
			float4 _DiffuseRemapScale1;
			float4 _DiffuseRemapScale2;
			float4 _DiffuseRemapScale3;
			TEXTURE2D(_TerrainHolesTexture);
			SAMPLER(sampler_TerrainHolesTexture);
			TEXTURE2D(_TempTriplanarAlbedo);
			TEXTURE2D(_TempTopAlbedo);
			SAMPLER(sampler_TempTopAlbedo);
			#ifdef UNITY_INSTANCING_ENABLED//ASE Terrain Instancing
				TEXTURE2D(_TerrainHeightmapTexture);//ASE Terrain Instancing
				TEXTURE2D( _TerrainNormalmapTexture);//ASE Terrain Instancing
				SAMPLER(sampler_TerrainNormalmapTexture);//ASE Terrain Instancing
			#endif//ASE Terrain Instancing
			UNITY_INSTANCING_BUFFER_START( Terrain )//ASE Terrain Instancing
				UNITY_DEFINE_INSTANCED_PROP( float4, _TerrainPatchInstanceData )//ASE Terrain Instancing
			UNITY_INSTANCING_BUFFER_END( Terrain)//ASE Terrain Instancing
			CBUFFER_START( UnityTerrain)//ASE Terrain Instancing
				#ifdef UNITY_INSTANCING_ENABLED//ASE Terrain Instancing
					float4 _TerrainHeightmapRecipSize;//ASE Terrain Instancing
					float4 _TerrainHeightmapScale;//ASE Terrain Instancing
				#endif//ASE Terrain Instancing
			CBUFFER_END//ASE Terrain Instancing


			VertexInput ApplyMeshModification( VertexInput v )
			{
			#ifdef UNITY_INSTANCING_ENABLED
				float2 patchVertex = v.vertex.xy;
				float4 instanceData = UNITY_ACCESS_INSTANCED_PROP( Terrain, _TerrainPatchInstanceData );
				float2 sampleCoords = ( patchVertex.xy + instanceData.xy ) * instanceData.z;
				float height = UnpackHeightmap( _TerrainHeightmapTexture.Load( int3( sampleCoords, 0 ) ) );
				v.vertex.xz = sampleCoords* _TerrainHeightmapScale.xz;
				v.vertex.y = height* _TerrainHeightmapScale.y;
				#ifdef ENABLE_TERRAIN_PERPIXEL_NORMAL
					v.ase_normal = float3(0, 1, 0);
				#else
					v.ase_normal = _TerrainNormalmapTexture.Load(int3(sampleCoords, 0)).rgb* 2 - 1;
				#endif
				v.texcoord0.xy = sampleCoords* _TerrainHeightmapRecipSize.zw;
			#endif
				return v;
			}
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				v = ApplyMeshModification(v);
				float3 localCalculateTangentsSRP76_g11 = ( ( v.ase_tangent.xyz * v.ase_normal * 0.0 ) );
				{
				v.ase_tangent.xyz = cross ( v.ase_normal, float3( 0, 0, 1 ) );
				v.ase_tangent.w = -1;
				}
				float3 TangetsAlpha72 = localCalculateTangentsSRP76_g11;
				
				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord6.xyz = ase_worldNormal;
				
				o.ase_texcoord4.xy = v.texcoord0.xy;
				o.ase_texcoord5 = v.vertex;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord4.zw = 0;
				o.ase_texcoord6.w = 0;
				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = TangetsAlpha72;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif

				o.clipPos = MetaVertexPosition( v.vertex, v.texcoord1.xy, v.texcoord1.xy, unity_LightmapST, unity_DynamicLightmapST );

			#ifdef EDITOR_VISUALIZATION
				float2 VizUV = 0;
				float4 LightCoord = 0;
				UnityEditorVizData(v.vertex.xyz, v.texcoord0.xy, v.texcoord1.xy, v.texcoord2.xy, VizUV, LightCoord);
				o.VizUV = float4(VizUV, 0, 0);
				o.LightCoord = LightCoord;
			#endif

			#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				VertexPositionInputs vertexInput = (VertexPositionInputs)0;
				vertexInput.positionWS = positionWS;
				vertexInput.positionCS = o.clipPos;
				o.shadowCoord = GetShadowCoord( vertexInput );
			#endif
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 texcoord0 : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				float4 ase_tangent : TANGENT;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.texcoord0 = v.texcoord0;
				o.texcoord1 = v.texcoord1;
				o.texcoord2 = v.texcoord2;
				o.ase_tangent = v.ase_tangent;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.texcoord0 = patch[0].texcoord0 * bary.x + patch[1].texcoord0 * bary.y + patch[2].texcoord0 * bary.z;
				o.texcoord1 = patch[0].texcoord1 * bary.x + patch[1].texcoord1 * bary.y + patch[2].texcoord1 * bary.z;
				o.texcoord2 = patch[0].texcoord2 * bary.x + patch[1].texcoord2 * bary.y + patch[2].texcoord2 * bary.z;
				o.ase_tangent = patch[0].ase_tangent * bary.x + patch[1].ase_tangent * bary.y + patch[2].ase_tangent * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float2 uv_Control = IN.ase_texcoord4.xy * _Control_ST.xy + _Control_ST.zw;
				float4 tex2DNode5_g11 = SAMPLE_TEXTURE2D( _Control, sampler_Control, uv_Control );
				float dotResult20_g11 = dot( tex2DNode5_g11 , float4(1,1,1,1) );
				float SplatWeight22_g11 = dotResult20_g11;
				float localSplatClip74_g11 = ( SplatWeight22_g11 );
				float SplatWeight74_g11 = SplatWeight22_g11;
				{
				#if !defined(SHADER_API_MOBILE) && defined(TERRAIN_SPLAT_ADDPASS)
				clip(SplatWeight74_g11 == 0.0f ? -1 : 1);
				#endif
				}
				float4 SplatControl26_g11 = ( tex2DNode5_g11 / ( localSplatClip74_g11 + 0.001 ) );
				float4 appendResult55_g11 = (float4(_Metallic0 , _Metallic1 , _Metallic2 , _Metallic3));
				float localComputeMasks130_g11 = ( 0.0 );
				float4 masks0130_g11 = float4( 0,0,0,0 );
				float4 masks1130_g11 = float4( 0,0,0,0 );
				float4 masks2130_g11 = float4( 0,0,0,0 );
				float4 masks3130_g11 = float4( 0,0,0,0 );
				float4 appendResult135_g11 = (float4(_LayerHasMask0 , _LayerHasMask1 , _LayerHasMask2 , _LayerHasMask3));
				float4 layerHasMask136_g11 = appendResult135_g11;
				float4 hasMask130_g11 = layerHasMask136_g11;
				float2 uv_Splat0 = IN.ase_texcoord4.xy * _Splat0_ST.xy + _Splat0_ST.zw;
				float2 uvMask0130_g11 = uv_Splat0;
				float2 uv_Splat1 = IN.ase_texcoord4.xy * _Splat1_ST.xy + _Splat1_ST.zw;
				float2 uvMask1130_g11 = uv_Splat1;
				float2 uv_Splat2 = IN.ase_texcoord4.xy * _Splat2_ST.xy + _Splat2_ST.zw;
				float2 uvMask2130_g11 = uv_Splat2;
				float2 uv_Splat3 = IN.ase_texcoord4.xy * _Splat3_ST.xy + _Splat3_ST.zw;
				float2 uvMask3130_g11 = uv_Splat3;
				SamplerState SamplerS130_g11 = sampler_linear_repeat_aniso4;
				{
				masks0130_g11 = 0.5h;
				masks1130_g11 = 0.5h;
				masks2130_g11 = 0.5h;
				masks3130_g11 = 0.5h;
				#ifdef _MASKMAP
				masks0130_g11 = lerp(masks0130_g11, SAMPLE_TEXTURE2D(_Mask0, SamplerS130_g11, uvMask0130_g11), hasMask130_g11.x);
				masks1130_g11 = lerp(masks1130_g11, SAMPLE_TEXTURE2D(_Mask1, SamplerS130_g11, uvMask1130_g11), hasMask130_g11.y);
				masks2130_g11 = lerp(masks2130_g11, SAMPLE_TEXTURE2D(_Mask2, SamplerS130_g11, uvMask2130_g11), hasMask130_g11.z);
				masks3130_g11 = lerp(masks3130_g11, SAMPLE_TEXTURE2D(_Mask3, SamplerS130_g11, uvMask3130_g11), hasMask130_g11.w);
				#endif
				masks0130_g11 *= _MaskMapRemapScale0.rgba;
				masks0130_g11 += _MaskMapRemapOffset0.rgba;
				masks1130_g11 *= _MaskMapRemapScale1.rgba;
				masks1130_g11 += _MaskMapRemapOffset1.rgba;
				masks2130_g11 *= _MaskMapRemapScale2.rgba;
				masks2130_g11 += _MaskMapRemapOffset2.rgba;
				masks3130_g11 *= _MaskMapRemapScale3.rgba;
				masks3130_g11 += _MaskMapRemapOffset3.rgba;
				}
				float4 mask0138_g11 = masks0130_g11;
				float4 mask1139_g11 = masks1130_g11;
				float4 mask2140_g11 = masks2130_g11;
				float4 mask3141_g11 = masks3130_g11;
				float4 appendResult168_g11 = (float4((mask0138_g11).x , (mask1139_g11).x , (mask2140_g11).x , (mask3141_g11).x));
				float4 maskMetallic169_g11 = appendResult168_g11;
				float4 lerpResult202_g11 = lerp( appendResult55_g11 , maskMetallic169_g11 , layerHasMask136_g11);
				float dotResult53_g11 = dot( SplatControl26_g11 , lerpResult202_g11 );
				float temp_output_527_56 = dotResult53_g11;
				float4 appendResult33_g11 = (float4(1.0 , 1.0 , 1.0 , _Smoothness0));
				float4 tex2DNode4_g11 = SAMPLE_TEXTURE2D( _Splat0, sampler_linear_repeat_aniso4, uv_Splat0 );
				float4 appendResult258_g11 = (float4(( (SplatControl26_g11).rrr * (_DiffuseRemapScale0).rgb ) , 1.0));
				float4 tintLayer0253_g11 = appendResult258_g11;
				float4 appendResult36_g11 = (float4(1.0 , 1.0 , 1.0 , _Smoothness1));
				float4 tex2DNode3_g11 = SAMPLE_TEXTURE2D( _Splat1, sampler_linear_repeat_aniso4, uv_Splat1 );
				float4 appendResult261_g11 = (float4(( (SplatControl26_g11).ggg * (_DiffuseRemapScale1).rgb ) , 1.0));
				float4 tintLayer1254_g11 = appendResult261_g11;
				float4 appendResult39_g11 = (float4(1.0 , 1.0 , 1.0 , _Smoothness2));
				float4 tex2DNode6_g11 = SAMPLE_TEXTURE2D( _Splat2, sampler_linear_repeat_aniso4, uv_Splat2 );
				float4 appendResult263_g11 = (float4(( (SplatControl26_g11).bbb * (_DiffuseRemapScale2).rgb ) , 1.0));
				float4 tintLayer2255_g11 = appendResult263_g11;
				float4 appendResult42_g11 = (float4(1.0 , 1.0 , 1.0 , _Smoothness3));
				float4 tex2DNode7_g11 = SAMPLE_TEXTURE2D( _Splat3, sampler_linear_repeat_aniso4, uv_Splat3 );
				float4 appendResult265_g11 = (float4(( (SplatControl26_g11).aaa * (_DiffuseRemapScale3).rgb ) , 1.0));
				float4 tintLayer3256_g11 = appendResult265_g11;
				float4 weightedBlendVar9_g11 = float4(1,1,1,1);
				float4 weightedBlend9_g11 = ( weightedBlendVar9_g11.x*( appendResult33_g11 * tex2DNode4_g11 * tintLayer0253_g11 ) + weightedBlendVar9_g11.y*( appendResult36_g11 * tex2DNode3_g11 * tintLayer1254_g11 ) + weightedBlendVar9_g11.z*( appendResult39_g11 * tex2DNode6_g11 * tintLayer2255_g11 ) + weightedBlendVar9_g11.w*( appendResult42_g11 * tex2DNode7_g11 * tintLayer3256_g11 ) );
				float4 MixDiffuse28_g11 = weightedBlend9_g11;
				float4 temp_output_60_0_g11 = MixDiffuse28_g11;
				float4 localClipHoles100_g11 = ( temp_output_60_0_g11 );
				float2 uv_TerrainHolesTexture = IN.ase_texcoord4.xy * _TerrainHolesTexture_ST.xy + _TerrainHolesTexture_ST.zw;
				float holeClipValue99_g11 = SAMPLE_TEXTURE2D( _TerrainHolesTexture, sampler_TerrainHolesTexture, uv_TerrainHolesTexture ).r;
				float Hole100_g11 = holeClipValue99_g11;
				{
				#ifdef _ALPHATEST_ON
				clip(Hole100_g11 == 0.0f ? -1 : 1);
				#endif
				}
				float4 AlbedoLayer67 = ( temp_output_527_56 * localClipHoles100_g11 );
				float mTriTiling455 = _TriTiling;
				float2 appendResult352 = (float2(IN.ase_texcoord5.xyz.y , IN.ase_texcoord5.xyz.z));
				float mTriUVRotation493 = _TriUVRotation;
				float cos491 = cos( mTriUVRotation493 );
				float sin491 = sin( mTriUVRotation493 );
				float2 rotator491 = mul( ( mTriTiling455 * appendResult352 ) - float2( 0,0 ) , float2x2( cos491 , -sin491 , sin491 , cos491 )) + float2( 0,0 );
				float2 mTriOffset503 = _TriOffset;
				float2 TriplanarUV_1415 = ( 1.0 - ( rotator491 + mTriOffset503 ) );
				float3 ase_worldNormal = IN.ase_texcoord6.xyz;
				float3 temp_output_275_0 = abs( mul( GetWorldToObjectMatrix(), float4( ase_worldNormal , 0.0 ) ).xyz );
				float dotResult276 = dot( temp_output_275_0 , float3(1,1,1) );
				float3 BlendComponents278 = ( temp_output_275_0 / dotResult276 );
				float TriCom_1480 = BlendComponents278.x;
				float2 appendResult354 = (float2(IN.ase_texcoord5.xyz.x , IN.ase_texcoord5.xyz.z));
				float cos495 = cos( mTriUVRotation493 );
				float sin495 = sin( mTriUVRotation493 );
				float2 rotator495 = mul( ( mTriTiling455 * appendResult354 ) - float2( 0,0 ) , float2x2( cos495 , -sin495 , sin495 , cos495 )) + float2( 0,0 );
				float2 TriplanarUV_2416 = ( 1.0 - ( rotator495 + mTriOffset503 ) );
				float TriCom_2479 = BlendComponents278.y;
				float2 appendResult351 = (float2(IN.ase_texcoord5.xyz.x , IN.ase_texcoord5.xyz.y));
				float cos496 = cos( mTriUVRotation493 );
				float sin496 = sin( mTriUVRotation493 );
				float2 rotator496 = mul( ( mTriTiling455 * appendResult351 ) - float2( 0,0 ) , float2x2( cos496 , -sin496 , sin496 , cos496 )) + float2( 0,0 );
				float2 TriplanarUV_3417 = ( 1.0 - ( rotator496 + mTriOffset503 ) );
				float TriCom_3477 = BlendComponents278.z;
				float WorldObjectSwitch321 = _WorldtoObjectSwitch;
				float3 lerpResult353 = lerp( WorldPosition , IN.ase_texcoord5.xyz , WorldObjectSwitch321);
				float3 break361 = lerpResult353;
				float2 appendResult365 = (float2(break361.x , break361.z));
				float2 TriplanarTopUV418 = ( ( mTriTiling455 * appendResult365 ) + mTriOffset503 );
				float3 lerpResult342 = lerp( ase_worldNormal , mul( GetWorldToObjectMatrix(), float4( ase_worldNormal , 0.0 ) ).xyz , WorldObjectSwitch321);
				float3 temp_cast_11 = (_CoverageFalloff).xxx;
				float TriplanarColorWeight419 = pow( saturate( ( lerpResult342 + _CoverageAmount ) ) , temp_cast_11 ).y;
				float4 lerpResult461 = lerp( ( ( ( SAMPLE_TEXTURE2D( _TempTriplanarAlbedo, sampler_linear_repeat_aniso4, TriplanarUV_1415 ) * TriCom_1480 ) + ( SAMPLE_TEXTURE2D( _TempTriplanarAlbedo, sampler_linear_repeat_aniso4, TriplanarUV_2416 ) * TriCom_2479 ) ) + ( SAMPLE_TEXTURE2D( _TempTriplanarAlbedo, sampler_linear_repeat_aniso4, TriplanarUV_3417 ) * TriCom_3477 ) ) , SAMPLE_TEXTURE2D( _TempTopAlbedo, sampler_TempTopAlbedo, TriplanarTopUV418 ) , TriplanarColorWeight419);
				float4 TempTriAlbedo463 = lerpResult461;
				float4 temp_output_59_0_g11 = SplatControl26_g11;
				float FirstLayerControl66 = temp_output_59_0_g11.x;
				float4 lerpResult81 = lerp( AlbedoLayer67 , TempTriAlbedo463 , FirstLayerControl66);
				
				float AlphaLayer71 = SplatWeight22_g11;
				
				
				float3 Albedo = lerpResult81.rgb;
				float3 Emission = 0;
				float Alpha = AlphaLayer71;
				float AlphaClipThreshold = 0.5;

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				MetaInput metaInput = (MetaInput)0;
				metaInput.Albedo = Albedo;
				metaInput.Emission = Emission;
			#ifdef EDITOR_VISUALIZATION
				metaInput.VizUV = IN.VizUV.xy;
				metaInput.LightCoord = IN.LightCoord;
			#endif
				
				return MetaFragment(metaInput);
			}
			ENDHLSL
		}

		UsePass "Hidden/Nature/Terrain/Utilities/PICKING"
	UsePass "Hidden/Nature/Terrain/Utilities/SELECTION"

		Pass
		{
			
			Name "Universal2D"
			Tags { "LightMode"="Universal2D" }

			Blend One Zero, One Zero
			ColorMask RGBA

			HLSLPROGRAM
			
			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define ASE_FINAL_COLOR_ALPHA_MULTIPLY 1
			#define _SPECULAR_SETUP 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 999999
			#define ASE_USING_SAMPLING_MACROS 1

			
			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS SHADERPASS_2D
        
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
			
			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_FRAG_POSITION
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_VERT_POSITION
			#pragma multi_compile_instancing
			#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap forwardadd
			#pragma multi_compile_local __ _ALPHATEST_ON
			#pragma shader_feature_local _MASKMAP


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _Control_ST;
			float4 _Specular1;
			float4 _Specular0;
			float4 _TerrainHolesTexture_ST;
			float4 _Specular2;
			float4 _Splat3_ST;
			float4 _Splat2_ST;
			float4 _Specular3;
			float4 _Splat0_ST;
			float4 _Splat1_ST;
			float2 _TriOffset;
			float _Smoothness0;
			float _Smoothness1;
			float _Metallic3;
			float _Smoothness3;
			float _Metallic2;
			float _TriTiling;
			float _TriUVRotation;
			float _WorldtoObjectSwitch;
			float _CoverageAmount;
			float _CoverageFalloff;
			float _Metallic1;
			float _Metallic0;
			float _Smoothness2;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			float4 _MaskMapRemapOffset2;
			float4 _MaskMapRemapScale2;
			float4 _MaskMapRemapScale0;
			float4 _MaskMapRemapOffset3;
			float4 _MaskMapRemapOffset1;
			float4 _MaskMapRemapOffset0;
			float4 _MaskMapRemapScale3;
			float4 _MaskMapRemapScale1;
			TEXTURE2D(_Mask1);
			SAMPLER(sampler_Mask1);
			TEXTURE2D(_Mask2);
			SAMPLER(sampler_Mask2);
			TEXTURE2D(_Mask3);
			SAMPLER(sampler_Mask3);
			TEXTURE2D(_Mask0);
			SAMPLER(sampler_Mask0);
			TEXTURE2D(_Control);
			SAMPLER(sampler_Control);
			float _LayerHasMask0;
			float _LayerHasMask1;
			float _LayerHasMask2;
			float _LayerHasMask3;
			TEXTURE2D(_Splat0);
			TEXTURE2D(_Splat1);
			TEXTURE2D(_Splat2);
			TEXTURE2D(_Splat3);
			SAMPLER(sampler_linear_repeat_aniso4);
			float4 _DiffuseRemapScale0;
			float4 _DiffuseRemapScale1;
			float4 _DiffuseRemapScale2;
			float4 _DiffuseRemapScale3;
			TEXTURE2D(_TerrainHolesTexture);
			SAMPLER(sampler_TerrainHolesTexture);
			TEXTURE2D(_TempTriplanarAlbedo);
			TEXTURE2D(_TempTopAlbedo);
			SAMPLER(sampler_TempTopAlbedo);
			#ifdef UNITY_INSTANCING_ENABLED//ASE Terrain Instancing
				TEXTURE2D(_TerrainHeightmapTexture);//ASE Terrain Instancing
				TEXTURE2D( _TerrainNormalmapTexture);//ASE Terrain Instancing
				SAMPLER(sampler_TerrainNormalmapTexture);//ASE Terrain Instancing
			#endif//ASE Terrain Instancing
			UNITY_INSTANCING_BUFFER_START( Terrain )//ASE Terrain Instancing
				UNITY_DEFINE_INSTANCED_PROP( float4, _TerrainPatchInstanceData )//ASE Terrain Instancing
			UNITY_INSTANCING_BUFFER_END( Terrain)//ASE Terrain Instancing
			CBUFFER_START( UnityTerrain)//ASE Terrain Instancing
				#ifdef UNITY_INSTANCING_ENABLED//ASE Terrain Instancing
					float4 _TerrainHeightmapRecipSize;//ASE Terrain Instancing
					float4 _TerrainHeightmapScale;//ASE Terrain Instancing
				#endif//ASE Terrain Instancing
			CBUFFER_END//ASE Terrain Instancing


			VertexInput ApplyMeshModification( VertexInput v )
			{
			#ifdef UNITY_INSTANCING_ENABLED
				float2 patchVertex = v.vertex.xy;
				float4 instanceData = UNITY_ACCESS_INSTANCED_PROP( Terrain, _TerrainPatchInstanceData );
				float2 sampleCoords = ( patchVertex.xy + instanceData.xy ) * instanceData.z;
				float height = UnpackHeightmap( _TerrainHeightmapTexture.Load( int3( sampleCoords, 0 ) ) );
				v.vertex.xz = sampleCoords* _TerrainHeightmapScale.xz;
				v.vertex.y = height* _TerrainHeightmapScale.y;
				#ifdef ENABLE_TERRAIN_PERPIXEL_NORMAL
					v.ase_normal = float3(0, 1, 0);
				#else
					v.ase_normal = _TerrainNormalmapTexture.Load(int3(sampleCoords, 0)).rgb* 2 - 1;
				#endif
				v.ase_texcoord.xy = sampleCoords* _TerrainHeightmapRecipSize.zw;
			#endif
				return v;
			}
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				v = ApplyMeshModification(v);
				float3 localCalculateTangentsSRP76_g11 = ( ( v.ase_tangent.xyz * v.ase_normal * 0.0 ) );
				{
				v.ase_tangent.xyz = cross ( v.ase_normal, float3( 0, 0, 1 ) );
				v.ase_tangent.w = -1;
				}
				float3 TangetsAlpha72 = localCalculateTangentsSRP76_g11;
				
				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord4.xyz = ase_worldNormal;
				
				o.ase_texcoord2.xy = v.ase_texcoord.xy;
				o.ase_texcoord3 = v.vertex;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.zw = 0;
				o.ase_texcoord4.w = 0;
				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = TangetsAlpha72;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float4 positionCS = TransformWorldToHClip( positionWS );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = positionCS;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif

				o.clipPos = positionCS;
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_tangent = v.ase_tangent;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_tangent = patch[0].ase_tangent * bary.x + patch[1].ase_tangent * bary.y + patch[2].ase_tangent * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float2 uv_Control = IN.ase_texcoord2.xy * _Control_ST.xy + _Control_ST.zw;
				float4 tex2DNode5_g11 = SAMPLE_TEXTURE2D( _Control, sampler_Control, uv_Control );
				float dotResult20_g11 = dot( tex2DNode5_g11 , float4(1,1,1,1) );
				float SplatWeight22_g11 = dotResult20_g11;
				float localSplatClip74_g11 = ( SplatWeight22_g11 );
				float SplatWeight74_g11 = SplatWeight22_g11;
				{
				#if !defined(SHADER_API_MOBILE) && defined(TERRAIN_SPLAT_ADDPASS)
				clip(SplatWeight74_g11 == 0.0f ? -1 : 1);
				#endif
				}
				float4 SplatControl26_g11 = ( tex2DNode5_g11 / ( localSplatClip74_g11 + 0.001 ) );
				float4 appendResult55_g11 = (float4(_Metallic0 , _Metallic1 , _Metallic2 , _Metallic3));
				float localComputeMasks130_g11 = ( 0.0 );
				float4 masks0130_g11 = float4( 0,0,0,0 );
				float4 masks1130_g11 = float4( 0,0,0,0 );
				float4 masks2130_g11 = float4( 0,0,0,0 );
				float4 masks3130_g11 = float4( 0,0,0,0 );
				float4 appendResult135_g11 = (float4(_LayerHasMask0 , _LayerHasMask1 , _LayerHasMask2 , _LayerHasMask3));
				float4 layerHasMask136_g11 = appendResult135_g11;
				float4 hasMask130_g11 = layerHasMask136_g11;
				float2 uv_Splat0 = IN.ase_texcoord2.xy * _Splat0_ST.xy + _Splat0_ST.zw;
				float2 uvMask0130_g11 = uv_Splat0;
				float2 uv_Splat1 = IN.ase_texcoord2.xy * _Splat1_ST.xy + _Splat1_ST.zw;
				float2 uvMask1130_g11 = uv_Splat1;
				float2 uv_Splat2 = IN.ase_texcoord2.xy * _Splat2_ST.xy + _Splat2_ST.zw;
				float2 uvMask2130_g11 = uv_Splat2;
				float2 uv_Splat3 = IN.ase_texcoord2.xy * _Splat3_ST.xy + _Splat3_ST.zw;
				float2 uvMask3130_g11 = uv_Splat3;
				SamplerState SamplerS130_g11 = sampler_linear_repeat_aniso4;
				{
				masks0130_g11 = 0.5h;
				masks1130_g11 = 0.5h;
				masks2130_g11 = 0.5h;
				masks3130_g11 = 0.5h;
				#ifdef _MASKMAP
				masks0130_g11 = lerp(masks0130_g11, SAMPLE_TEXTURE2D(_Mask0, SamplerS130_g11, uvMask0130_g11), hasMask130_g11.x);
				masks1130_g11 = lerp(masks1130_g11, SAMPLE_TEXTURE2D(_Mask1, SamplerS130_g11, uvMask1130_g11), hasMask130_g11.y);
				masks2130_g11 = lerp(masks2130_g11, SAMPLE_TEXTURE2D(_Mask2, SamplerS130_g11, uvMask2130_g11), hasMask130_g11.z);
				masks3130_g11 = lerp(masks3130_g11, SAMPLE_TEXTURE2D(_Mask3, SamplerS130_g11, uvMask3130_g11), hasMask130_g11.w);
				#endif
				masks0130_g11 *= _MaskMapRemapScale0.rgba;
				masks0130_g11 += _MaskMapRemapOffset0.rgba;
				masks1130_g11 *= _MaskMapRemapScale1.rgba;
				masks1130_g11 += _MaskMapRemapOffset1.rgba;
				masks2130_g11 *= _MaskMapRemapScale2.rgba;
				masks2130_g11 += _MaskMapRemapOffset2.rgba;
				masks3130_g11 *= _MaskMapRemapScale3.rgba;
				masks3130_g11 += _MaskMapRemapOffset3.rgba;
				}
				float4 mask0138_g11 = masks0130_g11;
				float4 mask1139_g11 = masks1130_g11;
				float4 mask2140_g11 = masks2130_g11;
				float4 mask3141_g11 = masks3130_g11;
				float4 appendResult168_g11 = (float4((mask0138_g11).x , (mask1139_g11).x , (mask2140_g11).x , (mask3141_g11).x));
				float4 maskMetallic169_g11 = appendResult168_g11;
				float4 lerpResult202_g11 = lerp( appendResult55_g11 , maskMetallic169_g11 , layerHasMask136_g11);
				float dotResult53_g11 = dot( SplatControl26_g11 , lerpResult202_g11 );
				float temp_output_527_56 = dotResult53_g11;
				float4 appendResult33_g11 = (float4(1.0 , 1.0 , 1.0 , _Smoothness0));
				float4 tex2DNode4_g11 = SAMPLE_TEXTURE2D( _Splat0, sampler_linear_repeat_aniso4, uv_Splat0 );
				float4 appendResult258_g11 = (float4(( (SplatControl26_g11).rrr * (_DiffuseRemapScale0).rgb ) , 1.0));
				float4 tintLayer0253_g11 = appendResult258_g11;
				float4 appendResult36_g11 = (float4(1.0 , 1.0 , 1.0 , _Smoothness1));
				float4 tex2DNode3_g11 = SAMPLE_TEXTURE2D( _Splat1, sampler_linear_repeat_aniso4, uv_Splat1 );
				float4 appendResult261_g11 = (float4(( (SplatControl26_g11).ggg * (_DiffuseRemapScale1).rgb ) , 1.0));
				float4 tintLayer1254_g11 = appendResult261_g11;
				float4 appendResult39_g11 = (float4(1.0 , 1.0 , 1.0 , _Smoothness2));
				float4 tex2DNode6_g11 = SAMPLE_TEXTURE2D( _Splat2, sampler_linear_repeat_aniso4, uv_Splat2 );
				float4 appendResult263_g11 = (float4(( (SplatControl26_g11).bbb * (_DiffuseRemapScale2).rgb ) , 1.0));
				float4 tintLayer2255_g11 = appendResult263_g11;
				float4 appendResult42_g11 = (float4(1.0 , 1.0 , 1.0 , _Smoothness3));
				float4 tex2DNode7_g11 = SAMPLE_TEXTURE2D( _Splat3, sampler_linear_repeat_aniso4, uv_Splat3 );
				float4 appendResult265_g11 = (float4(( (SplatControl26_g11).aaa * (_DiffuseRemapScale3).rgb ) , 1.0));
				float4 tintLayer3256_g11 = appendResult265_g11;
				float4 weightedBlendVar9_g11 = float4(1,1,1,1);
				float4 weightedBlend9_g11 = ( weightedBlendVar9_g11.x*( appendResult33_g11 * tex2DNode4_g11 * tintLayer0253_g11 ) + weightedBlendVar9_g11.y*( appendResult36_g11 * tex2DNode3_g11 * tintLayer1254_g11 ) + weightedBlendVar9_g11.z*( appendResult39_g11 * tex2DNode6_g11 * tintLayer2255_g11 ) + weightedBlendVar9_g11.w*( appendResult42_g11 * tex2DNode7_g11 * tintLayer3256_g11 ) );
				float4 MixDiffuse28_g11 = weightedBlend9_g11;
				float4 temp_output_60_0_g11 = MixDiffuse28_g11;
				float4 localClipHoles100_g11 = ( temp_output_60_0_g11 );
				float2 uv_TerrainHolesTexture = IN.ase_texcoord2.xy * _TerrainHolesTexture_ST.xy + _TerrainHolesTexture_ST.zw;
				float holeClipValue99_g11 = SAMPLE_TEXTURE2D( _TerrainHolesTexture, sampler_TerrainHolesTexture, uv_TerrainHolesTexture ).r;
				float Hole100_g11 = holeClipValue99_g11;
				{
				#ifdef _ALPHATEST_ON
				clip(Hole100_g11 == 0.0f ? -1 : 1);
				#endif
				}
				float4 AlbedoLayer67 = ( temp_output_527_56 * localClipHoles100_g11 );
				float mTriTiling455 = _TriTiling;
				float2 appendResult352 = (float2(IN.ase_texcoord3.xyz.y , IN.ase_texcoord3.xyz.z));
				float mTriUVRotation493 = _TriUVRotation;
				float cos491 = cos( mTriUVRotation493 );
				float sin491 = sin( mTriUVRotation493 );
				float2 rotator491 = mul( ( mTriTiling455 * appendResult352 ) - float2( 0,0 ) , float2x2( cos491 , -sin491 , sin491 , cos491 )) + float2( 0,0 );
				float2 mTriOffset503 = _TriOffset;
				float2 TriplanarUV_1415 = ( 1.0 - ( rotator491 + mTriOffset503 ) );
				float3 ase_worldNormal = IN.ase_texcoord4.xyz;
				float3 temp_output_275_0 = abs( mul( GetWorldToObjectMatrix(), float4( ase_worldNormal , 0.0 ) ).xyz );
				float dotResult276 = dot( temp_output_275_0 , float3(1,1,1) );
				float3 BlendComponents278 = ( temp_output_275_0 / dotResult276 );
				float TriCom_1480 = BlendComponents278.x;
				float2 appendResult354 = (float2(IN.ase_texcoord3.xyz.x , IN.ase_texcoord3.xyz.z));
				float cos495 = cos( mTriUVRotation493 );
				float sin495 = sin( mTriUVRotation493 );
				float2 rotator495 = mul( ( mTriTiling455 * appendResult354 ) - float2( 0,0 ) , float2x2( cos495 , -sin495 , sin495 , cos495 )) + float2( 0,0 );
				float2 TriplanarUV_2416 = ( 1.0 - ( rotator495 + mTriOffset503 ) );
				float TriCom_2479 = BlendComponents278.y;
				float2 appendResult351 = (float2(IN.ase_texcoord3.xyz.x , IN.ase_texcoord3.xyz.y));
				float cos496 = cos( mTriUVRotation493 );
				float sin496 = sin( mTriUVRotation493 );
				float2 rotator496 = mul( ( mTriTiling455 * appendResult351 ) - float2( 0,0 ) , float2x2( cos496 , -sin496 , sin496 , cos496 )) + float2( 0,0 );
				float2 TriplanarUV_3417 = ( 1.0 - ( rotator496 + mTriOffset503 ) );
				float TriCom_3477 = BlendComponents278.z;
				float WorldObjectSwitch321 = _WorldtoObjectSwitch;
				float3 lerpResult353 = lerp( WorldPosition , IN.ase_texcoord3.xyz , WorldObjectSwitch321);
				float3 break361 = lerpResult353;
				float2 appendResult365 = (float2(break361.x , break361.z));
				float2 TriplanarTopUV418 = ( ( mTriTiling455 * appendResult365 ) + mTriOffset503 );
				float3 lerpResult342 = lerp( ase_worldNormal , mul( GetWorldToObjectMatrix(), float4( ase_worldNormal , 0.0 ) ).xyz , WorldObjectSwitch321);
				float3 temp_cast_11 = (_CoverageFalloff).xxx;
				float TriplanarColorWeight419 = pow( saturate( ( lerpResult342 + _CoverageAmount ) ) , temp_cast_11 ).y;
				float4 lerpResult461 = lerp( ( ( ( SAMPLE_TEXTURE2D( _TempTriplanarAlbedo, sampler_linear_repeat_aniso4, TriplanarUV_1415 ) * TriCom_1480 ) + ( SAMPLE_TEXTURE2D( _TempTriplanarAlbedo, sampler_linear_repeat_aniso4, TriplanarUV_2416 ) * TriCom_2479 ) ) + ( SAMPLE_TEXTURE2D( _TempTriplanarAlbedo, sampler_linear_repeat_aniso4, TriplanarUV_3417 ) * TriCom_3477 ) ) , SAMPLE_TEXTURE2D( _TempTopAlbedo, sampler_TempTopAlbedo, TriplanarTopUV418 ) , TriplanarColorWeight419);
				float4 TempTriAlbedo463 = lerpResult461;
				float4 temp_output_59_0_g11 = SplatControl26_g11;
				float FirstLayerControl66 = temp_output_59_0_g11.x;
				float4 lerpResult81 = lerp( AlbedoLayer67 , TempTriAlbedo463 , FirstLayerControl66);
				
				float AlphaLayer71 = SplatWeight22_g11;
				
				
				float3 Albedo = lerpResult81.rgb;
				float Alpha = AlphaLayer71;
				float AlphaClipThreshold = 0.5;

				half4 color = half4( Albedo, Alpha );

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				return color;
			}
			ENDHLSL
		}

		UsePass "Hidden/Nature/Terrain/Utilities/PICKING"
	UsePass "Hidden/Nature/Terrain/Utilities/SELECTION"

		Pass
		{
			
			Name "DepthNormals"
			Tags { "LightMode"="DepthNormals" }

			ZWrite On
			Blend One Zero
            ZTest LEqual
            ZWrite On

			HLSLPROGRAM
			
			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define ASE_FINAL_COLOR_ALPHA_MULTIPLY 1
			#define _SPECULAR_SETUP 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 999999
			#define ASE_USING_SAMPLING_MACROS 1

			
			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS SHADERPASS_DEPTHNORMALSONLY

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			#define ASE_NEEDS_VERT_TANGENT
			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_FRAG_POSITION
			#define ASE_NEEDS_FRAG_WORLD_NORMAL
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_VERT_POSITION
			#pragma multi_compile_instancing
			#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap forwardadd
			#pragma multi_compile_local __ _ALPHATEST_ON
			#pragma shader_feature_local _MASKMAP


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float3 worldNormal : TEXCOORD2;
				float4 worldTangent : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
				float4 ase_texcoord5 : TEXCOORD5;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _Control_ST;
			float4 _Specular1;
			float4 _Specular0;
			float4 _TerrainHolesTexture_ST;
			float4 _Specular2;
			float4 _Splat3_ST;
			float4 _Splat2_ST;
			float4 _Specular3;
			float4 _Splat0_ST;
			float4 _Splat1_ST;
			float2 _TriOffset;
			float _Smoothness0;
			float _Smoothness1;
			float _Metallic3;
			float _Smoothness3;
			float _Metallic2;
			float _TriTiling;
			float _TriUVRotation;
			float _WorldtoObjectSwitch;
			float _CoverageAmount;
			float _CoverageFalloff;
			float _Metallic1;
			float _Metallic0;
			float _Smoothness2;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			float4 _MaskMapRemapOffset2;
			float4 _MaskMapRemapScale2;
			float4 _MaskMapRemapScale0;
			float4 _MaskMapRemapOffset3;
			float4 _MaskMapRemapOffset1;
			float4 _MaskMapRemapOffset0;
			float4 _MaskMapRemapScale3;
			float4 _MaskMapRemapScale1;
			TEXTURE2D(_Mask1);
			SAMPLER(sampler_Mask1);
			TEXTURE2D(_Mask2);
			SAMPLER(sampler_Mask2);
			TEXTURE2D(_Mask3);
			SAMPLER(sampler_Mask3);
			TEXTURE2D(_Mask0);
			SAMPLER(sampler_Mask0);
			TEXTURE2D(_Control);
			SAMPLER(sampler_Control);
			TEXTURE2D(_Normal0);
			TEXTURE2D(_Splat0);
			SAMPLER(sampler_linear_repeat_aniso4);
			TEXTURE2D(_Normal1);
			TEXTURE2D(_Splat1);
			TEXTURE2D(_Normal2);
			TEXTURE2D(_Splat2);
			TEXTURE2D(_Normal3);
			TEXTURE2D(_Splat3);
			TEXTURE2D(_TriplanarNormal);
			TEXTURE2D(_TopNormal);
			SAMPLER(sampler_TopNormal);
			#ifdef UNITY_INSTANCING_ENABLED//ASE Terrain Instancing
				TEXTURE2D(_TerrainHeightmapTexture);//ASE Terrain Instancing
				TEXTURE2D( _TerrainNormalmapTexture);//ASE Terrain Instancing
				SAMPLER(sampler_TerrainNormalmapTexture);//ASE Terrain Instancing
			#endif//ASE Terrain Instancing
			UNITY_INSTANCING_BUFFER_START( Terrain )//ASE Terrain Instancing
				UNITY_DEFINE_INSTANCED_PROP( float4, _TerrainPatchInstanceData )//ASE Terrain Instancing
			UNITY_INSTANCING_BUFFER_END( Terrain)//ASE Terrain Instancing
			CBUFFER_START( UnityTerrain)//ASE Terrain Instancing
				#ifdef UNITY_INSTANCING_ENABLED//ASE Terrain Instancing
					float4 _TerrainHeightmapRecipSize;//ASE Terrain Instancing
					float4 _TerrainHeightmapScale;//ASE Terrain Instancing
				#endif//ASE Terrain Instancing
			CBUFFER_END//ASE Terrain Instancing


			VertexInput ApplyMeshModification( VertexInput v )
			{
			#ifdef UNITY_INSTANCING_ENABLED
				float2 patchVertex = v.vertex.xy;
				float4 instanceData = UNITY_ACCESS_INSTANCED_PROP( Terrain, _TerrainPatchInstanceData );
				float2 sampleCoords = ( patchVertex.xy + instanceData.xy ) * instanceData.z;
				float height = UnpackHeightmap( _TerrainHeightmapTexture.Load( int3( sampleCoords, 0 ) ) );
				v.vertex.xz = sampleCoords* _TerrainHeightmapScale.xz;
				v.vertex.y = height* _TerrainHeightmapScale.y;
				#ifdef ENABLE_TERRAIN_PERPIXEL_NORMAL
					v.ase_normal = float3(0, 1, 0);
				#else
					v.ase_normal = _TerrainNormalmapTexture.Load(int3(sampleCoords, 0)).rgb* 2 - 1;
				#endif
				v.ase_texcoord.xy = sampleCoords* _TerrainHeightmapRecipSize.zw;
			#endif
				return v;
			}
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				v = ApplyMeshModification(v);
				float3 localCalculateTangentsSRP76_g11 = ( ( v.ase_tangent.xyz * v.ase_normal * 0.0 ) );
				{
				v.ase_tangent.xyz = cross ( v.ase_normal, float3( 0, 0, 1 ) );
				v.ase_tangent.w = -1;
				}
				float3 TangetsAlpha72 = localCalculateTangentsSRP76_g11;
				
				o.ase_texcoord4.xy = v.ase_texcoord.xy;
				o.ase_texcoord5 = v.vertex;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord4.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = TangetsAlpha72;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;
				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float3 normalWS = TransformObjectToWorldNormal( v.ase_normal );
				float4 tangentWS = float4(TransformObjectToWorldDir( v.ase_tangent.xyz), v.ase_tangent.w);
				float4 positionCS = TransformWorldToHClip( positionWS );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif

				o.worldNormal = normalWS;
				o.worldTangent = tangentWS;

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = positionCS;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				o.clipPos = positionCS;
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_tangent = v.ase_tangent;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_tangent = patch[0].ase_tangent * bary.x + patch[1].ase_tangent * bary.y + patch[2].ase_tangent * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			#if defined(ASE_EARLY_Z_DEPTH_OPTIMIZE)
				#define ASE_SV_DEPTH SV_DepthLessEqual  
			#else
				#define ASE_SV_DEPTH SV_Depth
			#endif
			half4 frag(	VertexOutput IN 
						#ifdef ASE_DEPTH_WRITE_ON
						,out float outputDepth : ASE_SV_DEPTH
						#endif
						 ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );
				
				float3 WorldNormal = IN.worldNormal;
				float4 WorldTangent = IN.worldTangent;

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float2 uv_Control = IN.ase_texcoord4.xy * _Control_ST.xy + _Control_ST.zw;
				float4 tex2DNode5_g11 = SAMPLE_TEXTURE2D( _Control, sampler_Control, uv_Control );
				float dotResult20_g11 = dot( tex2DNode5_g11 , float4(1,1,1,1) );
				float SplatWeight22_g11 = dotResult20_g11;
				float localSplatClip74_g11 = ( SplatWeight22_g11 );
				float SplatWeight74_g11 = SplatWeight22_g11;
				{
				#if !defined(SHADER_API_MOBILE) && defined(TERRAIN_SPLAT_ADDPASS)
				clip(SplatWeight74_g11 == 0.0f ? -1 : 1);
				#endif
				}
				float4 SplatControl26_g11 = ( tex2DNode5_g11 / ( localSplatClip74_g11 + 0.001 ) );
				float4 temp_output_59_0_g11 = SplatControl26_g11;
				float2 uv_Splat0 = IN.ase_texcoord4.xy * _Splat0_ST.xy + _Splat0_ST.zw;
				float2 uv_Splat1 = IN.ase_texcoord4.xy * _Splat1_ST.xy + _Splat1_ST.zw;
				float2 uv_Splat2 = IN.ase_texcoord4.xy * _Splat2_ST.xy + _Splat2_ST.zw;
				float2 uv_Splat3 = IN.ase_texcoord4.xy * _Splat3_ST.xy + _Splat3_ST.zw;
				float4 weightedBlendVar8_g11 = temp_output_59_0_g11;
				float4 weightedBlend8_g11 = ( weightedBlendVar8_g11.x*SAMPLE_TEXTURE2D( _Normal0, sampler_linear_repeat_aniso4, uv_Splat0 ) + weightedBlendVar8_g11.y*SAMPLE_TEXTURE2D( _Normal1, sampler_linear_repeat_aniso4, uv_Splat1 ) + weightedBlendVar8_g11.z*SAMPLE_TEXTURE2D( _Normal2, sampler_linear_repeat_aniso4, uv_Splat2 ) + weightedBlendVar8_g11.w*SAMPLE_TEXTURE2D( _Normal3, sampler_linear_repeat_aniso4, uv_Splat3 ) );
				float3 temp_output_61_0_g11 = UnpackNormalScale( weightedBlend8_g11, 1.0 );
				float3 NormalLayer68 = temp_output_61_0_g11;
				float mTriTiling455 = _TriTiling;
				float2 appendResult352 = (float2(IN.ase_texcoord5.xyz.y , IN.ase_texcoord5.xyz.z));
				float mTriUVRotation493 = _TriUVRotation;
				float cos491 = cos( mTriUVRotation493 );
				float sin491 = sin( mTriUVRotation493 );
				float2 rotator491 = mul( ( mTriTiling455 * appendResult352 ) - float2( 0,0 ) , float2x2( cos491 , -sin491 , sin491 , cos491 )) + float2( 0,0 );
				float2 mTriOffset503 = _TriOffset;
				float2 TriplanarUV_1415 = ( 1.0 - ( rotator491 + mTriOffset503 ) );
				float3 temp_output_275_0 = abs( mul( GetWorldToObjectMatrix(), float4( WorldNormal , 0.0 ) ).xyz );
				float dotResult276 = dot( temp_output_275_0 , float3(1,1,1) );
				float3 BlendComponents278 = ( temp_output_275_0 / dotResult276 );
				float TriCom_1480 = BlendComponents278.x;
				float2 appendResult354 = (float2(IN.ase_texcoord5.xyz.x , IN.ase_texcoord5.xyz.z));
				float cos495 = cos( mTriUVRotation493 );
				float sin495 = sin( mTriUVRotation493 );
				float2 rotator495 = mul( ( mTriTiling455 * appendResult354 ) - float2( 0,0 ) , float2x2( cos495 , -sin495 , sin495 , cos495 )) + float2( 0,0 );
				float2 TriplanarUV_2416 = ( 1.0 - ( rotator495 + mTriOffset503 ) );
				float TriCom_2479 = BlendComponents278.y;
				float2 appendResult351 = (float2(IN.ase_texcoord5.xyz.x , IN.ase_texcoord5.xyz.y));
				float cos496 = cos( mTriUVRotation493 );
				float sin496 = sin( mTriUVRotation493 );
				float2 rotator496 = mul( ( mTriTiling455 * appendResult351 ) - float2( 0,0 ) , float2x2( cos496 , -sin496 , sin496 , cos496 )) + float2( 0,0 );
				float2 TriplanarUV_3417 = ( 1.0 - ( rotator496 + mTriOffset503 ) );
				float TriCom_3477 = BlendComponents278.z;
				float WorldObjectSwitch321 = _WorldtoObjectSwitch;
				float3 lerpResult353 = lerp( WorldPosition , IN.ase_texcoord5.xyz , WorldObjectSwitch321);
				float3 break361 = lerpResult353;
				float2 appendResult365 = (float2(break361.x , break361.z));
				float2 TriplanarTopUV418 = ( ( mTriTiling455 * appendResult365 ) + mTriOffset503 );
				float3 lerpResult342 = lerp( WorldNormal , mul( GetWorldToObjectMatrix(), float4( WorldNormal , 0.0 ) ).xyz , WorldObjectSwitch321);
				float3 temp_cast_7 = (_CoverageFalloff).xxx;
				float TriplanarColorWeight419 = pow( saturate( ( lerpResult342 + _CoverageAmount ) ) , temp_cast_7 ).y;
				float3 lerpResult448 = lerp( ( ( ( UnpackNormalScale( SAMPLE_TEXTURE2D( _TriplanarNormal, sampler_linear_repeat_aniso4, TriplanarUV_1415 ), 1.0f ) * TriCom_1480 ) + ( UnpackNormalScale( SAMPLE_TEXTURE2D( _TriplanarNormal, sampler_linear_repeat_aniso4, TriplanarUV_2416 ), 1.0f ) * TriCom_2479 ) ) + ( UnpackNormalScale( SAMPLE_TEXTURE2D( _TriplanarNormal, sampler_linear_repeat_aniso4, TriplanarUV_3417 ), 1.0f ) * TriCom_3477 ) ) , UnpackNormalScale( SAMPLE_TEXTURE2D( _TopNormal, sampler_TopNormal, TriplanarTopUV418 ), 1.0f ) , TriplanarColorWeight419);
				float3 TriNormal451 = lerpResult448;
				float FirstLayerControl66 = temp_output_59_0_g11.x;
				float3 lerpResult84 = lerp( NormalLayer68 , TriNormal451 , FirstLayerControl66);
				
				float AlphaLayer71 = SplatWeight22_g11;
				
				float3 Normal = lerpResult84;
				float Alpha = AlphaLayer71;
				float AlphaClipThreshold = 0.5;
				#ifdef ASE_DEPTH_WRITE_ON
				float DepthValue = 0;
				#endif

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif
				
				#ifdef ASE_DEPTH_WRITE_ON
				outputDepth = DepthValue;
				#endif
				
				#if defined(_GBUFFER_NORMALS_OCT)
					float2 octNormalWS = PackNormalOctQuadEncode(WorldNormal);
					float2 remappedOctNormalWS = saturate(octNormalWS * 0.5 + 0.5);
					half3 packedNormalWS = PackFloat2To888(remappedOctNormalWS);
					return half4(packedNormalWS, 0.0);
				#else
					
					#if defined(_NORMALMAP)
						#if _NORMAL_DROPOFF_TS
							float crossSign = (WorldTangent.w > 0.0 ? 1.0 : -1.0) * GetOddNegativeScale();
							float3 bitangent = crossSign * cross(WorldNormal.xyz, WorldTangent.xyz);
							float3 normalWS = TransformTangentToWorld(Normal, half3x3(WorldTangent.xyz, bitangent, WorldNormal.xyz));
						#elif _NORMAL_DROPOFF_OS
							float3 normalWS = TransformObjectToWorldNormal(Normal);
						#elif _NORMAL_DROPOFF_WS
							float3 normalWS = Normal;
						#endif
					#else
						float3 normalWS = WorldNormal;
					#endif

					return half4(NormalizeNormalPerPixel(normalWS), 0.0);
				#endif
			}
			ENDHLSL
		}

		UsePass "Hidden/Nature/Terrain/Utilities/PICKING"
	UsePass "Hidden/Nature/Terrain/Utilities/SELECTION"

		Pass
		{
			
			Name "GBuffer"
			Tags { "LightMode"="UniversalGBuffer" }
			
			Blend One Zero, One Zero
			ColorMask RGBA
			

			HLSLPROGRAM
			
			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define ASE_FINAL_COLOR_ALPHA_MULTIPLY 1
			#define _SPECULAR_SETUP 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 999999
			#define ASE_USING_SAMPLING_MACROS 1

			
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile _ DYNAMICLIGHTMAP_ON
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
			
			#pragma multi_compile _ _REFLECTION_PROBE_BLENDING
			#pragma multi_compile _ _REFLECTION_PROBE_BOX_PROJECTION

			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
			#pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
			#pragma multi_compile _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
			#pragma multi_compile _ _GBUFFER_NORMALS_OCT
			#pragma multi_compile _ _LIGHT_LAYERS
			#pragma multi_compile _ _RENDER_PASS_ENABLED

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS SHADERPASS_GBUFFER

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityGBuffer.hlsl"


			#if defined(UNITY_INSTANCING_ENABLED) && defined(_TERRAIN_INSTANCED_PERPIXEL_NORMAL)
			    #define ENABLE_TERRAIN_PERPIXEL_NORMAL
			#endif

			#define ASE_NEEDS_VERT_TANGENT
			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_FRAG_POSITION
			#define ASE_NEEDS_FRAG_WORLD_NORMAL
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_VERT_POSITION
			#pragma multi_compile_instancing
			#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap forwardadd
			#pragma multi_compile_local __ _ALPHATEST_ON
			#pragma shader_feature_local _MASKMAP


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 lightmapUVOrVertexSH : TEXCOORD0;
				half4 fogFactorAndVertexLight : TEXCOORD1;
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				float4 shadowCoord : TEXCOORD2;
				#endif
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
				#if defined(ASE_NEEDS_FRAG_SCREEN_POSITION)
				float4 screenPos : TEXCOORD6;
				#endif
				#if defined(DYNAMICLIGHTMAP_ON)
				float2 dynamicLightmapUV : TEXCOORD7;
				#endif
				float4 ase_texcoord8 : TEXCOORD8;
				float4 ase_texcoord9 : TEXCOORD9;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _Control_ST;
			float4 _Specular1;
			float4 _Specular0;
			float4 _TerrainHolesTexture_ST;
			float4 _Specular2;
			float4 _Splat3_ST;
			float4 _Splat2_ST;
			float4 _Specular3;
			float4 _Splat0_ST;
			float4 _Splat1_ST;
			float2 _TriOffset;
			float _Smoothness0;
			float _Smoothness1;
			float _Metallic3;
			float _Smoothness3;
			float _Metallic2;
			float _TriTiling;
			float _TriUVRotation;
			float _WorldtoObjectSwitch;
			float _CoverageAmount;
			float _CoverageFalloff;
			float _Metallic1;
			float _Metallic0;
			float _Smoothness2;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			float4 _MaskMapRemapOffset2;
			float4 _MaskMapRemapScale2;
			float4 _MaskMapRemapScale0;
			float4 _MaskMapRemapOffset3;
			float4 _MaskMapRemapOffset1;
			float4 _MaskMapRemapOffset0;
			float4 _MaskMapRemapScale3;
			float4 _MaskMapRemapScale1;
			TEXTURE2D(_Mask1);
			SAMPLER(sampler_Mask1);
			TEXTURE2D(_Mask2);
			SAMPLER(sampler_Mask2);
			TEXTURE2D(_Mask3);
			SAMPLER(sampler_Mask3);
			TEXTURE2D(_Mask0);
			SAMPLER(sampler_Mask0);
			TEXTURE2D(_Control);
			SAMPLER(sampler_Control);
			float _LayerHasMask0;
			float _LayerHasMask1;
			float _LayerHasMask2;
			float _LayerHasMask3;
			TEXTURE2D(_Splat0);
			TEXTURE2D(_Splat1);
			TEXTURE2D(_Splat2);
			TEXTURE2D(_Splat3);
			SAMPLER(sampler_linear_repeat_aniso4);
			float4 _DiffuseRemapScale0;
			float4 _DiffuseRemapScale1;
			float4 _DiffuseRemapScale2;
			float4 _DiffuseRemapScale3;
			TEXTURE2D(_TerrainHolesTexture);
			SAMPLER(sampler_TerrainHolesTexture);
			TEXTURE2D(_TempTriplanarAlbedo);
			TEXTURE2D(_TempTopAlbedo);
			SAMPLER(sampler_TempTopAlbedo);
			TEXTURE2D(_Normal0);
			TEXTURE2D(_Normal1);
			TEXTURE2D(_Normal2);
			TEXTURE2D(_Normal3);
			TEXTURE2D(_TriplanarNormal);
			TEXTURE2D(_TopNormal);
			SAMPLER(sampler_TopNormal);
			TEXTURE2D(_TriplanarMask);
			TEXTURE2D(_TopMask);
			#ifdef UNITY_INSTANCING_ENABLED//ASE Terrain Instancing
				TEXTURE2D(_TerrainHeightmapTexture);//ASE Terrain Instancing
				TEXTURE2D( _TerrainNormalmapTexture);//ASE Terrain Instancing
				SAMPLER(sampler_TerrainNormalmapTexture);//ASE Terrain Instancing
			#endif//ASE Terrain Instancing
			UNITY_INSTANCING_BUFFER_START( Terrain )//ASE Terrain Instancing
				UNITY_DEFINE_INSTANCED_PROP( float4, _TerrainPatchInstanceData )//ASE Terrain Instancing
			UNITY_INSTANCING_BUFFER_END( Terrain)//ASE Terrain Instancing
			CBUFFER_START( UnityTerrain)//ASE Terrain Instancing
				#ifdef UNITY_INSTANCING_ENABLED//ASE Terrain Instancing
					float4 _TerrainHeightmapRecipSize;//ASE Terrain Instancing
					float4 _TerrainHeightmapScale;//ASE Terrain Instancing
				#endif//ASE Terrain Instancing
			CBUFFER_END//ASE Terrain Instancing


			VertexInput ApplyMeshModification( VertexInput v )
			{
			#ifdef UNITY_INSTANCING_ENABLED
				float2 patchVertex = v.vertex.xy;
				float4 instanceData = UNITY_ACCESS_INSTANCED_PROP( Terrain, _TerrainPatchInstanceData );
				float2 sampleCoords = ( patchVertex.xy + instanceData.xy ) * instanceData.z;
				float height = UnpackHeightmap( _TerrainHeightmapTexture.Load( int3( sampleCoords, 0 ) ) );
				v.vertex.xz = sampleCoords* _TerrainHeightmapScale.xz;
				v.vertex.y = height* _TerrainHeightmapScale.y;
				#ifdef ENABLE_TERRAIN_PERPIXEL_NORMAL
					v.ase_normal = float3(0, 1, 0);
				#else
					v.ase_normal = _TerrainNormalmapTexture.Load(int3(sampleCoords, 0)).rgb* 2 - 1;
				#endif
				v.texcoord.xy = sampleCoords* _TerrainHeightmapRecipSize.zw;
			#endif
				return v;
			}
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				v = ApplyMeshModification(v);
				float3 localCalculateTangentsSRP76_g11 = ( ( v.ase_tangent.xyz * v.ase_normal * 0.0 ) );
				{
				v.ase_tangent.xyz = cross ( v.ase_normal, float3( 0, 0, 1 ) );
				v.ase_tangent.w = -1;
				}
				float3 TangetsAlpha72 = localCalculateTangentsSRP76_g11;
				
				o.ase_texcoord8.xy = v.texcoord.xy;
				o.ase_texcoord9 = v.vertex;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord8.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = TangetsAlpha72;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float3 positionVS = TransformWorldToView( positionWS );
				float4 positionCS = TransformWorldToHClip( positionWS );

				VertexNormalInputs normalInput = GetVertexNormalInputs( v.ase_normal, v.ase_tangent );

				o.tSpace0 = float4( normalInput.normalWS, positionWS.x);
				o.tSpace1 = float4( normalInput.tangentWS, positionWS.y);
				o.tSpace2 = float4( normalInput.bitangentWS, positionWS.z);

				OUTPUT_LIGHTMAP_UV( v.texcoord1, unity_LightmapST, o.lightmapUVOrVertexSH.xy );
				#if defined(DYNAMICLIGHTMAP_ON)
				o.dynamicLightmapUV.xy = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
				#endif

				OUTPUT_SH( normalInput.normalWS.xyz, o.lightmapUVOrVertexSH.xyz );

				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					o.lightmapUVOrVertexSH.zw = v.texcoord;
					o.lightmapUVOrVertexSH.xy = v.texcoord * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif

				half3 vertexLight = VertexLighting( positionWS, normalInput.normalWS );
				#ifdef ASE_FOG
					half fogFactor = ComputeFogFactor( positionCS.z );
				#else
					half fogFactor = 0;
				#endif
				o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
				
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				VertexPositionInputs vertexInput = (VertexPositionInputs)0;
				vertexInput.positionWS = positionWS;
				vertexInput.positionCS = positionCS;
				o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				
				o.clipPos = positionCS;
				#if defined(ASE_NEEDS_FRAG_SCREEN_POSITION)
				o.screenPos = ComputeScreenPos(positionCS);
				#endif
				return o;
			}
			
			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_tangent = v.ase_tangent;
				o.texcoord = v.texcoord;
				o.texcoord1 = v.texcoord1;
				o.texcoord2 = v.texcoord2;
				
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_tangent = patch[0].ase_tangent * bary.x + patch[1].ase_tangent * bary.y + patch[2].ase_tangent * bary.z;
				o.texcoord = patch[0].texcoord * bary.x + patch[1].texcoord * bary.y + patch[2].texcoord * bary.z;
				o.texcoord1 = patch[0].texcoord1 * bary.x + patch[1].texcoord1 * bary.y + patch[2].texcoord1 * bary.z;
				o.texcoord2 = patch[0].texcoord2 * bary.x + patch[1].texcoord2 * bary.y + patch[2].texcoord2 * bary.z;
				
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			#if defined(ASE_EARLY_Z_DEPTH_OPTIMIZE)
				#define ASE_SV_DEPTH SV_DepthLessEqual  
			#else
				#define ASE_SV_DEPTH SV_Depth
			#endif
			FragmentOutput frag ( VertexOutput IN 
								#ifdef ASE_DEPTH_WRITE_ON
								,out float outputDepth : ASE_SV_DEPTH
								#endif
								 )
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif

				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					float2 sampleCoords = (IN.lightmapUVOrVertexSH.zw / _TerrainHeightmapRecipSize.zw + 0.5f) * _TerrainHeightmapRecipSize.xy;
					float3 WorldNormal = TransformObjectToWorldNormal(normalize(SAMPLE_TEXTURE2D(_TerrainNormalmapTexture, sampler_TerrainNormalmapTexture, sampleCoords).rgb * 2 - 1));
					float3 WorldTangent = -cross(GetObjectToWorldMatrix()._13_23_33, WorldNormal);
					float3 WorldBiTangent = cross(WorldNormal, -WorldTangent);
				#else
					float3 WorldNormal = normalize( IN.tSpace0.xyz );
					float3 WorldTangent = IN.tSpace1.xyz;
					float3 WorldBiTangent = IN.tSpace2.xyz;
				#endif
				float3 WorldPosition = float3(IN.tSpace0.w,IN.tSpace1.w,IN.tSpace2.w);
				float3 WorldViewDirection = _WorldSpaceCameraPos.xyz  - WorldPosition;
				float4 ShadowCoords = float4( 0, 0, 0, 0 );
				#if defined(ASE_NEEDS_FRAG_SCREEN_POSITION)
				float4 ScreenPos = IN.screenPos;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
					ShadowCoords = IN.shadowCoord;
				#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
					ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
				#else
					ShadowCoords = float4(0, 0, 0, 0);
				#endif


	
				WorldViewDirection = SafeNormalize( WorldViewDirection );

				float2 uv_Control = IN.ase_texcoord8.xy * _Control_ST.xy + _Control_ST.zw;
				float4 tex2DNode5_g11 = SAMPLE_TEXTURE2D( _Control, sampler_Control, uv_Control );
				float dotResult20_g11 = dot( tex2DNode5_g11 , float4(1,1,1,1) );
				float SplatWeight22_g11 = dotResult20_g11;
				float localSplatClip74_g11 = ( SplatWeight22_g11 );
				float SplatWeight74_g11 = SplatWeight22_g11;
				{
				#if !defined(SHADER_API_MOBILE) && defined(TERRAIN_SPLAT_ADDPASS)
				clip(SplatWeight74_g11 == 0.0f ? -1 : 1);
				#endif
				}
				float4 SplatControl26_g11 = ( tex2DNode5_g11 / ( localSplatClip74_g11 + 0.001 ) );
				float4 appendResult55_g11 = (float4(_Metallic0 , _Metallic1 , _Metallic2 , _Metallic3));
				float localComputeMasks130_g11 = ( 0.0 );
				float4 masks0130_g11 = float4( 0,0,0,0 );
				float4 masks1130_g11 = float4( 0,0,0,0 );
				float4 masks2130_g11 = float4( 0,0,0,0 );
				float4 masks3130_g11 = float4( 0,0,0,0 );
				float4 appendResult135_g11 = (float4(_LayerHasMask0 , _LayerHasMask1 , _LayerHasMask2 , _LayerHasMask3));
				float4 layerHasMask136_g11 = appendResult135_g11;
				float4 hasMask130_g11 = layerHasMask136_g11;
				float2 uv_Splat0 = IN.ase_texcoord8.xy * _Splat0_ST.xy + _Splat0_ST.zw;
				float2 uvMask0130_g11 = uv_Splat0;
				float2 uv_Splat1 = IN.ase_texcoord8.xy * _Splat1_ST.xy + _Splat1_ST.zw;
				float2 uvMask1130_g11 = uv_Splat1;
				float2 uv_Splat2 = IN.ase_texcoord8.xy * _Splat2_ST.xy + _Splat2_ST.zw;
				float2 uvMask2130_g11 = uv_Splat2;
				float2 uv_Splat3 = IN.ase_texcoord8.xy * _Splat3_ST.xy + _Splat3_ST.zw;
				float2 uvMask3130_g11 = uv_Splat3;
				SamplerState SamplerS130_g11 = sampler_linear_repeat_aniso4;
				{
				masks0130_g11 = 0.5h;
				masks1130_g11 = 0.5h;
				masks2130_g11 = 0.5h;
				masks3130_g11 = 0.5h;
				#ifdef _MASKMAP
				masks0130_g11 = lerp(masks0130_g11, SAMPLE_TEXTURE2D(_Mask0, SamplerS130_g11, uvMask0130_g11), hasMask130_g11.x);
				masks1130_g11 = lerp(masks1130_g11, SAMPLE_TEXTURE2D(_Mask1, SamplerS130_g11, uvMask1130_g11), hasMask130_g11.y);
				masks2130_g11 = lerp(masks2130_g11, SAMPLE_TEXTURE2D(_Mask2, SamplerS130_g11, uvMask2130_g11), hasMask130_g11.z);
				masks3130_g11 = lerp(masks3130_g11, SAMPLE_TEXTURE2D(_Mask3, SamplerS130_g11, uvMask3130_g11), hasMask130_g11.w);
				#endif
				masks0130_g11 *= _MaskMapRemapScale0.rgba;
				masks0130_g11 += _MaskMapRemapOffset0.rgba;
				masks1130_g11 *= _MaskMapRemapScale1.rgba;
				masks1130_g11 += _MaskMapRemapOffset1.rgba;
				masks2130_g11 *= _MaskMapRemapScale2.rgba;
				masks2130_g11 += _MaskMapRemapOffset2.rgba;
				masks3130_g11 *= _MaskMapRemapScale3.rgba;
				masks3130_g11 += _MaskMapRemapOffset3.rgba;
				}
				float4 mask0138_g11 = masks0130_g11;
				float4 mask1139_g11 = masks1130_g11;
				float4 mask2140_g11 = masks2130_g11;
				float4 mask3141_g11 = masks3130_g11;
				float4 appendResult168_g11 = (float4((mask0138_g11).x , (mask1139_g11).x , (mask2140_g11).x , (mask3141_g11).x));
				float4 maskMetallic169_g11 = appendResult168_g11;
				float4 lerpResult202_g11 = lerp( appendResult55_g11 , maskMetallic169_g11 , layerHasMask136_g11);
				float dotResult53_g11 = dot( SplatControl26_g11 , lerpResult202_g11 );
				float temp_output_527_56 = dotResult53_g11;
				float4 appendResult33_g11 = (float4(1.0 , 1.0 , 1.0 , _Smoothness0));
				float4 tex2DNode4_g11 = SAMPLE_TEXTURE2D( _Splat0, sampler_linear_repeat_aniso4, uv_Splat0 );
				float4 appendResult258_g11 = (float4(( (SplatControl26_g11).rrr * (_DiffuseRemapScale0).rgb ) , 1.0));
				float4 tintLayer0253_g11 = appendResult258_g11;
				float4 appendResult36_g11 = (float4(1.0 , 1.0 , 1.0 , _Smoothness1));
				float4 tex2DNode3_g11 = SAMPLE_TEXTURE2D( _Splat1, sampler_linear_repeat_aniso4, uv_Splat1 );
				float4 appendResult261_g11 = (float4(( (SplatControl26_g11).ggg * (_DiffuseRemapScale1).rgb ) , 1.0));
				float4 tintLayer1254_g11 = appendResult261_g11;
				float4 appendResult39_g11 = (float4(1.0 , 1.0 , 1.0 , _Smoothness2));
				float4 tex2DNode6_g11 = SAMPLE_TEXTURE2D( _Splat2, sampler_linear_repeat_aniso4, uv_Splat2 );
				float4 appendResult263_g11 = (float4(( (SplatControl26_g11).bbb * (_DiffuseRemapScale2).rgb ) , 1.0));
				float4 tintLayer2255_g11 = appendResult263_g11;
				float4 appendResult42_g11 = (float4(1.0 , 1.0 , 1.0 , _Smoothness3));
				float4 tex2DNode7_g11 = SAMPLE_TEXTURE2D( _Splat3, sampler_linear_repeat_aniso4, uv_Splat3 );
				float4 appendResult265_g11 = (float4(( (SplatControl26_g11).aaa * (_DiffuseRemapScale3).rgb ) , 1.0));
				float4 tintLayer3256_g11 = appendResult265_g11;
				float4 weightedBlendVar9_g11 = float4(1,1,1,1);
				float4 weightedBlend9_g11 = ( weightedBlendVar9_g11.x*( appendResult33_g11 * tex2DNode4_g11 * tintLayer0253_g11 ) + weightedBlendVar9_g11.y*( appendResult36_g11 * tex2DNode3_g11 * tintLayer1254_g11 ) + weightedBlendVar9_g11.z*( appendResult39_g11 * tex2DNode6_g11 * tintLayer2255_g11 ) + weightedBlendVar9_g11.w*( appendResult42_g11 * tex2DNode7_g11 * tintLayer3256_g11 ) );
				float4 MixDiffuse28_g11 = weightedBlend9_g11;
				float4 temp_output_60_0_g11 = MixDiffuse28_g11;
				float4 localClipHoles100_g11 = ( temp_output_60_0_g11 );
				float2 uv_TerrainHolesTexture = IN.ase_texcoord8.xy * _TerrainHolesTexture_ST.xy + _TerrainHolesTexture_ST.zw;
				float holeClipValue99_g11 = SAMPLE_TEXTURE2D( _TerrainHolesTexture, sampler_TerrainHolesTexture, uv_TerrainHolesTexture ).r;
				float Hole100_g11 = holeClipValue99_g11;
				{
				#ifdef _ALPHATEST_ON
				clip(Hole100_g11 == 0.0f ? -1 : 1);
				#endif
				}
				float4 AlbedoLayer67 = ( temp_output_527_56 * localClipHoles100_g11 );
				float mTriTiling455 = _TriTiling;
				float2 appendResult352 = (float2(IN.ase_texcoord9.xyz.y , IN.ase_texcoord9.xyz.z));
				float mTriUVRotation493 = _TriUVRotation;
				float cos491 = cos( mTriUVRotation493 );
				float sin491 = sin( mTriUVRotation493 );
				float2 rotator491 = mul( ( mTriTiling455 * appendResult352 ) - float2( 0,0 ) , float2x2( cos491 , -sin491 , sin491 , cos491 )) + float2( 0,0 );
				float2 mTriOffset503 = _TriOffset;
				float2 TriplanarUV_1415 = ( 1.0 - ( rotator491 + mTriOffset503 ) );
				float3 temp_output_275_0 = abs( mul( GetWorldToObjectMatrix(), float4( WorldNormal , 0.0 ) ).xyz );
				float dotResult276 = dot( temp_output_275_0 , float3(1,1,1) );
				float3 BlendComponents278 = ( temp_output_275_0 / dotResult276 );
				float TriCom_1480 = BlendComponents278.x;
				float2 appendResult354 = (float2(IN.ase_texcoord9.xyz.x , IN.ase_texcoord9.xyz.z));
				float cos495 = cos( mTriUVRotation493 );
				float sin495 = sin( mTriUVRotation493 );
				float2 rotator495 = mul( ( mTriTiling455 * appendResult354 ) - float2( 0,0 ) , float2x2( cos495 , -sin495 , sin495 , cos495 )) + float2( 0,0 );
				float2 TriplanarUV_2416 = ( 1.0 - ( rotator495 + mTriOffset503 ) );
				float TriCom_2479 = BlendComponents278.y;
				float2 appendResult351 = (float2(IN.ase_texcoord9.xyz.x , IN.ase_texcoord9.xyz.y));
				float cos496 = cos( mTriUVRotation493 );
				float sin496 = sin( mTriUVRotation493 );
				float2 rotator496 = mul( ( mTriTiling455 * appendResult351 ) - float2( 0,0 ) , float2x2( cos496 , -sin496 , sin496 , cos496 )) + float2( 0,0 );
				float2 TriplanarUV_3417 = ( 1.0 - ( rotator496 + mTriOffset503 ) );
				float TriCom_3477 = BlendComponents278.z;
				float WorldObjectSwitch321 = _WorldtoObjectSwitch;
				float3 lerpResult353 = lerp( WorldPosition , IN.ase_texcoord9.xyz , WorldObjectSwitch321);
				float3 break361 = lerpResult353;
				float2 appendResult365 = (float2(break361.x , break361.z));
				float2 TriplanarTopUV418 = ( ( mTriTiling455 * appendResult365 ) + mTriOffset503 );
				float3 lerpResult342 = lerp( WorldNormal , mul( GetWorldToObjectMatrix(), float4( WorldNormal , 0.0 ) ).xyz , WorldObjectSwitch321);
				float3 temp_cast_11 = (_CoverageFalloff).xxx;
				float TriplanarColorWeight419 = pow( saturate( ( lerpResult342 + _CoverageAmount ) ) , temp_cast_11 ).y;
				float4 lerpResult461 = lerp( ( ( ( SAMPLE_TEXTURE2D( _TempTriplanarAlbedo, sampler_linear_repeat_aniso4, TriplanarUV_1415 ) * TriCom_1480 ) + ( SAMPLE_TEXTURE2D( _TempTriplanarAlbedo, sampler_linear_repeat_aniso4, TriplanarUV_2416 ) * TriCom_2479 ) ) + ( SAMPLE_TEXTURE2D( _TempTriplanarAlbedo, sampler_linear_repeat_aniso4, TriplanarUV_3417 ) * TriCom_3477 ) ) , SAMPLE_TEXTURE2D( _TempTopAlbedo, sampler_TempTopAlbedo, TriplanarTopUV418 ) , TriplanarColorWeight419);
				float4 TempTriAlbedo463 = lerpResult461;
				float4 temp_output_59_0_g11 = SplatControl26_g11;
				float FirstLayerControl66 = temp_output_59_0_g11.x;
				float4 lerpResult81 = lerp( AlbedoLayer67 , TempTriAlbedo463 , FirstLayerControl66);
				
				float4 weightedBlendVar8_g11 = temp_output_59_0_g11;
				float4 weightedBlend8_g11 = ( weightedBlendVar8_g11.x*SAMPLE_TEXTURE2D( _Normal0, sampler_linear_repeat_aniso4, uv_Splat0 ) + weightedBlendVar8_g11.y*SAMPLE_TEXTURE2D( _Normal1, sampler_linear_repeat_aniso4, uv_Splat1 ) + weightedBlendVar8_g11.z*SAMPLE_TEXTURE2D( _Normal2, sampler_linear_repeat_aniso4, uv_Splat2 ) + weightedBlendVar8_g11.w*SAMPLE_TEXTURE2D( _Normal3, sampler_linear_repeat_aniso4, uv_Splat3 ) );
				float3 temp_output_61_0_g11 = UnpackNormalScale( weightedBlend8_g11, 1.0 );
				float3 NormalLayer68 = temp_output_61_0_g11;
				float3 lerpResult448 = lerp( ( ( ( UnpackNormalScale( SAMPLE_TEXTURE2D( _TriplanarNormal, sampler_linear_repeat_aniso4, TriplanarUV_1415 ), 1.0f ) * TriCom_1480 ) + ( UnpackNormalScale( SAMPLE_TEXTURE2D( _TriplanarNormal, sampler_linear_repeat_aniso4, TriplanarUV_2416 ), 1.0f ) * TriCom_2479 ) ) + ( UnpackNormalScale( SAMPLE_TEXTURE2D( _TriplanarNormal, sampler_linear_repeat_aniso4, TriplanarUV_3417 ), 1.0f ) * TriCom_3477 ) ) , UnpackNormalScale( SAMPLE_TEXTURE2D( _TopNormal, sampler_TopNormal, TriplanarTopUV418 ), 1.0f ) , TriplanarColorWeight419);
				float3 TriNormal451 = lerpResult448;
				float3 lerpResult84 = lerp( NormalLayer68 , TriNormal451 , FirstLayerControl66);
				
				float SpecularLayer76 = 0.0;
				float4 temp_cast_15 = (SpecularLayer76).xxxx;
				float4 lerpResult413 = lerp( ( ( ( SAMPLE_TEXTURE2D( _TriplanarMask, sampler_linear_repeat_aniso4, TriplanarUV_1415 ) * TriCom_1480 ) + ( SAMPLE_TEXTURE2D( _TriplanarMask, sampler_linear_repeat_aniso4, TriplanarUV_2416 ) * TriCom_2479 ) ) + ( SAMPLE_TEXTURE2D( _TriplanarMask, sampler_linear_repeat_aniso4, TriplanarUV_3417 ) * TriCom_3477 ) ) , SAMPLE_TEXTURE2D( _TopMask, sampler_linear_repeat_aniso4, TriplanarTopUV418 ) , TriplanarColorWeight419);
				float4 TriSpecular429 = lerpResult413;
				float4 lerpResult87 = lerp( temp_cast_15 , TriSpecular429 , FirstLayerControl66);
				
				float4 appendResult205_g11 = (float4(_Smoothness0 , _Smoothness1 , _Smoothness2 , _Smoothness3));
				float4 appendResult206_g11 = (float4(tex2DNode4_g11.a , tex2DNode3_g11.a , tex2DNode6_g11.a , tex2DNode7_g11.a));
				float4 defaultSmoothness210_g11 = ( appendResult205_g11 * appendResult206_g11 );
				float4 appendResult158_g11 = (float4((mask0138_g11).w , (mask1139_g11).w , (mask2140_g11).w , (mask3141_g11).w));
				float4 maskSmoothness149_g11 = appendResult158_g11;
				float4 lerpResult215_g11 = lerp( defaultSmoothness210_g11 , maskSmoothness149_g11 , layerHasMask136_g11);
				float dotResult216_g11 = dot( lerpResult215_g11 , SplatControl26_g11 );
				float4 weightedBlendVar295_g11 = float4(1,1,1,1);
				float4 weightedBlend295_g11 = ( weightedBlendVar295_g11.x*_Specular0 + weightedBlendVar295_g11.y*_Specular1 + weightedBlendVar295_g11.z*_Specular2 + weightedBlendVar295_g11.w*_Specular3 );
				float mLayerSmootness515 = ( dotResult216_g11 * weightedBlend295_g11.r );
				float TriSmoothness428 = lerpResult413.a;
				float lerpResult90 = lerp( mLayerSmootness515 , TriSmoothness428 , FirstLayerControl66);
				
				float AlphaLayer71 = SplatWeight22_g11;
				
				float3 Albedo = lerpResult81.rgb;
				float3 Normal = lerpResult84;
				float3 Emission = 0;
				float3 Specular = lerpResult87.rgb;
				float Metallic = 0;
				float Smoothness = lerpResult90;
				float Occlusion = 1;
				float Alpha = AlphaLayer71;
				float AlphaClipThreshold = 0.5;
				float AlphaClipThresholdShadow = 0.5;
				float3 BakedGI = 0;
				float3 RefractionColor = 1;
				float RefractionIndex = 1;
				float3 Transmission = 1;
				float3 Translucency = 1;
				#ifdef ASE_DEPTH_WRITE_ON
				float DepthValue = 0;
				#endif

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				InputData inputData = (InputData)0;
				inputData.positionWS = WorldPosition;
				inputData.positionCS = IN.clipPos;
				inputData.shadowCoord = ShadowCoords;



				#ifdef _NORMALMAP
					#if _NORMAL_DROPOFF_TS
					inputData.normalWS = TransformTangentToWorld(Normal, half3x3( WorldTangent, WorldBiTangent, WorldNormal ));
					#elif _NORMAL_DROPOFF_OS
					inputData.normalWS = TransformObjectToWorldNormal(Normal);
					#elif _NORMAL_DROPOFF_WS
					inputData.normalWS = Normal;
					#endif
				#else
					inputData.normalWS = WorldNormal;
				#endif
					
				inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);
				inputData.viewDirectionWS = SafeNormalize( WorldViewDirection );



				#ifdef ASE_FOG
					inputData.fogCoord = InitializeInputDataFog(float4(WorldPosition, 1.0),  IN.fogFactorAndVertexLight.x);
				#endif

				inputData.vertexLighting = IN.fogFactorAndVertexLight.yzw;

				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					float3 SH = SampleSH(inputData.normalWS.xyz);
				#else
					float3 SH = IN.lightmapUVOrVertexSH.xyz;
				#endif

				

				#ifdef _ASE_BAKEDGI
					inputData.bakedGI = BakedGI;
				#else
					#if defined(DYNAMICLIGHTMAP_ON)
						inputData.bakedGI = SAMPLE_GI( IN.lightmapUVOrVertexSH.xy, IN.dynamicLightmapUV.xy, SH, inputData.normalWS);
					#else
						inputData.bakedGI = SAMPLE_GI( IN.lightmapUVOrVertexSH.xy, SH, inputData.normalWS );
					#endif
				#endif

				inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(IN.clipPos);
				inputData.shadowMask = SAMPLE_SHADOWMASK(IN.lightmapUVOrVertexSH.xy);

				#if defined(DEBUG_DISPLAY)
					#if defined(DYNAMICLIGHTMAP_ON)
						inputData.dynamicLightmapUV = IN.dynamicLightmapUV.xy;
						#endif
					#if defined(LIGHTMAP_ON)
						inputData.staticLightmapUV = IN.lightmapUVOrVertexSH.xy;
					#else
						inputData.vertexSH = SH;
					#endif
				#endif

				#ifdef _DBUFFER
					ApplyDecal(IN.clipPos,
						Albedo,
						Specular,
						inputData.normalWS,
						Metallic,
						Occlusion,
						Smoothness);
				#endif

				BRDFData brdfData;
				InitializeBRDFData
				(Albedo, Metallic, Specular, Smoothness, Alpha, brdfData);

				Light mainLight = GetMainLight(inputData.shadowCoord, inputData.positionWS, inputData.shadowMask);
				half4 color;
				MixRealtimeAndBakedGI(mainLight, inputData.normalWS, inputData.bakedGI, inputData.shadowMask);
				color.rgb = GlobalIllumination(brdfData, inputData.bakedGI, Occlusion, inputData.positionWS, inputData.normalWS, inputData.viewDirectionWS);
				color.a = Alpha;
				
				#ifdef ASE_FINAL_COLOR_ALPHA_MULTIPLY
					color.rgb *= color.a;
				#endif
				
				#ifdef ASE_FOG
					#ifdef TERRAIN_SPLAT_ADDPASS
						color.rgb = MixFogColor(color.rgb, half3( 0, 0, 0 ), IN.fogFactorAndVertexLight.x );
					#else
						color.rgb = MixFog(color.rgb, IN.fogFactorAndVertexLight.x);
					#endif
				#endif
				
				#ifdef ASE_DEPTH_WRITE_ON
					outputDepth = DepthValue;
				#endif
				
				return BRDFDataToGbuffer(brdfData, inputData, Smoothness, Emission + color.rgb);
			}

			ENDHLSL
		}

		UsePass "Hidden/Nature/Terrain/Utilities/PICKING"
	UsePass "Hidden/Nature/Terrain/Utilities/SELECTION"

        Pass
        {
			
            Name "SceneSelectionPass"
            Tags { "LightMode"="SceneSelectionPass" }
        
			Cull Off

			HLSLPROGRAM
        
			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define ASE_FINAL_COLOR_ALPHA_MULTIPLY 1
			#define _SPECULAR_SETUP 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 999999
			#define ASE_USING_SAMPLING_MACROS 1

        
			#pragma only_renderers d3d11 glcore gles gles3 
			#pragma vertex vert
			#pragma fragment frag

			#define ATTRIBUTES_NEED_NORMAL
			#define ATTRIBUTES_NEED_TANGENT
			#define SHADERPASS SHADERPASS_DEPTHONLY

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_VERT_POSITION
			#pragma multi_compile_instancing
			#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap forwardadd
			#pragma multi_compile_local __ _ALPHATEST_ON
			#pragma shader_feature_local _MASKMAP


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
        
			CBUFFER_START(UnityPerMaterial)
			float4 _Control_ST;
			float4 _Specular1;
			float4 _Specular0;
			float4 _TerrainHolesTexture_ST;
			float4 _Specular2;
			float4 _Splat3_ST;
			float4 _Splat2_ST;
			float4 _Specular3;
			float4 _Splat0_ST;
			float4 _Splat1_ST;
			float2 _TriOffset;
			float _Smoothness0;
			float _Smoothness1;
			float _Metallic3;
			float _Smoothness3;
			float _Metallic2;
			float _TriTiling;
			float _TriUVRotation;
			float _WorldtoObjectSwitch;
			float _CoverageAmount;
			float _CoverageFalloff;
			float _Metallic1;
			float _Metallic0;
			float _Smoothness2;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			float4 _MaskMapRemapOffset2;
			float4 _MaskMapRemapScale2;
			float4 _MaskMapRemapScale0;
			float4 _MaskMapRemapOffset3;
			float4 _MaskMapRemapOffset1;
			float4 _MaskMapRemapOffset0;
			float4 _MaskMapRemapScale3;
			float4 _MaskMapRemapScale1;
			TEXTURE2D(_Mask1);
			SAMPLER(sampler_Mask1);
			TEXTURE2D(_Mask2);
			SAMPLER(sampler_Mask2);
			TEXTURE2D(_Mask3);
			SAMPLER(sampler_Mask3);
			TEXTURE2D(_Mask0);
			SAMPLER(sampler_Mask0);
			TEXTURE2D(_Control);
			SAMPLER(sampler_Control);
			#ifdef UNITY_INSTANCING_ENABLED//ASE Terrain Instancing
				TEXTURE2D(_TerrainHeightmapTexture);//ASE Terrain Instancing
				TEXTURE2D( _TerrainNormalmapTexture);//ASE Terrain Instancing
				SAMPLER(sampler_TerrainNormalmapTexture);//ASE Terrain Instancing
			#endif//ASE Terrain Instancing
			UNITY_INSTANCING_BUFFER_START( Terrain )//ASE Terrain Instancing
				UNITY_DEFINE_INSTANCED_PROP( float4, _TerrainPatchInstanceData )//ASE Terrain Instancing
			UNITY_INSTANCING_BUFFER_END( Terrain)//ASE Terrain Instancing
			CBUFFER_START( UnityTerrain)//ASE Terrain Instancing
				#ifdef UNITY_INSTANCING_ENABLED//ASE Terrain Instancing
					float4 _TerrainHeightmapRecipSize;//ASE Terrain Instancing
					float4 _TerrainHeightmapScale;//ASE Terrain Instancing
				#endif//ASE Terrain Instancing
			CBUFFER_END//ASE Terrain Instancing


			VertexInput ApplyMeshModification( VertexInput v )
			{
			#ifdef UNITY_INSTANCING_ENABLED
				float2 patchVertex = v.vertex.xy;
				float4 instanceData = UNITY_ACCESS_INSTANCED_PROP( Terrain, _TerrainPatchInstanceData );
				float2 sampleCoords = ( patchVertex.xy + instanceData.xy ) * instanceData.z;
				float height = UnpackHeightmap( _TerrainHeightmapTexture.Load( int3( sampleCoords, 0 ) ) );
				v.vertex.xz = sampleCoords* _TerrainHeightmapScale.xz;
				v.vertex.y = height* _TerrainHeightmapScale.y;
				#ifdef ENABLE_TERRAIN_PERPIXEL_NORMAL
					v.ase_normal = float3(0, 1, 0);
				#else
					v.ase_normal = _TerrainNormalmapTexture.Load(int3(sampleCoords, 0)).rgb* 2 - 1;
				#endif
				v.ase_texcoord.xy = sampleCoords* _TerrainHeightmapRecipSize.zw;
			#endif
				return v;
			}
			

			int _ObjectId;
			int _PassValue;

			struct SurfaceDescription
			{
				float Alpha;
				float AlphaClipThreshold;
			};
        
			VertexOutput VertexFunction(VertexInput v  )
			{
				VertexOutput o;
				ZERO_INITIALIZE(VertexOutput, o);

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);


				v = ApplyMeshModification(v);
				float3 localCalculateTangentsSRP76_g11 = ( ( v.ase_tangent.xyz * v.ase_normal * 0.0 ) );
				{
				v.ase_tangent.xyz = cross ( v.ase_normal, float3( 0, 0, 1 ) );
				v.ase_tangent.w = -1;
				}
				float3 TangetsAlpha72 = localCalculateTangentsSRP76_g11;
				
				o.ase_texcoord.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = TangetsAlpha72;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				o.clipPos = TransformWorldToHClip(positionWS);
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_tangent = v.ase_tangent;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_tangent = patch[0].ase_tangent * bary.x + patch[1].ase_tangent * bary.y + patch[2].ase_tangent * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif
			
			half4 frag(VertexOutput IN ) : SV_TARGET
			{
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				float2 uv_Control = IN.ase_texcoord.xy * _Control_ST.xy + _Control_ST.zw;
				float4 tex2DNode5_g11 = SAMPLE_TEXTURE2D( _Control, sampler_Control, uv_Control );
				float dotResult20_g11 = dot( tex2DNode5_g11 , float4(1,1,1,1) );
				float SplatWeight22_g11 = dotResult20_g11;
				float AlphaLayer71 = SplatWeight22_g11;
				
				surfaceDescription.Alpha = AlphaLayer71;
				surfaceDescription.AlphaClipThreshold = 0.5;


				#if _ALPHATEST_ON
					float alphaClipThreshold = 0.01f;
					#if ALPHA_CLIP_THRESHOLD
						alphaClipThreshold = surfaceDescription.AlphaClipThreshold;
					#endif
					clip(surfaceDescription.Alpha - alphaClipThreshold);
				#endif

				half4 outColor = half4(_ObjectId, _PassValue, 1.0, 1.0);
				return outColor;
			}

			ENDHLSL
        }

		UsePass "Hidden/Nature/Terrain/Utilities/PICKING"
	UsePass "Hidden/Nature/Terrain/Utilities/SELECTION"

        Pass
        {
			
            Name "ScenePickingPass"
            Tags { "LightMode"="Picking" }
        
			HLSLPROGRAM

			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define ASE_FINAL_COLOR_ALPHA_MULTIPLY 1
			#define _SPECULAR_SETUP 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 999999
			#define ASE_USING_SAMPLING_MACROS 1


			#pragma only_renderers d3d11 glcore gles gles3 
			#pragma vertex vert
			#pragma fragment frag

        
			#define ATTRIBUTES_NEED_NORMAL
			#define ATTRIBUTES_NEED_TANGENT
			#define SHADERPASS SHADERPASS_DEPTHONLY
			

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_VERT_POSITION
			#pragma multi_compile_instancing
			#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap forwardadd
			#pragma multi_compile_local __ _ALPHATEST_ON
			#pragma shader_feature_local _MASKMAP


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
        
			CBUFFER_START(UnityPerMaterial)
			float4 _Control_ST;
			float4 _Specular1;
			float4 _Specular0;
			float4 _TerrainHolesTexture_ST;
			float4 _Specular2;
			float4 _Splat3_ST;
			float4 _Splat2_ST;
			float4 _Specular3;
			float4 _Splat0_ST;
			float4 _Splat1_ST;
			float2 _TriOffset;
			float _Smoothness0;
			float _Smoothness1;
			float _Metallic3;
			float _Smoothness3;
			float _Metallic2;
			float _TriTiling;
			float _TriUVRotation;
			float _WorldtoObjectSwitch;
			float _CoverageAmount;
			float _CoverageFalloff;
			float _Metallic1;
			float _Metallic0;
			float _Smoothness2;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			float4 _MaskMapRemapOffset2;
			float4 _MaskMapRemapScale2;
			float4 _MaskMapRemapScale0;
			float4 _MaskMapRemapOffset3;
			float4 _MaskMapRemapOffset1;
			float4 _MaskMapRemapOffset0;
			float4 _MaskMapRemapScale3;
			float4 _MaskMapRemapScale1;
			TEXTURE2D(_Mask1);
			SAMPLER(sampler_Mask1);
			TEXTURE2D(_Mask2);
			SAMPLER(sampler_Mask2);
			TEXTURE2D(_Mask3);
			SAMPLER(sampler_Mask3);
			TEXTURE2D(_Mask0);
			SAMPLER(sampler_Mask0);
			TEXTURE2D(_Control);
			SAMPLER(sampler_Control);
			#ifdef UNITY_INSTANCING_ENABLED//ASE Terrain Instancing
				TEXTURE2D(_TerrainHeightmapTexture);//ASE Terrain Instancing
				TEXTURE2D( _TerrainNormalmapTexture);//ASE Terrain Instancing
				SAMPLER(sampler_TerrainNormalmapTexture);//ASE Terrain Instancing
			#endif//ASE Terrain Instancing
			UNITY_INSTANCING_BUFFER_START( Terrain )//ASE Terrain Instancing
				UNITY_DEFINE_INSTANCED_PROP( float4, _TerrainPatchInstanceData )//ASE Terrain Instancing
			UNITY_INSTANCING_BUFFER_END( Terrain)//ASE Terrain Instancing
			CBUFFER_START( UnityTerrain)//ASE Terrain Instancing
				#ifdef UNITY_INSTANCING_ENABLED//ASE Terrain Instancing
					float4 _TerrainHeightmapRecipSize;//ASE Terrain Instancing
					float4 _TerrainHeightmapScale;//ASE Terrain Instancing
				#endif//ASE Terrain Instancing
			CBUFFER_END//ASE Terrain Instancing


			VertexInput ApplyMeshModification( VertexInput v )
			{
			#ifdef UNITY_INSTANCING_ENABLED
				float2 patchVertex = v.vertex.xy;
				float4 instanceData = UNITY_ACCESS_INSTANCED_PROP( Terrain, _TerrainPatchInstanceData );
				float2 sampleCoords = ( patchVertex.xy + instanceData.xy ) * instanceData.z;
				float height = UnpackHeightmap( _TerrainHeightmapTexture.Load( int3( sampleCoords, 0 ) ) );
				v.vertex.xz = sampleCoords* _TerrainHeightmapScale.xz;
				v.vertex.y = height* _TerrainHeightmapScale.y;
				#ifdef ENABLE_TERRAIN_PERPIXEL_NORMAL
					v.ase_normal = float3(0, 1, 0);
				#else
					v.ase_normal = _TerrainNormalmapTexture.Load(int3(sampleCoords, 0)).rgb* 2 - 1;
				#endif
				v.ase_texcoord.xy = sampleCoords* _TerrainHeightmapRecipSize.zw;
			#endif
				return v;
			}
			

        
			float4 _SelectionID;

        
			struct SurfaceDescription
			{
				float Alpha;
				float AlphaClipThreshold;
			};
        
			VertexOutput VertexFunction(VertexInput v  )
			{
				VertexOutput o;
				ZERO_INITIALIZE(VertexOutput, o);

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);


				v = ApplyMeshModification(v);
				float3 localCalculateTangentsSRP76_g11 = ( ( v.ase_tangent.xyz * v.ase_normal * 0.0 ) );
				{
				v.ase_tangent.xyz = cross ( v.ase_normal, float3( 0, 0, 1 ) );
				v.ase_tangent.w = -1;
				}
				float3 TangetsAlpha72 = localCalculateTangentsSRP76_g11;
				
				o.ase_texcoord.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = TangetsAlpha72;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				o.clipPos = TransformWorldToHClip(positionWS);
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_tangent = v.ase_tangent;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_tangent = patch[0].ase_tangent * bary.x + patch[1].ase_tangent * bary.y + patch[2].ase_tangent * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN ) : SV_TARGET
			{
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				float2 uv_Control = IN.ase_texcoord.xy * _Control_ST.xy + _Control_ST.zw;
				float4 tex2DNode5_g11 = SAMPLE_TEXTURE2D( _Control, sampler_Control, uv_Control );
				float dotResult20_g11 = dot( tex2DNode5_g11 , float4(1,1,1,1) );
				float SplatWeight22_g11 = dotResult20_g11;
				float AlphaLayer71 = SplatWeight22_g11;
				
				surfaceDescription.Alpha = AlphaLayer71;
				surfaceDescription.AlphaClipThreshold = 0.5;


				#if _ALPHATEST_ON
					float alphaClipThreshold = 0.01f;
					#if ALPHA_CLIP_THRESHOLD
						alphaClipThreshold = surfaceDescription.AlphaClipThreshold;
					#endif
					clip(surfaceDescription.Alpha - alphaClipThreshold);
				#endif

				half4 outColor = 0;
				outColor = _SelectionID;
				
				return outColor;
			}
        
			ENDHLSL
        }
		
	}
	
	CustomEditor "UnityEditor.ShaderGraphLitGUI"
	Fallback "Hidden/InternalErrorShader"
	
	Dependency "BaseMapShader"="ASESampleShaders/SRP Universal/TerrainBasePass"
	Dependency "AddPassShader"="ASESampleShaders/SRP Universal/TerrainAddPass"
	Dependency "BaseMapShader"="ASESampleShaders/SRP Universal/TerrainBasePass"
	Dependency "AddPassShader"="ASESampleShaders/SRP Universal/TerrainAddPass"

}
/*ASEBEGIN
Version=18935
289;73;1013;620;3907.059;-89.97272;1.529565;True;False
Node;AmplifyShaderEditor.FunctionNode;527;-3813.195,252.1583;Inherit;False;Four Splats First Pass Terrain;0;;11;37452fdfb732e1443b7e39720d05b708;2,85,0,102,1;7;59;FLOAT4;0,0,0,0;False;60;FLOAT4;0,0,0,0;False;61;FLOAT3;0,0,0;False;57;FLOAT;0;False;58;FLOAT;0;False;201;FLOAT;0;False;62;FLOAT;0;False;9;COLOR;294;FLOAT4;274;FLOAT4;0;FLOAT3;14;FLOAT;56;FLOAT;45;FLOAT;200;FLOAT;19;FLOAT3;17
Node;AmplifyShaderEditor.CommentaryNode;327;-5270.317,-2123.534;Inherit;False;317.8;243.84;Coverage in World mode;1;453;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;318;-5372.119,-1760.902;Inherit;False;436.2993;336.8007;Coverage in Object mode;2;329;326;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;72;-3417.276,570.2044;Inherit;False;TangetsAlpha;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;71;-3415.276,486.2044;Inherit;False;AlphaLayer;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;330;-4783.41,-2358.029;Inherit;False;224;239;Coverage in World mode;1;343;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;328;-4783.41,-2086.029;Inherit;False;235.9301;237.3099;Coverage in Object mode;1;340;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;446;-4817.046,-5149.473;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;273;-6847.41,-2630.029;Inherit;False;2;2;0;FLOAT4x4;0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;449;-5057.046,-4749.472;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WireNode;442;-4801.046,-4797.472;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;83;-1379.17,-1060.64;Inherit;False;67;AlbedoLayer;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;418;-3394.138,-2203.609;Inherit;False;TriplanarTopUV;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;485;-5363.015,-4793.73;Inherit;False;479;TriCom_2;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;482;-5291.677,-3758.928;Inherit;False;479;TriCom_2;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;361;-4351.41,-2198.029;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SamplerNode;431;-4577.702,-4554.694;Inherit;True;Property;_TopNormal;Top Normal;36;0;Create;True;0;0;0;False;0;False;-1;None;84956715d52a7f040a6a341dfd449801;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;501;-4788.27,-2496.479;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;503;-7036.064,-2850.907;Inherit;False;mTriOffset;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.BreakToComponentsNode;427;-3452.115,-3843.425;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.WorldNormalVector;272;-7119.41,-2598.029;Inherit;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.BreakToComponentsNode;322;-5887.41,-2790.029;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.GetLocalVarNode;488;-5354.587,-5842.598;Inherit;False;479;TriCom_2;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;85;-1309.934,-772.3953;Inherit;False;68;NormalLayer;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;419;-3929.713,-1866.194;Inherit;False;TriplanarColorWeight;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;320;-5503.41,-1862.029;Float;False;Property;_WorldtoObjectSwitch;World to Object Switch;37;1;[IntRange];Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;502;-7269.064,-2862.907;Inherit;False;Property;_TriOffset;TriOffset;28;0;Create;True;0;0;0;False;0;False;0,0;0.025,-0.265;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RegisterLocalVarNode;463;-3517.964,-5925.84;Inherit;False;TempTriAlbedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;76;-2414.424,328.1126;Inherit;False;SpecularLayer;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;68;-3125.047,344.2878;Inherit;False;NormalLayer;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;466;-4556.076,-5943.86;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;445;-4561.046,-4893.472;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.AbsOpNode;275;-6687.41,-2630.029;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldNormalVector;452;-5448.938,-1540.962;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;472;-5627.363,-5748.842;Inherit;False;417;TriplanarUV_3;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;425;-4763.115,-3392.425;Inherit;False;418;TriplanarTopUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;398;-3948.038,-2194.194;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;417;-4373.47,-2549.09;Inherit;False;TriplanarUV_3;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;450;-5057.046,-4989.472;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldToObjectMatrix;271;-7119.41,-2694.029;Inherit;False;0;1;FLOAT4x4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;428;-3219.115,-3710.425;Inherit;False;TriSmoothness;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;73;-536.7903,-261.3591;Inherit;False;71;AlphaLayer;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;370;-4127.41,-1814.029;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.GetLocalVarNode;387;-5475.105,-3143.154;Inherit;False;455;mTriTiling;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;469;-5421.858,-6037.298;Inherit;True;Property;_TextureSample6;Texture Sample 6;31;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;473;Auto;Texture2D;8;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;468;-5630.441,-6120.361;Inherit;False;416;TriplanarUV_2;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;481;-5302.082,-3439.129;Inherit;False;477;TriCom_3;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;66;-2991.268,142.0624;Inherit;False;FirstLayerControl;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;444;-5057.046,-5261.473;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;465;-5052.076,-6311.861;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerStateNode;421;-6420.078,-5448.896;Inherit;False;0;0;0;1;-1;X4;1;0;SAMPLER2D;;False;1;SAMPLERSTATE;0
Node;AmplifyShaderEditor.WireNode;336;-4765.317,-1392.234;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;483;-5268.747,-4015.56;Inherit;False;480;TriCom_1;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;81;-1036.42,-957.4458;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DotProductOpNode;276;-6513.51,-2563.631;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;365;-4203.775,-2190.198;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;411;-5318.667,-3665.394;Inherit;True;Property;_TextureSample3;Texture Sample 3;30;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;412;Auto;Texture2D;8;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;348;-4623.41,-1814.029;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BreakToComponentsNode;60;-3154.525,129.7808;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.PowerNode;367;-4303.41,-1814.029;Inherit;False;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;515;-2862.228,590.7416;Inherit;False;mLayerSmootness;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;528;-3186.289,702.2208;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;529;-3047.191,533.2206;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;92;-1516.876,-84.56341;Inherit;False;428;TriSmoothness;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;511;-1484.477,-168.6675;Inherit;False;515;mLayerSmootness;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;88;-1321.442,-447.849;Inherit;False;76;SpecularLayer;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerStateNode;476;-5946.252,-5806.787;Inherit;False;0;0;0;1;-1;X4;1;0;SAMPLER2D;;False;1;SAMPLERSTATE;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;329;-5094.82,-1640.801;Inherit;False;2;2;0;FLOAT4x4;0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;504;-5043.535,-3192.052;Inherit;False;503;mTriOffset;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WireNode;355;-4405.618,-1308.033;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;420;-1421.35,-682.0063;Inherit;False;451;TriNormal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;432;-5635.411,-5069.973;Inherit;False;416;TriplanarUV_2;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WorldToObjectMatrix;326;-5337.42,-1662.991;Inherit;False;0;1;FLOAT4x4;0
Node;AmplifyShaderEditor.WireNode;459;-4796.076,-5847.86;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PosVertexDataNode;340;-4751.41,-2022.029;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;434;-4869.577,-4436.004;Inherit;False;418;TriplanarTopUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;87;-1024.692,-347.6548;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;441;-5427.828,-4986.91;Inherit;True;Property;_TextureSample4;Texture Sample 4;33;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Instance;440;Auto;Texture2D;8;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;487;-5378.587,-5564.598;Inherit;False;477;TriCom_3;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;337;-5887.41,-2646.029;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;389;-5277.958,-2494.899;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;484;-5375.88,-4506.215;Inherit;False;477;TriCom_3;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;499;-4813.27,-3030.479;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;467;-5631.983,-6326.931;Inherit;False;415;TriplanarUV_1;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;435;-4153.017,-4544.293;Inherit;False;419;TriplanarColorWeight;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;500;-4807.27,-2780.479;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;451;-3535.933,-4835.695;Inherit;False;TriNormal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;360;-4463.41,-1814.029;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;454;-1489.766,-984.3516;Inherit;False;463;TempTriAlbedo;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;486;-5379.587,-5066.598;Inherit;False;480;TriCom_1;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;443;-5424.044,-4708.973;Inherit;True;Property;_TextureSample5;Texture Sample 5;33;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Instance;440;Auto;Texture2D;8;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;506;-3903.497,-2287.48;Inherit;False;503;mTriOffset;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;351;-5443.26,-2471.732;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;460;-5052.076,-5799.86;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;426;-3935.115,-3609.425;Inherit;False;419;TriplanarColorWeight;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;471;-5419.074,-5759.361;Inherit;True;Property;_TextureSample7;Texture Sample 7;31;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;473;Auto;Texture2D;8;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;512;-2533.393,164.5378;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;456;-4572.732,-5605.082;Inherit;True;Property;_TempTopAlbedo;Temp Top Albedo;34;0;Create;True;0;0;0;False;0;False;-1;None;8f81199c7a78242fa94d2b11796326a2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;342;-4831.41,-1814.029;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;397;-4248.18,-2290.243;Inherit;False;455;mTriTiling;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;493;-7050.042,-2998.968;Inherit;False;mTriUVRotation;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;89;-1359.442,-272.8494;Inherit;False;66;FirstLayerControl;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;339;-4829.619,-2105.134;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;412;-5316.369,-4198.793;Inherit;True;Property;_TriplanarMask;Triplanar Mask;30;0;Create;True;0;0;0;False;0;False;-1;None;3d2140ec449eb0049bc3eb51c3a7cf7b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;91;-1325.633,6.070984;Inherit;False;66;FirstLayerControl;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;99;-2712.798,469.7848;Inherit;False;Constant;_Float10;Float 10;12;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;510;-4641.897,-2503.213;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;414;-4472.325,-3511.115;Inherit;True;Property;_TopMask;Top Mask;35;0;Create;True;0;0;0;False;0;False;-1;None;592277751c386d34fa5609a321b5618e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;84;-957.3374,-731.3334;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;474;-4263.975,-5798.004;Inherit;False;419;TriplanarColorWeight;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;74;-562.7903,-174.3591;Inherit;False;72;TangetsAlpha;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;277;-6351.41,-2630.029;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;433;-5632.333,-4698.454;Inherit;False;417;TriplanarUV_3;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;461;-3868.075,-5943.86;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;505;-3566.497,-2189.48;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;413;-3767.669,-3849.893;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;479;-5673.971,-2600.407;Inherit;False;TriCom_2;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;353;-4511.41,-2198.029;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;386;-5257.092,-3050.797;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;407;-4951.669,-4217.894;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;423;-5530.034,-4026.394;Inherit;False;416;TriplanarUV_2;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WorldPosInputsNode;343;-4751.41,-2294.029;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;405;-4711.669,-4105.894;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;477;-5666.471,-2324.271;Inherit;False;TriCom_3;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;321;-5167.718,-1864.934;Float;False;WorldObjectSwitch;4;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;410;-5321.451,-3943.331;Inherit;True;Property;_TextureSample1;Texture Sample 1;30;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;412;Auto;Texture2D;8;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;429;-3225.558,-3966.873;Inherit;False;TriSpecular;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RotatorNode;491;-5045.834,-3042.787;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;384;-7277.555,-3139.218;Inherit;False;Property;_TriTiling;TriTiling;32;0;Create;True;0;0;0;False;0;False;0.1;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;406;-4455.669,-3849.893;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;67;-2361.376,213.6006;Inherit;False;AlbedoLayer;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RotatorNode;496;-5036.146,-2510.159;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PosVertexDataNode;338;-5689.195,-2489.364;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;349;-4496.418,-1212.833;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;489;-5341.587,-6108.598;Inherit;False;480;TriCom_1;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;400;-4951.669,-3705.893;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;404;-4951.669,-3945.893;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;278;-6191.41,-2630.029;Float;True;BlendComponents;1;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;280;-5186.817,-1357.031;Float;False;Property;_CoverageAmount;Coverage Amount;38;0;Create;True;0;0;0;False;0;False;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;494;-5266.146,-3200.159;Inherit;False;493;mTriUVRotation;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;453;-5233.634,-2064.49;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DynamicAppendNode;354;-5444.16,-2746.333;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;455;-7048.849,-3123.76;Inherit;False;mTriTiling;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;475;-4784.855,-5555.958;Inherit;False;418;TriplanarTopUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;388;-5263.092,-2773.663;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;458;-5052.076,-6039.86;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector3Node;274;-6720.528,-2449.579;Float;False;Constant;_Vector6;Vector 6;-1;0;Create;True;0;0;0;False;0;False;1,1,1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;86;-1347.934,-597.3957;Inherit;False;66;FirstLayerControl;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;90;-1032.883,-119.7344;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;430;-1545.105,-380.7673;Inherit;False;429;TriSpecular;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;332;-4877.39,-2065.769;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;323;-5887.41,-2502.029;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.GetLocalVarNode;422;-5531.576,-4232.964;Inherit;False;415;TriplanarUV_1;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;448;-3873.046,-4893.472;Inherit;True;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;508;-4668.01,-3030.764;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;462;-4812.076,-6199.861;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;416;-4440.4,-2798.689;Inherit;False;TriplanarUV_2;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WireNode;408;-4695.669,-3753.893;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;509;-4642.937,-2761.242;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;424;-5526.956,-3654.875;Inherit;False;417;TriplanarUV_3;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;82;-1371.17,-882.6404;Inherit;False;66;FirstLayerControl;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;335;-5689.195,-3033.364;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;439;-5636.953,-5276.542;Inherit;False;415;TriplanarUV_1;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;69;-3125.798,418.2878;Inherit;False;MetallicnessLayer;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;352;-5437.16,-3002.333;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;282;-5182.917,-1236.732;Float;False;Property;_CoverageFalloff;Coverage Falloff;39;0;Create;True;0;0;0;False;0;False;0.5;2;0.01;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;415;-4436.136,-3023.413;Inherit;False;TriplanarUV_1;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;440;-5421.746,-5242.372;Inherit;True;Property;_TriplanarNormal;Triplanar Normal;33;0;Create;True;0;0;0;False;0;False;-1;None;527b74b713b11c54b8b3ee728b61cd6f;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RotatorNode;495;-5057.146,-2776.159;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;480;-5704.814,-2871.051;Inherit;False;TriCom_1;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;473;-5416.776,-6292.76;Inherit;True;Property;_TempTriplanarAlbedo;Temp Triplanar Albedo;31;0;Create;True;0;0;0;False;0;False;-1;None;afbd2772cfa009b46bbd9838dd988440;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;492;-7318.042,-3004.968;Inherit;False;Property;_TriUVRotation;TriUVRotation;29;0;Create;True;0;0;0;False;0;False;0;1.55;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;345;-5689.195,-2777.364;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;22;-784,-448;Float;False;False;-1;2;UnityEditor.ShaderGraphLitGUI;0;2;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;ShadowCaster;0;2;ShadowCaster;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;True;False;False;False;False;0;False;-1;False;False;False;False;False;False;False;False;False;True;1;False;-1;True;3;False;-1;False;True;1;LightMode=ShadowCaster;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;21;-271,-485;Float;False;True;-1;2;UnityEditor.ShaderGraphLitGUI;0;2;Universal Render Pipeline/Terrain/ProceduralTerrainFirstPass;94348b07e5e8bab40bd6c8a1e3df54cd;True;Forward;0;1;Forward;19;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=-100;True;2;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;True;1;1;False;-1;0;False;-1;1;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;1;LightMode=UniversalForward;False;False;0;Hidden/InternalErrorShader;4;BaseMapShader=ASESampleShaders/SRP Universal/TerrainBasePass;AddPassShader=ASESampleShaders/SRP Universal/TerrainAddPass;BaseMapShader=ASESampleShaders/SRP Universal/TerrainBasePass;AddPassShader=ASESampleShaders/SRP Universal/TerrainAddPass;0;Standard;40;Workflow;0;638650188499305010;Surface;0;0;  Refraction Model;0;0;  Blend;0;0;Two Sided;1;0;Fragment Normal Space,InvertActionOnDeselection;0;0;Transmission;0;0;  Transmission Shadow;0.5,False,-1;0;Translucency;0;0;  Translucency Strength;1,False,-1;0;  Normal Distortion;0.5,False,-1;0;  Scattering;2,False,-1;0;  Direct;0.9,False,-1;0;  Ambient;0.1,False,-1;0;  Shadow;0.5,False,-1;0;Cast Shadows;1;0;  Use Shadow Threshold;0;0;Receive Shadows;1;0;GPU Instancing;1;0;LOD CrossFade;1;0;Built-in Fog;1;0;_FinalColorxAlpha;1;0;Meta Pass;1;0;Override Baked GI;0;0;Extra Pre Pass;0;0;DOTS Instancing;0;0;Tessellation;0;0;  Phong;0;0;  Strength;0.5,False,-1;0;  Type;0;0;  Tess;16,False,-1;0;  Min;10,False,-1;0;  Max;25,False,-1;0;  Edge Length;16,False,-1;0;  Max Displacement;25,False,-1;0;Write Depth;0;0;  Early Z;0;0;Vertex Position,InvertActionOnDeselection;1;0;Debug Display;0;0;Clear Coat;0;0;0;10;False;True;True;True;True;True;True;True;True;True;True;;True;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;29;-784,-388;Float;False;False;-1;2;UnityEditor.ShaderGraphLitGUI;0;2;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;GBuffer;0;7;GBuffer;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;True;1;1;False;-1;0;False;-1;1;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;1;LightMode=UniversalGBuffer;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;23;-784,-448;Float;False;False;-1;2;UnityEditor.ShaderGraphLitGUI;0;2;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;DepthOnly;0;3;DepthOnly;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;True;False;False;False;False;0;False;-1;False;False;False;False;False;False;False;False;False;True;1;False;-1;False;False;True;1;LightMode=DepthOnly;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;24;-784,-448;Float;False;False;-1;2;UnityEditor.ShaderGraphLitGUI;0;2;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;Meta;0;4;Meta;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Meta;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;26;-784,-448;Float;False;False;-1;2;UnityEditor.ShaderGraphLitGUI;0;2;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;ExtraPrePass;0;0;ExtraPrePass;5;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;True;1;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;0;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;31;-784,-388;Float;False;False;-1;2;UnityEditor.ShaderGraphLitGUI;0;2;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;ScenePickingPass;0;9;ScenePickingPass;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Picking;False;True;4;d3d11;glcore;gles;gles3;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;30;-784,-388;Float;False;False;-1;2;UnityEditor.ShaderGraphLitGUI;0;2;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;SceneSelectionPass;0;8;SceneSelectionPass;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=SceneSelectionPass;False;True;4;d3d11;glcore;gles;gles3;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;28;-784,-388;Float;False;False;-1;2;UnityEditor.ShaderGraphLitGUI;0;2;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;DepthNormals;0;6;DepthNormals;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;True;1;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;False;-1;True;3;False;-1;False;True;1;LightMode=DepthNormals;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;25;-784,-448;Float;False;False;-1;2;UnityEditor.ShaderGraphLitGUI;0;2;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;Universal2D;0;5;Universal2D;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;True;1;1;False;-1;0;False;-1;1;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Universal2D;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
WireConnection;72;0;527;17
WireConnection;71;0;527;19
WireConnection;446;0;444;0
WireConnection;446;1;450;0
WireConnection;273;0;271;0
WireConnection;273;1;272;0
WireConnection;449;0;443;0
WireConnection;449;1;484;0
WireConnection;442;0;449;0
WireConnection;418;0;505;0
WireConnection;361;0;353;0
WireConnection;431;1;434;0
WireConnection;501;0;496;0
WireConnection;501;1;504;0
WireConnection;503;0;502;0
WireConnection;427;0;413;0
WireConnection;322;0;278;0
WireConnection;419;0;370;1
WireConnection;463;0;461;0
WireConnection;76;0;99;0
WireConnection;68;0;527;14
WireConnection;466;0;462;0
WireConnection;466;1;459;0
WireConnection;445;0;446;0
WireConnection;445;1;442;0
WireConnection;275;0;273;0
WireConnection;398;0;397;0
WireConnection;398;1;365;0
WireConnection;417;0;510;0
WireConnection;450;0;441;0
WireConnection;450;1;485;0
WireConnection;428;0;427;3
WireConnection;370;0;367;0
WireConnection;469;1;468;0
WireConnection;469;7;476;0
WireConnection;66;0;60;0
WireConnection;444;0;440;0
WireConnection;444;1;486;0
WireConnection;465;0;473;0
WireConnection;465;1;489;0
WireConnection;336;0;280;0
WireConnection;81;0;83;0
WireConnection;81;1;454;0
WireConnection;81;2;82;0
WireConnection;276;0;275;0
WireConnection;276;1;274;0
WireConnection;365;0;361;0
WireConnection;365;1;361;2
WireConnection;411;1;424;0
WireConnection;411;7;421;0
WireConnection;348;0;342;0
WireConnection;348;1;336;0
WireConnection;60;0;527;274
WireConnection;367;0;360;0
WireConnection;367;1;355;0
WireConnection;515;0;529;0
WireConnection;528;0;527;294
WireConnection;529;0;527;45
WireConnection;529;1;528;0
WireConnection;329;0;326;0
WireConnection;329;1;452;0
WireConnection;355;0;349;0
WireConnection;459;0;460;0
WireConnection;87;0;88;0
WireConnection;87;1;430;0
WireConnection;87;2;89;0
WireConnection;441;1;432;0
WireConnection;441;7;421;0
WireConnection;337;0;278;0
WireConnection;389;0;387;0
WireConnection;389;1;351;0
WireConnection;499;0;491;0
WireConnection;499;1;504;0
WireConnection;500;0;495;0
WireConnection;500;1;504;0
WireConnection;451;0;448;0
WireConnection;360;0;348;0
WireConnection;443;1;433;0
WireConnection;443;7;421;0
WireConnection;351;0;338;1
WireConnection;351;1;338;2
WireConnection;460;0;471;0
WireConnection;460;1;487;0
WireConnection;471;1;472;0
WireConnection;471;7;476;0
WireConnection;512;0;527;56
WireConnection;512;1;527;0
WireConnection;456;1;475;0
WireConnection;342;0;453;0
WireConnection;342;1;329;0
WireConnection;342;2;321;0
WireConnection;493;0;492;0
WireConnection;339;0;332;0
WireConnection;412;1;422;0
WireConnection;412;7;421;0
WireConnection;510;0;501;0
WireConnection;414;1;425;0
WireConnection;414;7;421;0
WireConnection;84;0;85;0
WireConnection;84;1;420;0
WireConnection;84;2;86;0
WireConnection;277;0;275;0
WireConnection;277;1;276;0
WireConnection;461;0;466;0
WireConnection;461;1;456;0
WireConnection;461;2;474;0
WireConnection;505;0;398;0
WireConnection;505;1;506;0
WireConnection;413;0;406;0
WireConnection;413;1;414;0
WireConnection;413;2;426;0
WireConnection;479;0;337;1
WireConnection;353;0;343;0
WireConnection;353;1;340;0
WireConnection;353;2;339;0
WireConnection;386;0;387;0
WireConnection;386;1;352;0
WireConnection;407;0;412;0
WireConnection;407;1;483;0
WireConnection;405;0;407;0
WireConnection;405;1;404;0
WireConnection;477;0;323;2
WireConnection;321;0;320;0
WireConnection;410;1;423;0
WireConnection;410;7;421;0
WireConnection;429;0;413;0
WireConnection;491;0;386;0
WireConnection;491;2;494;0
WireConnection;406;0;405;0
WireConnection;406;1;408;0
WireConnection;67;0;512;0
WireConnection;496;0;389;0
WireConnection;496;2;494;0
WireConnection;349;0;282;0
WireConnection;400;0;411;0
WireConnection;400;1;481;0
WireConnection;404;0;410;0
WireConnection;404;1;482;0
WireConnection;278;0;277;0
WireConnection;354;0;345;1
WireConnection;354;1;345;3
WireConnection;455;0;384;0
WireConnection;388;0;387;0
WireConnection;388;1;354;0
WireConnection;458;0;469;0
WireConnection;458;1;488;0
WireConnection;90;0;511;0
WireConnection;90;1;92;0
WireConnection;90;2;91;0
WireConnection;332;0;321;0
WireConnection;323;0;278;0
WireConnection;448;0;445;0
WireConnection;448;1;431;0
WireConnection;448;2;435;0
WireConnection;508;0;499;0
WireConnection;462;0;465;0
WireConnection;462;1;458;0
WireConnection;416;0;509;0
WireConnection;408;0;400;0
WireConnection;509;0;500;0
WireConnection;69;0;527;56
WireConnection;352;0;335;2
WireConnection;352;1;335;3
WireConnection;415;0;508;0
WireConnection;440;1;439;0
WireConnection;440;7;421;0
WireConnection;495;0;388;0
WireConnection;495;2;494;0
WireConnection;480;0;322;0
WireConnection;473;1;467;0
WireConnection;473;7;476;0
WireConnection;21;0;81;0
WireConnection;21;1;84;0
WireConnection;21;9;87;0
WireConnection;21;4;90;0
WireConnection;21;6;73;0
WireConnection;21;8;74;0
ASEEND*/
//CHKSM=E8165B46A0FCC03347E7B7DBBE7699F6536772FD