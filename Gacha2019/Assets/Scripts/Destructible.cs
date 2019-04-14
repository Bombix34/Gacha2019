using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Destructible : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        Player player = other.transform.GetComponent<Player>();
        if (player && Planet.instance.IsBoosting)
        {
            Destroy(gameObject);
        }
    }
}
