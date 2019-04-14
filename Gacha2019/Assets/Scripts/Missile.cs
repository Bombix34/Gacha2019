using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private float m_Speed = 5;
    [SerializeField] private float m_LifeTime = 5;
    //boost power multiply the speed of the missile
    [SerializeField] private float m_BoostPower = 2f;

    private float m_SpeedMultiplier = 1f;

    private bool m_IsBoosting = false;

    private float m_BoostTimer = 0f;

    private float m_RadiusPosition;

    public bool IsBoosting
    {
        get
        {
            return m_IsBoosting;
        }
    }

    private void Start()
    {
        m_RadiusPosition = transform.position.magnitude;
    }

    private void Update()
    {
        float radius = transform.position.magnitude;
        Vector3 up = transform.position / radius;

        transform.LookAt(Player.Instance.transform);
        transform.position += transform.forward * m_SpeedMultiplier * m_Speed * Time.deltaTime;

        transform.position += up * (m_RadiusPosition - radius);

        m_LifeTime -= Time.deltaTime;
        if (m_LifeTime <= 0.0f)
        {
            Destroy(gameObject);
        }

        if (m_IsBoosting)
        {
            m_BoostTimer -= Time.deltaTime;

            if (m_BoostTimer <= 0f)
            {
                m_IsBoosting = false;
                m_SpeedMultiplier = 1f;
            }
        }

    }

    //private void SetPositionOnPlanet(float _Offset)
    //{
    //    if (m_Planet != null)
    //    {
    //        Vector3 localPosition = new Vector3(0, m_Planet.Radius, 0);
    //        Quaternion rotation = Quaternion.Euler(-90 + m_BaseOffset + _Offset * m_Direction.x, 0, _Offset * m_Direction.y);
    //        localPosition = rotation * localPosition;
    //        transform.position = m_Planet.transform.position + localPosition;
    //        transform.rotation = rotation;
    //        transform.Rotate(Vector3.up, m_Rotation);
    //    }
    //}

    private void OnTriggerEnter(Collider _other)
    {

        Player player = _other.GetComponent<Player>();

        if (player)
        {
            Debug.Log("I hit you looser !");
            Destroy(gameObject);
        }

        SpeedPad boost = _other.GetComponent<SpeedPad>();
        if (boost)
        {
            m_IsBoosting = true;
            m_SpeedMultiplier *= 2f;
            m_BoostTimer = 1f;
        }
    }
}
