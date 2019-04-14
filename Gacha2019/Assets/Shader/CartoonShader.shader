//http://docs.unity3d.com/Manual/SL-SurfaceShaders.html
//http://docs.unity3d.com/Manual/SL-SurfaceShaderExamples.html
//http://docs.unity3d.com/Manual/SL-SurfaceShaderLightingExamples.html
//http://docs.unity3d.com/Manual/SL-VertexProgramInputs.html


Shader "Custom/CartoonShader"
{
	Properties
	{
		_Color ("Color", Color) = (1, 1, 1, 1)
		_MainTex ("Diffuse", 2D) = "white" {}
		_AmbientOcclusion ("Ambient Occlusion", 2D) = "white" {}
		_NormalMap ("Normal map", 2D) = "bump" {} //couleur "bump" par défaut
		//_SpecularMap ("Specular map", 2D) = "white" {}
		//_GlossMap ("Gloss map", 2D) = "white" {}
		//
		//_SpecularFactor ("Specular Factor", float) = 500
		//_GlossFactor ("Gloss Factor", float) = 10
	}

	Category
	{
		SubShader
		{
			CGPROGRAM
			//ici, on veut utiliser le modèle d'éclairage SimpleSpecular associé à la "fonction LightingSimpleSpecular", 
			//la fonction "surf" en fonction de surface et la fonction "vertexFunction" en vertex shader
			#pragma surface surf Cartoon 
			#pragma target 3.0

			float4 _Color;

			sampler2D _MainTex;
			sampler2D _AmbientOcclusion;
			sampler2D _NormalMap;
			//sampler2D _SpecularMap;
			//sampler2D _GlossMap;
			//
			//float _SpecularFactor;
			//float _GlossFactor;
			
			
			//Structure d'entrée/sortie du surface shader.
			struct Input
			{
				float3 viewDir; //will contain view direction, for computing Parallax effects, rim lighting etc.
				float2 uv_MainTex: TEXCOORD0; //Premier niveau d'UV. Doit etre de la forme uv_<nom d'une property existante>. Incompréhensible.
				float2 uv2_MainTex: TEXCOORD1;//Second niveau d'UV (généralement utilisé pour la lightmap). Doit etre de la forme uv2_<nom d'une property>
				float3 worldPos; //Position dans le monde
				float3 worldRefl; //Vecteur de reflection dans le monde
				float3 worldNormal; //Normale dans le monde. INTERNAL_DATA permet de modifier ce paramètre dans la fonction surf.
				float4 screenPos;
				float depth;
				INTERNAL_DATA		//Obligatoire pour utiliser une normal map. Ne pas oublier d'écrire une valeur par défaut sinon!
			};
			
			//Fonction principale du surface shader
			//Il faut remplir les paramètres albedo, normal, specular, gloss, emission
			void surf (Input i, inout SurfaceOutput o) 
			{
				o.Albedo = tex2D(_MainTex, i.uv_MainTex.xy)
				* tex2D(_AmbientOcclusion, i.uv_MainTex.xy)
				* _Color;
				
				o.Normal = UnpackNormal(tex2D(_NormalMap, i.uv_MainTex.xy));
				
				//o.Specular = tex2D(_SpecularMap, i.uv_MainTex.xy) * _SpecularFactor;
				//o.Gloss = tex2D(_GlossMap, i.uv_MainTex.xy) * _GlossFactor;
			}
			
			//Fonction d'éclairage custom, qui sera lancée pour chaque lumière et gère le spéculaire.
			half4 LightingCartoon (SurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
			{
				//half3 h = normalize (normalize(lightDir) + viewDir);
				//half diff = max (0, dot (s.Normal, lightDir));
				//float nh = max (0, dot (s.Normal, h));
				//float spec = s.Gloss * pow (nh, s.Specular);

				//c.rgb = (s.Albedo * _LightColor0.rgb * diff +  _LightColor0.rgb*spec) * (atten * 2);

				half4 color;

				float light = dot(s.Normal, normalize(lightDir)) > 0.0f ? 1.0f : 0.0f;
				float lightCast = atten > 0.1f ? 1.0f : 0.0f;

				color.rgb = s.Albedo * _LightColor0.rgb * light * lightCast;
				color.a = s.Alpha;
		
				return color;
			}
			
			ENDCG
		}

		Fallback "VertexLit"
	}
}