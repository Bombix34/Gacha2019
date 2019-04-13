using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SphereButtons : MonoBehaviour
{
    [SerializeField]
    UnityEvent m_OnClick = null;

    private void OnMouseDown()
    {
        m_OnClick.Invoke();
    }
}
