using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Destructible : MonoBehaviour
{
    [SerializeField]
    private bool m_CanBeDestroyedByPlayer = true;

    [SerializeField]
    private int m_BoostSpeedStepNeeded = 1;

    [SerializeField]
    private bool m_CanBeDestroyedByMissile = false;

    [SerializeField]
    private bool m_MissileNeedsToBoostToDestroy = false;

    private void OnCollisionEnter(Collision other)
    {
        Player player = other.transform.GetComponent<Player>();
        if (player && Planet.instance.IsBoosting)
        {
            if (m_CanBeDestroyedByPlayer)
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

        Missile missile = other.transform.GetComponent<Missile>();
        if (missile)
        {
            if (missile.Level >= 3)
            {
                Destroy(gameObject);
            }
            else if (m_CanBeDestroyedByMissile)
            {
                if (m_MissileNeedsToBoostToDestroy)
                {
                    if (missile.IsBoosting)
                    {
                        Destroy(missile.gameObject);
                        Destroy(gameObject);
                    }
                }
                else
                {
                    Destroy(missile.gameObject);
                    Destroy(gameObject);
                }
            }
        }
    }
}
