using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleHouse : MonoBehaviour
{

    Animator anim;
    MeshRenderer mesh;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        anim.transform.gameObject.SetActive(false);
        mesh = GetComponentInChildren<MeshRenderer>();
        if (!mesh)
            mesh = GetComponent<MeshRenderer>();
    }

    public void Explode()
    {
        BoxCollider[] collid = GetComponents<BoxCollider>();
        SoundManager.instance.PlaySound(3);
        foreach(BoxCollider col in collid)
        {
            col.enabled = false;
        }
        mesh.enabled = false;
        anim.transform.gameObject.SetActive(true);
        anim.SetTrigger("Explosion");
    }
}
