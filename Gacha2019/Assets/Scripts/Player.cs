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

    private float m_CurrentOffset = 0;

    private float m_TimeBeforeRegeneration = 0;

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
        SetPositionOnPlanet(0);
    }

    private void Update()
    {
        if (m_TimeBeforeRegeneration > 0)
        {
            m_TimeBeforeRegeneration -= Time.deltaTime;
        }

        if (m_Planet != null && Physics.SphereCast(transform.position + transform.up, 0.5f, transform.forward, out RaycastHit _HitInfo, 0.3f))
        {
            m_CurrentOffset -= m_Planet.Speed * Time.deltaTime;
            m_TimeBeforeRegeneration = 0.5f;
        }

        if (m_TimeBeforeRegeneration <= 0 && m_CurrentOffset < 0)
        {
            m_CurrentOffset += m_OffsetRegeneration * Time.deltaTime;
            if (m_CurrentOffset > 0)
            {
                m_CurrentOffset = 0;
            }
        }
        SetPositionOnPlanet(m_CurrentOffset);
    }

    private void SetPositionOnPlanet(float _Offset)
    {
        if (m_Planet != null)
        {
            Vector3 localPosition = new Vector3(0, m_Planet.Radius, 0);
            Quaternion rotation = Quaternion.Euler(-90 + _Offset, 0, 0);
            localPosition = rotation * localPosition;
            transform.position = m_Planet.transform.position + localPosition;
            transform.rotation = rotation;
        }
    }
}
