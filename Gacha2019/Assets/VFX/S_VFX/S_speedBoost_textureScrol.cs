using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_speedBoost_textureScrol : MonoBehaviour
{

    float offset = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        offset = offset + 0.01f ; 
        GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(offset, offset/2));
    }
}
