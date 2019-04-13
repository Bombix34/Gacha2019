using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private float m_Speed = 5;
    [SerializeField] private float m_LifeTime = 5;

    private float timer;

    private float m_RadiusPosition;

    private void Start()
    {
        timer = m_LifeTime;
        m_RadiusPosition = transform.position.magnitude;
    }

    private void Update()
    {
        float radius = transform.position.magnitude;
        Vector3 up = transform.position / radius;

        transform.LookAt(Player.Instance.transform);
        transform.position += transform.forward * m_Speed * Time.deltaTime;

        transform.position += up * (m_RadiusPosition - radius);

        timer -= Time.deltaTime;

        if (timer <= 0.0f)
        {
            Destroy(gameObject);
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
        }

        Destroy(gameObject);
    }
}
