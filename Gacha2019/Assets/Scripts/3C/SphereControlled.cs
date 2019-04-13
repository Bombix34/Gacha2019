using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereControlled : MonoBehaviour
{
    private bool m_CanRotate = true;

    public bool CanRotate
    {
        get { return m_CanRotate; }
        set { m_CanRotate = value; }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_CanRotate)
        {
            transform.Rotate(Vector3.right, -1f, Space.World);
        }
    }

    public void TurnRight(float _Value)
    {
        transform.Rotate(Vector3.forward, _Value, Space.World);
    }

    public void Roll(float _Value)
    {
        //transform.Rotate(Vector3.forward, _Value, Space.World);
    }
}
