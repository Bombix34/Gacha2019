using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathLimit : MonoBehaviour
{

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.instance.GameOver(collision.gameObject);
        }
    }
}
