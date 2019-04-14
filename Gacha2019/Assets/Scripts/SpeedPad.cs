using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpeedPad : MonoBehaviour
{
    [SerializeField]
    float m_SpeedMultiplier = 1.5f;

    [SerializeField]
    float m_BoostDurationInSeconds = 1f;

    BeginBoostEvent m_OnBeginBoost = new BeginBoostEvent();

    public BeginBoostEvent OnBeginBoost
    {
        get { return m_OnBeginBoost; }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player)
        {
            m_OnBeginBoost.Invoke(m_SpeedMultiplier, m_BoostDurationInSeconds);
        }
    }
}
