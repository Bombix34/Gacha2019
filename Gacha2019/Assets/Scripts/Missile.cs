using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField]
    private float m_Speed = 10;

    private float m_CurrentOffset = 0;

    private Planet m_Planet;

    private Vector2 m_Direction;

    private float m_BaseOffset;
    private float m_Rotation;
    
    public void Launch(Vector2 _Direction, Planet _Planet, float _BaseOffset, float _Rotation)
    {
        m_Direction = _Direction;
        m_Planet = _Planet;
        m_BaseOffset = _BaseOffset;
        m_Rotation = _Rotation;
    }

    private void Update()
    {
        m_CurrentOffset += m_Speed * Time.deltaTime;
        SetPositionOnPlanet(m_CurrentOffset);
    }

    private void SetPositionOnPlanet(float _Offset)
    {
        if (m_Planet != null)
        {
            Vector3 localPosition = new Vector3(0, m_Planet.Radius, 0);
            Quaternion rotation = Quaternion.Euler(-90 + m_BaseOffset + _Offset * m_Direction.x, 0, _Offset * m_Direction.y);
            localPosition = rotation * localPosition;
            transform.position = m_Planet.transform.position + localPosition;
            transform.rotation = rotation;
            transform.Rotate(Vector3.up, m_Rotation);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Planet>() == null && other.GetComponent<Player>() == null)
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
