﻿using System.Collections;
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
    private int m_SpeedPadCount = 10;

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

    [SerializeField]
    private GameObject m_TriggerEndPrefab = null;

    [SerializeField]
    private GameObject m_SpeedPadPrefab = null;

    [SerializeField]
    private bool auto = false;

    private Vector3 m_LastMousePosition;

    private bool m_IsCursorPressed = false;

    private List<GameObject> m_objectsOnPlanet;

    private float m_SpeedMultiplier = 1f;

    private float m_BoostDuration = 0f;

    private bool m_IsBoosting = false;

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

    private void Awake()
    {
        m_objectsOnPlanet = new List<GameObject>();
    }

    private void Start()
    {
        if (auto)
        {
            ScalePlanet();
            InitObjectOnPlanet();
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
        transform.Rotate(-m_RotationSpeed * m_SpeedMultiplier * Time.deltaTime, 0, 0);
        //when player is boost
        if (m_IsBoosting)
        {
            m_BoostDuration -= Time.deltaTime;
            if (m_BoostDuration <= 0f)
            {
                m_IsBoosting = false;
                m_SpeedMultiplier = 1f;
            }
        }
    }

    private void InitObjectOnPlanet()
    {
        //PLACEMENT ALEATOIRE DES OBJETS
        if (m_ObstaclePrefab != null)
        {
            for (int i = 0; i < m_ObstacleCount; i++)
            {
                Vector3 localPosition = new Vector3(0, Radius, 0);
                Quaternion rotation = Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), 0);
                localPosition = rotation * localPosition;
                m_objectsOnPlanet.Add(Instantiate(m_ObstaclePrefab, transform.position + localPosition, rotation, m_PlanetAutoRotation));
            }
        }
        if (m_DestructiblePrefab != null)
        {
            for (int i = 0; i < m_DestructibleCount; i++)
            {
                Vector3 localPosition = new Vector3(0, Radius, 0);
                Quaternion rotation = Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), 0);
                localPosition = rotation * localPosition;
                GameObject go = Instantiate(m_DestructiblePrefab, transform.position + localPosition, rotation, m_PlanetAutoRotation);
                Destructible destructible = go.transform.GetChild(0).GetComponent<Destructible>();
                if (destructible)
                {
                    destructible.OnDestructibleTriggerEnter.AddListener(OnDestructibleTriggered);
                }
                m_objectsOnPlanet.Add(go);
            }
        }
        if (m_ShooterPrefab != null)
        {
            for (int i = 0; i < m_ShooterCount; i++)
            {
                Vector3 localPosition = new Vector3(0, Radius, 0);
                Quaternion rotation = Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), 0);
                localPosition = rotation * localPosition;
                m_objectsOnPlanet.Add(Instantiate(m_ShooterPrefab, transform.position + localPosition, rotation, m_PlanetAutoRotation));
            }
        }
        if (m_TriggerEndPrefab != null)
        {
            Vector3 localPosition = new Vector3(0, Radius, 0);
            Quaternion rotation = Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), 0);
            localPosition = rotation * localPosition;
            m_objectsOnPlanet.Add(Instantiate(m_TriggerEndPrefab, transform.position + localPosition, rotation, m_PlanetAutoRotation));
        }
        if (m_SpeedPadPrefab != null)
        {
            for (int i = 0; i < m_SpeedPadCount; i++)
            {
                Vector3 localPosition = new Vector3(0, Radius, 0);
                Quaternion rotation = Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), 0);
                localPosition = rotation * localPosition;
                GameObject go = Instantiate(m_SpeedPadPrefab, transform.position + localPosition, rotation, m_PlanetAutoRotation);
                SpeedPad test = go.transform.GetChild(0).GetComponent<SpeedPad>();
                if (test)
                {
                    test.OnBeginBoost.AddListener(OnBoostBegin);
                }
                m_objectsOnPlanet.Add(go);
            }
        }
    }

    public void ResetObjectsOnPlanet()
    {
        foreach (GameObject obj in m_objectsOnPlanet)
        {
            Destroy(obj);
        }
    }

    private void ScalePlanet()
    {
        if (m_PlanetModel != null)
        {
            m_PlanetModel.localScale = new Vector3(m_Radius * 2, m_Radius * 2, m_Radius * 2);
        }
    }

    public void SetUpNextPlanet()
    {
        m_Radius *= 0.8f;
        ResetObjectsOnPlanet();
        ScalePlanet();
        InitObjectOnPlanet();
    }

    void OnBoostBegin(float _SpeedMultiplier, float _BoostDuration)
    {
        m_IsBoosting = true;
        m_SpeedMultiplier += _SpeedMultiplier;
        m_BoostDuration = _BoostDuration;
    }

    void OnDestructibleTriggered(Destructible _Destructible)
    {
        if (m_IsBoosting)
        {
            Destroy(_Destructible.transform.parent.gameObject);
        }
    }

    //SINGLETON________________________________________________________________________________________________
    private static Planet s_Instance = null;

    // This defines a static instance property that attempts to find the manager object in the scene and
    // returns it to the caller.
    public static Planet instance
    {
        get
        {
            if (s_Instance == null)
            {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first AManager object in the scene.
                s_Instance = FindObjectOfType(typeof(Planet)) as Planet;
            }

            // If it is still null, create a new instance
            if (s_Instance == null)
            {
                Debug.Log("error");
                GameObject obj = new GameObject("Error");
                s_Instance = obj.AddComponent(typeof(Planet)) as Planet;
            }

            return s_Instance;
        }
    }

}
