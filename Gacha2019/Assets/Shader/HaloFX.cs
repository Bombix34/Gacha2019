using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class HaloFX : MonoBehaviour {
	
	public Shader FXShader;
    public float radius;

    private Material FXMaterial;

	void CreateMaterials() 
	{
		if (FXMaterial == null)
		{
			FXMaterial = new Material (FXShader);
			FXMaterial.hideFlags = HideFlags.HideAndDontSave;
		}	
	}

	void OnRenderImage(RenderTexture source,RenderTexture destination) //Fonction appelée par unity à chaque fin de rendu. C'est maintenant qu'on fait le post-effet
	{
		CreateMaterials();
		FXMaterial.SetVector("camPos", GetComponent<Camera>().transform.position);
		FXMaterial.SetVector("camUp", GetComponent<Camera>().transform.up);
		FXMaterial.SetVector("camForward", GetComponent<Camera>().transform.forward);
		FXMaterial.SetVector("camRight", GetComponent<Camera>().transform.right);
		FXMaterial.SetFloat("camAngle", GetComponent<Camera>().fieldOfView * 3.1415926535f/180.0f);
        FXMaterial.SetFloat("radius", radius);

        Graphics.Blit(source, destination, FXMaterial);
	}
}

