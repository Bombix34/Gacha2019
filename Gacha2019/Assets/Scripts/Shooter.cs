using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private GameObject m_RocketPrefab;
    [SerializeField] private float m_FireRate = 1.0f;
    [SerializeField] private float m_Range = 10.0f;


    private float m_Timer;

    private Transform m_ShootPostion;
    public Vector3 ShootPostion => m_ShootPostion != null ? m_ShootPostion.position : transform.position;


    void Start()
    {
        m_Timer = 0.0f;
        SearchShootPosition();
    }
    
    void Update()
    {
        UpdateTimer();

        if (CanShoot())
        {
            Fire();
            ResetTimer();
        }
    }

    void SearchShootPosition()
    {
        m_ShootPostion = transform.Find("Shoot Position");
    }

    bool CanShoot()
    {
        Vector3 delta =  Player.Instance.transform.position - ShootPostion;

        return m_Timer <= 0.0f && delta.sqrMagnitude <= m_Range * m_Range;
    }

    void ResetTimer()
    {
        m_Timer = 1.0f / m_FireRate;
    }

    void UpdateTimer()
    {
        if (m_Timer > 0.0f)
        {
            m_Timer -= Time.deltaTime;
        }
    }

    void Fire()
    {
        Vector3 lookVector = Player.Instance.transform.position - ShootPostion;
        Quaternion lookQuaternion = Quaternion.LookRotation(lookVector.normalized, transform.up);

        Instantiate(m_RocketPrefab, ShootPostion, lookQuaternion, transform.parent);
    }

    private void OnDrawGizmosSelected()
    {
        if (m_ShootPostion == null)
        {
            SearchShootPosition();
        }

        Gizmos.DrawWireSphere(ShootPostion, m_Range);
    }
}
