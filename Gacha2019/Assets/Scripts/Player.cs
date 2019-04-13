using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    static private Player instance = null;
    static public Player Instance { get { return instance; } }
    
    [SerializeField]
    private Planet m_Planet;

    [SerializeField]
    private float m_OffsetRegeneration = 5.0f;

    [SerializeField]
    private GameObject m_Missile;
    
    [SerializeField]
    private float m_Speed = 0.3f;

    [SerializeField]
    private Vector3 m_SupposedPosition = new Vector3(0, 0, -20);

    private Rigidbody m_Rigidbody;

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
    }

    private void Update()
    {
        Vector3 position = transform.position;
        position = position.normalized * Mathf.Abs(m_SupposedPosition.z * 0.5f);
        transform.position = position;
    }

    private void FixedUpdate()
    {
        Vector3 up = (transform.position - m_Planet.transform.position).normalized;
        transform.up = up;
        Physics.gravity = up * -9.81f;

        Vector3 wantedMovement = m_SupposedPosition - transform.position;
        float upMultiplier = Vector3.Dot(transform.up, wantedMovement);
        wantedMovement -= transform.up * upMultiplier;
        wantedMovement -= transform.up * 0.5f;
        if (wantedMovement.magnitude > m_Speed)
        {
            wantedMovement = wantedMovement.normalized * m_Speed;
        }
        m_Rigidbody.AddForce(wantedMovement, ForceMode.Impulse);
    }
}
