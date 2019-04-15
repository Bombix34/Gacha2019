using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField]
    private float m_Speed = 5;
    [SerializeField]
    private float m_LifeTime = 5;
    private float m_TimeBeforeExploding;

    [SerializeField]
    private float m_ScaleIncreaseAfterMerge = 0.5f;

    //boost power multiply the speed of the missile
    [SerializeField]
    private float m_BoostPower = 2f;

    private float m_SpeedMultiplier = 1f;

    private bool m_IsBoosting = false;

    private float m_BoostTimer = 0f;

    private int m_MissileLevel = 1;

    private bool m_HasAlreadyBeenMerged = false;

    public bool IsBoosting
    {
        get
        {
            return m_IsBoosting;
        }
    }

    public int Level
    {
        get
        {
            return m_MissileLevel;
        }
    }

    public bool HasAlreadyBeenMerged
    {
        get
        {
            return m_HasAlreadyBeenMerged;
        }
        set
        {
            m_HasAlreadyBeenMerged = value;
        }
    }

    private void Start()
    {
        m_TimeBeforeExploding = m_LifeTime;
    }

    private void Update()
    {
        m_TimeBeforeExploding -= Time.deltaTime;
        if (m_TimeBeforeExploding <= 0.0f)
        {
            Destroy(gameObject);
        }
        else
        {
            if (m_MissileLevel > 1 && m_IsBoosting)
            {
                MoveForward();
            }
            else
            {
                MoveTowardPlayer();
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
    }

    private void MoveTowardPlayer()
    {
        transform.position += transform.forward * m_SpeedMultiplier * m_Speed * Time.deltaTime;
        Vector3 up = (transform.position).normalized;
        Vector3 lookingDirection = Player.Instance.transform.position - transform.position;
        float upCount = Vector3.Dot(up, lookingDirection);
        lookingDirection -= up * upCount;
        transform.rotation = Quaternion.LookRotation(lookingDirection, up);
    }

    private void MoveForward()
    {
        transform.position += transform.forward * m_SpeedMultiplier * m_Speed * Time.deltaTime;
        Vector3 up = (transform.position).normalized;
        Vector3 forward = transform.forward;
        float upCount = Vector3.Dot(up, forward);
        forward -= up * upCount;
        transform.rotation = Quaternion.LookRotation(forward, up);
    }

    private void OnTriggerEnter(Collider _other)
    {
        Player player = _other.GetComponent<Player>();
        if (player != null)
        {
            Debug.Log("I hit you looser !");
            SoundManager.instance.PlaySound(6);
            Destroy(gameObject);
        }

        SpeedPad boost = _other.GetComponent<SpeedPad>();
        if (boost != null)
        {
            m_IsBoosting = true;
            m_SpeedMultiplier *= 2f;
            m_BoostTimer = 1f;
        }

        Missile missile = _other.GetComponent<Missile>();
        if (missile != null)
        {
            if (!HasAlreadyBeenMerged)
            {
                Debug.Log(Level + " + " + missile.Level);
                if ((missile.Level > Level) || (missile.Level == Level && !IsBoosting && missile.IsBoosting))
                {
                    transform.forward = missile.transform.forward;
                }
                m_MissileLevel += missile.Level;
                Destroy(missile.gameObject);

                if (IsBoosting || missile.IsBoosting)
                {
                    if (!IsBoosting)
                    {
                        m_SpeedMultiplier *= 2f;
                    }
                    m_IsBoosting = true;
                    m_BoostTimer = 1f;
                }

                if (m_MissileLevel >= 5)
                {
                    m_MissileLevel = 5;
                    Debug.Log("VICTORY ?");
                }

                m_TimeBeforeExploding = m_LifeTime * Level;
                float scale = 1 + (Level - 1) * m_ScaleIncreaseAfterMerge;
                transform.localScale = new Vector3(scale, scale, scale);
                
                missile.HasAlreadyBeenMerged = true;
            }
        }
    }
}
