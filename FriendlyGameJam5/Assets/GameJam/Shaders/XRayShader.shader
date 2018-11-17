Shader "Custom/X Ray Wave Shader" {

    Properties {
        _Tint ("Color", Color) = (0, 1, 0, 1)
    }

    SubShader {

		Tags { 
			"LightMode" = "ForwardBase"
			"Queue"="Transparent" 
			"RenderType"="Transparent" 
		}

		ZTest Greater

        Pass {
			Blend One OneMinusSrcAlpha

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityStandardBRDF.cginc"

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

            float4 _Tint;

            struct VertexData {
                float4 position : POSITION;
                float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
            };

            struct Interpolator {
                float4 position : SV_POSITION;
				float2 uv : TEXCOORD0; 
                float3 normal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
            };

            Interpolator vert(VertexData data) {
                Interpolator i;
                i.position = UnityObjectToClipPos(data.position);
                i.normal = UnityObjectToWorldNormal(data.normal);
                i.worldPos = mul(unity_ObjectToWorld, data.position);
				i.uv = data.uv;
                return i;
            }

            float4 frag(Interpolator data) : SV_Target0 {
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - data.worldPos);
				float intensity = 1 - DotClamped(viewDirection, data.normal);
				float distance = length(data.worldPos - _WorldSpaceCameraPos.xyz);
				float t = smoothstep(0.8, 1, saturate(sin(distance * 10 - _Time.w * 2)));
				intensity += t;
				intensity += random(data.uv * _Time.x) * smoothstep(0.95, 0.99, random(data.uv * _Time.x));
				//intensity = pow(intensity, 2);

				float color_t = smoothstep(0.5, 0.7, noise(intensity * 10 * (data.position.x / 100 + _Time.w)));
				float4 mixed_color = lerp(_Tint, float4(1, 1, 1, 1), color_t);

                return float4(mixed_color.rgb * intensity, intensity);
            }

            ENDCG
        }
    }
}