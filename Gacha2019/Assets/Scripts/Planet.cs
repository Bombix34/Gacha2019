using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField]
    private float m_RotationSpeed = 10;

    [SerializeField]
    private float m_Radius = 10;

    [SerializeField]
    private int m_ObstacleCount = 50;

    [SerializeField]
    private int m_DestructibleCount = 30;

    [SerializeField]
    private int m_ShooterCount = 5;

    [SerializeField]
    private float m_MovingSpeed = 0.5f;

    [SerializeField]
    private Transform m_PlanetModel = null;

    [SerializeField]
    private Transform m_PlanetAutoRotation = null;

    [SerializeField]
    private GameObject m_ObstaclePrefab = null;

    [SerializeField]
    private GameObject m_DestructiblePrefab = null;

    [SerializeField]
    private GameObject m_ShooterPrefab = null;

    private Vector3 m_LastMousePosition;

    private bool m_IsCursorPressed = false;

    public float Radius
    {
        get
        {
            return m_Radius;
        }
    }

    public float Speed
    {
        get
        {
            return m_RotationSpeed;
        }
    }

    private void Start()
    {
        if (m_PlanetModel != null)
        {
            m_PlanetModel.localScale = new Vector3(m_Radius * 2, m_Radius * 2, m_Radius * 2);
        }
        if (m_ObstaclePrefab != null)
        {
            for (int i = 0; i < m_ObstacleCount; i++)
            {
                Vector3 localPosition = new Vector3(0, Radius, 0);
                Quaternion rotation = Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), 0);
                localPosition = rotation * localPosition;
                Instantiate(m_ObstaclePrefab, transform.position + localPosition, rotation, m_PlanetAutoRotation);
            }
        }
        if (m_DestructiblePrefab != null)
        {
            for (int i = 0; i < m_DestructibleCount; i++)
            {
                Vector3 localPosition = new Vector3(0, Radius, 0);
                Quaternion rotation = Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), 0);
                localPosition = rotation * localPosition;
                Instantiate(m_DestructiblePrefab, transform.position + localPosition, rotation, m_PlanetAutoRotation);
            }
        }
        if (m_ShooterPrefab != null)
        {
            for (int i = 0; i < m_ShooterCount; i++)
            {
                Vector3 localPosition = new Vector3(0, Radius, 0);
                Quaternion rotation = Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), 0);
                localPosition = rotation * localPosition;
                Instantiate(m_ShooterPrefab, transform.position + localPosition, rotation, m_PlanetAutoRotation);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            m_IsCursorPressed = true;
            m_LastMousePosition = Input.mousePosition;
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            m_IsCursorPressed = false;
        }
        if (m_IsCursorPressed)
        {
            Vector3 currentMousePosition = Input.mousePosition;
            Vector3 middleOfScreen = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);

            Vector3 oldVector = m_LastMousePosition - middleOfScreen;
            Vector3 newVector = currentMousePosition - middleOfScreen;

            float rotation = Vector3.SignedAngle(oldVector, newVector, Vector3.forward);
            m_PlanetAutoRotation.Rotate(0, 0, rotation * m_MovingSpeed, Space.World);

            m_LastMousePosition = currentMousePosition;
        }
        transform.Rotate(-m_RotationSpeed * Time.deltaTime, 0, 0);
    }
}
