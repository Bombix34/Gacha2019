using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxManager : MonoBehaviour
{

    public float SkyboxSpeed = 1f;

    void Update()
    {
        GetComponent<Skybox>().material.SetFloat("_Rotation", Time.time* SkyboxSpeed);
    }

}
