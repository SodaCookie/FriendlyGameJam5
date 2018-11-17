Shader "Custom/MinimapTexture" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_BackColor ("Background", Color) = (0,0,0,1)
		_FireColor ("Fire Color", Color) = (1,0.5,0,1)
		_MainTexture ("Main Texture", 2D) = "zeroRGBA" {}
		_FireTexture ("Fire Texture", 2D) = "zeroRGBA" {}
	}
	SubShader {

		Pass {
			Tags { 
				"RenderType"="Opaque" 
			}
			LOD 200

			CGPROGRAM
			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0
			#pragma vertex Vertex
			#pragma fragment Fragment

			float random (float2 st) {
				return frac(sin(dot(st.xy,
                    float2(12.9898,78.233))) * 43758.5453123);
			}

			float noise (float2 st) {
				float2 i = floor(st);
				float2 f = frac(st);

				// Four corners in 2D of a tile
				float a = random(i);
				float b = random(i + float2(1.0, 0.0));
				float c = random(i + float2(0.0, 1.0));
				float d = random(i + float2(1.0, 1.0));

				// Smooth Interpolation

				// Cubic Hermine Curve.  Same as SmoothStep()
				float2 u = f*f*(3.0-2.0*f);
				// u = smoothstep(0.,1.,f);

				// Mix 4 coorners percentages
				return lerp(a, b, u.x) +
						(c - a)* u.y * (1.0 - u.x) +
						(d - b) * u.x * u.y;
			}

			struct Attributes {
				float4 position : POSITION;
				float2 uv : TEXCOORD0; 
			};

			struct Varyings {
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0; 
			};

			sampler2D _MainTexture, _FireTexture;
			float4 _Color, _BackColor, _FireColor;

			Varyings Vertex(Attributes a) {
				Varyings o;
				o.position = UnityObjectToClipPos(a.position);
				o.uv = a.uv;
				return o;
			}

			float4 Fragment(Varyings v) : SV_Target0 {
				float4 background = _Color * tex2D(_MainTexture, v.uv.xy).a + _BackColor * (1 - tex2D(_MainTexture, v.uv.xy).a);
				background = _FireColor * tex2D(_FireTexture, v.uv.xy).a + background * (1 - tex2D(_FireTexture, v.uv.xy).a);
				return background * random(v.uv + _Time.x);
			}
		
			ENDCG
		}
		
	}
	FallBack "Diffuse"
}
