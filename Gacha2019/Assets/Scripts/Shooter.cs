using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider _Other)
    {
        Player player = _Other.GetComponent<Player>();
        if (player != null)
        {
            Debug.Log("Yeah !");
        }
    }
}
