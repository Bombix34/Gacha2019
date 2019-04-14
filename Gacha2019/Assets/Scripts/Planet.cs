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
    private GameObject[] layers;

    [SerializeField]
    private float m_MovingSpeed = 0.5f;

    [SerializeField]
    private float m_MaxMovingSpeed = 5f;

    [SerializeField]
    private Transform m_PlanetAutoRotation = null;

    [SerializeField]
    private float m_KnockBackRecoverySpeed = 70;

    private Vector3 m_LastMousePosition;

    private bool m_IsCursorPressed = false;

    private List<GameObject> m_objectsOnPlanet;

    private float m_SpeedMultiplier = 1f;

    private float m_BoostDuration = 0f;

    private float m_KnockBackPower = 0;

    private int currentLayerIndex;
    private GameObject currentLayer;

    private bool m_IsBoosting = false;
    public bool IsBoosting => m_IsBoosting;

    private int m_BoostStep = 0; 
 
    public int BoostStep 
    { 
        get 
        { 
            return m_BoostStep; 
        } 
    }

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
        currentLayerIndex = 0;

        SpawnNextLayer();
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
            float rotationToApply = rotation * m_MovingSpeed;
            if (rotationToApply > m_MaxMovingSpeed)
            {
                rotationToApply = m_MaxMovingSpeed;
            }
            if (rotationToApply < -m_MaxMovingSpeed)
            {
                rotationToApply = -m_MaxMovingSpeed;
            }
            m_PlanetAutoRotation.Rotate(0, 0, rotationToApply, Space.World);

            m_LastMousePosition = currentMousePosition;
        }
        if (m_KnockBackPower <= 0)
        {
            transform.Rotate(-m_RotationSpeed * m_SpeedMultiplier * Time.deltaTime, 0, 0);
            //when player is boost
            if (m_IsBoosting)
            {
                m_BoostDuration -= Time.deltaTime;
                if (m_BoostDuration <= 0f)
                {
                    m_IsBoosting = false;
                    m_BoostStep = 0; 
                    m_SpeedMultiplier = 1f;
                }
            }
        }
        else
        {
            transform.Rotate(m_KnockBackPower * Time.deltaTime, 0, 0);
            m_KnockBackPower -= m_KnockBackRecoverySpeed * Time.deltaTime;

            m_IsBoosting = false;
            m_BoostStep = 0; 
            m_SpeedMultiplier = 1f;
        }
    }

    private bool SpawnNextLayer()
    {
        if (currentLayer != null)
            Destroy(currentLayer);

        if (currentLayerIndex < layers.Length)
        {
            currentLayer = Instantiate(layers[currentLayerIndex], m_PlanetAutoRotation);
            currentLayerIndex++;

            return true;
        }
        else
        {
            return false;
        }
    }

    public void OnFinishLayer()
    {
        if (GameManager.instance.IsButterflyObjectiveDone() && !SpawnNextLayer())
        {
            Debug.Log("FINISH !!!");
        }
    }

    public void OnBoostBegin(float _SpeedMultiplier, float _BoostDuration)
    {
        if (m_KnockBackPower <= 0)
        {
            m_BoostStep++; 
            m_IsBoosting = true;
            m_SpeedMultiplier += _SpeedMultiplier;
            m_BoostDuration = _BoostDuration;
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


    public void KnockBack(float _KnockBackPower)
    {
        if (m_IsBoosting)
        {
            m_KnockBackPower = _KnockBackPower;
            m_IsBoosting = false;
            m_SpeedMultiplier = 1f;
        }
    }
}
