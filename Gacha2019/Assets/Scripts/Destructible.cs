using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Destructible : MonoBehaviour
{
    [SerializeField]
    private bool m_CanBeDestroyed = true;

    [SerializeField]
    private int m_BoostSpeedStepNeeded = 1;

    private void OnCollisionEnter(Collision other)
    {
        Player player = other.transform.GetComponent<Player>();
        if (player && Planet.instance.IsBoosting)
        {
            if (m_CanBeDestroyed)
            {
                if (Planet.instance.BoostStep >= m_BoostSpeedStepNeeded)
                {
                    Destroy(gameObject);
                }
                else
                {
                    Planet.instance.KnockBack(30);
                    player.KnockBack(0.5f);
                }
            }
            else
            {
                Planet.instance.KnockBack(30);
                player.KnockBack(0.5f);
            }
        }
    }
}
