// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/HaloFX"
{
	Properties
	{
		_MainTex ("", 2D) = "" {}
	}
	SubShader
	{
		Pass
		{
			ZTest Always Cull Off ZWrite Off Fog { Mode off } //Parametrage du shader pour éviter de lire, écrire dans le zbuffer, désactiver le culling et le brouillard sur le polygone

			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma fragment frag
			
			float3 camPos;
			float3 camForward;
			float3 camUp;
			float3 camRight;
			float camAngle;
			float radius;

			sampler2D _MainTex;

			#define MAX_LOOP 128
			
			uniform sampler2D _CameraDepthTexture;

			
			struct Prog2Vertex
			{
				float4 vertex : POSITION; 	//Les "registres" précisés après chaque variable servent
				float4 texcoord : TEXCOORD0;
			};
				
			//Structure servant a transporter des données du vertex shader au pixel shader.
			//C'est au vertex shader de remplir a la main les informations de cette structure.
			struct Vertex2Pixel
			{
				float4 pos : SV_POSITION;
				float4 uv : TEXCOORD0;

			};  	 

			Vertex2Pixel vert (Prog2Vertex i)
			{
				Vertex2Pixel o;
				o.pos = UnityObjectToClipPos (i.vertex); //Projection du modèle 3D, cette ligne est obligatoire
				o.uv=i.texcoord; //UV de la texture
				
				return o;
			}

			
			unsigned int hasard;

			float rand_lcg() //Renvoie une valeur entre 0 et 1
			{
				// LCG values from Numerical Recipes
				hasard = 1664525 * hasard + 1013904223;
				return hasard / 4294967296.0f;
			}

			
			float4 frag(Vertex2Pixel i) : COLOR 
			{
				hasard=pow( pow(i.uv.x,i.uv.y) , pow(i.uv.y,i.uv.x) )*0xFFFFFFFF;
				
				float ratio = _ScreenParams.x/_ScreenParams.y;
				
				float factorX = -1+2* i.uv.x;
				float factorY = -1+2* i.uv.y;
				
				float Profondeur = LinearEyeDepth(tex2D(_CameraDepthTexture, i.uv.xy).r);
		
				float3 direction = normalize(camForward + camRight*factorX*ratio*tan(camAngle/2) + camUp*factorY*tan(camAngle/2));
				Profondeur/=dot(direction,camForward);

				float3 cpoint= camPos+rand_lcg()*50*direction/MAX_LOOP;
				float energie=0;
				float sqrLenght;
				
				for (int k=0; k<MAX_LOOP ;k++)
				{
					cpoint+= rand_lcg()*50*direction/MAX_LOOP;

					sqrLenght = cpoint.x * cpoint.x + cpoint.y * cpoint.y + cpoint.z * cpoint.z;
					
					if (sqrLenght <= radius * radius)
					{
						energie += 1.0f/MAX_LOOP;
					}
					
					if (distance (cpoint,camPos) > Profondeur) break;
				}
				
				//return Profondeur.xxxx*0.1;
				
				return tex2D(_MainTex, i.uv.xy) + float4(energie,energie,energie,1);
				// return float4(energie,energie,energie,1);
				//return rand_lcg().xxxx;
			}

			ENDCG 
		}
	}

	Fallback off
}