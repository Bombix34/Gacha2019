using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsSphereBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, -0.75f, Space.World);
    }

    void OnEnable()
    {
        transform.eulerAngles = new Vector3(90f, 180f, 0f);
    }
}
