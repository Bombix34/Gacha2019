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
        transform.Rotate(transform.up, 1f, Space.World);
    }

    private void OnEnable()
    {
        transform.eulerAngles = new Vector3(0f, -75f, 0f);
    }
}
