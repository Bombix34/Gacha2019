using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveParticle : MonoBehaviour
{
    [SerializeField]
    GameObject baseParticles;
    [SerializeField]
    GameObject speedBoostParticles;

    void Start()
    {
        speedBoostParticles.SetActive(false);
        baseParticles.SetActive(false);
    }

    public void LaunchParticles(bool isBase)
    {
        baseParticles.SetActive(isBase);
        speedBoostParticles.SetActive(!isBase);
    }



}
