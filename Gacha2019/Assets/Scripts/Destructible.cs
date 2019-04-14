using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Destructible : MonoBehaviour
{
    private DestructibleTriggerEvent m_OnTriggerEnter = new DestructibleTriggerEvent();

    public DestructibleTriggerEvent OnDestructibleTriggerEnter
    {
        get
        {
            return m_OnTriggerEnter;
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
        Player player = other.transform.GetComponent<Player>();
        if (player)
        {
            m_OnTriggerEnter.Invoke(this);
        }
    }
}
