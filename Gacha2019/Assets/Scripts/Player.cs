using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    static private Player instance = null;
    static public Player Instance { get { return instance; } }
    
    [SerializeField]
    private float m_Speed = 3;

    private Rigidbody m_Rigidbody;

    private float m_KnockBackDuration = 0;

    private Animator m_Animator;

    private Material[] baseMats;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Animator = GetComponentInChildren<Animator>();
        baseMats = GetComponentInChildren<SkinnedMeshRenderer>().materials;
    }

    private void Update()
    {
        Vector3 position = transform.position;
        position = position.normalized * Mathf.Abs(Planet.instance.Radius);
        transform.position = position;
    }

    private void FixedUpdate()
    {
        Vector3 up = (transform.position - Planet.instance.transform.position).normalized;
        transform.up = up;
        Physics.gravity = up * -9.81f;
        
        if (m_KnockBackDuration > 0)
        {
            m_KnockBackDuration -= Time.deltaTime;
        }
        else
        {
            Vector3 supposedPosition = new Vector3(0, 0, -Planet.instance.Radius);
            Vector3 wantedMovement = supposedPosition - transform.position;
            float upMultiplier = Vector3.Dot(transform.up, wantedMovement);
            wantedMovement -= transform.up * upMultiplier;
            wantedMovement -= transform.up * 0.5f;
            if (wantedMovement.magnitude > m_Speed)
            {
                wantedMovement = wantedMovement.normalized * m_Speed;
            }
            Vector3 forceNeeded = wantedMovement - m_Rigidbody.velocity;
            m_Rigidbody.AddForce(forceNeeded, ForceMode.Impulse);
        }
    }

    public void KnockBack(float _KnockBackDuration)
    {
        m_KnockBackDuration = _KnockBackDuration;
    }

    public void ResetMaterial()
    {
        GetComponentInChildren<SkinnedMeshRenderer>().materials = baseMats;
    }

    public Animator GetPlayerAnim()
    {
        return m_Animator;
    }
}
