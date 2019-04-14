using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Destructible : MonoBehaviour
{
    [SerializeField]
    private bool m_CanBeDestroyed = true;

    private void OnCollisionEnter(Collision other)
    {
        Player player = other.transform.GetComponent<Player>();
        if (player && Planet.instance.IsBoosting)
        {
            if (m_CanBeDestroyed)
            {
                Destroy(gameObject);
            }
            else
            {
                Planet.instance.KnockBack(30);
                player.KnockBack(0.5f);
            }
        }
    }
}
