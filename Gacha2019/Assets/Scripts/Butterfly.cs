using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly : MonoBehaviour
{
    [SerializeField]
    private float m_FleeRange = 10;

    [SerializeField]
    private float m_WanderAngle = 30;

    [SerializeField]
    private float m_WanderTimeBetweenDirectionChange = 0.5f;
    private float m_WanderTimer;

    [SerializeField]
    private float m_Speed = 0.5f;

    private void Update()
    {
        Vector3 currentForward = transform.forward;
        transform.rotation = Quaternion.LookRotation(currentForward, transform.position.normalized);

        Vector3 playerPosition = Player.Instance.transform.position;
        Vector3 wantedMovement = transform.position - playerPosition;
        if (wantedMovement.magnitude < m_FleeRange)
        {
            float upMultiplier = Vector3.Dot(transform.up, wantedMovement);
            wantedMovement -= transform.up * upMultiplier;
            m_WanderTimer = m_WanderTimeBetweenDirectionChange;
        }
        else
        {
            wantedMovement = transform.forward;
            if (m_WanderTimer > 0)
            {
                m_WanderTimer -= Time.deltaTime;
            }
            if (m_WanderTimer <= 0)
            {
                m_WanderTimer = m_WanderTimeBetweenDirectionChange;
                Vector3 randomDirection = Quaternion.Euler(0, Random.Range(-m_WanderAngle, m_WanderAngle), 0) * transform.forward;
                Vector3 targetPoint = transform.position + randomDirection;
                wantedMovement = targetPoint - transform.position;
            }
        }

        Vector3 target = transform.position + wantedMovement;
        target = target.normalized * Planet.instance.Radius;
        wantedMovement = target - transform.position;
        wantedMovement = CollisionAvoidance(wantedMovement);


        Vector3 position = transform.position + wantedMovement.normalized * m_Speed * Time.deltaTime;
        position = position.normalized * Planet.instance.Radius;
        transform.position = position;
        transform.rotation = Quaternion.LookRotation(wantedMovement.normalized, transform.position.normalized);
    }

    private Vector3 CollisionAvoidance(Vector3 _WantedMovement)
    {
        Vector3 newDirection = _WantedMovement;
        Vector3 leftDirection = Quaternion.Euler(transform.up * -30) * _WantedMovement;
        Vector3 rightDirection = Quaternion.Euler(transform.up * 30) * _WantedMovement;
        RaycastHit leftHitInfo;
        RaycastHit rightHitInfo;
        bool hasLeftCollision = Physics.Raycast(transform.position + transform.up, leftDirection, out leftHitInfo, m_Speed * 0.5f);
        bool hasRightCollision = Physics.Raycast(transform.position + transform.up, rightDirection, out rightHitInfo, m_Speed * 0.5f);
        if (hasLeftCollision && hasRightCollision)
        {
            float leftDistance = (leftHitInfo.point - transform.position).magnitude;
            float rightDistance = (rightHitInfo.point - transform.position).magnitude;
            if (leftDistance > rightDistance)
            {
                newDirection = leftDirection;
            }
            else
            {
                newDirection = rightDirection;
            }
        }
        else if (hasLeftCollision)
        {
            newDirection = rightDirection;
        }
        else if (hasRightCollision)
        {
            newDirection = leftDirection;
        }
        return newDirection;
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player)
        {
            Planet.instance.OnFinishLayer();
            player.SetIsCapturingButterfly(true);
            GameManager.instance.ChangeTimeScale(0.2f, 0.5f);
            Destroy(gameObject);
        }
    }
}
