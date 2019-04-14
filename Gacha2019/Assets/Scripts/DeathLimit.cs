using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathLimit : MonoBehaviour
{

    private void OnTriggerEnter(Collider collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player)
        {
            GameManager.instance.GameOver(player.gameObject);
        }
    }
}
