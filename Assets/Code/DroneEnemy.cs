using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class DroneEnemy : MonoBehaviour
{
    public enum TState
    {
        IDLE = 0,
        PATROL,
        ALERT,
        CHASE,
        ATTACK,
        HIT,
        DIE
    }
    public TState m_State;
    NavMeshAgent m_NavMeshAgent;
    public List<Transform> m_PatrolTargets;
    int m_CurrentPatrolTargetID = 0;
    public float m_HearingDistance = 2.0f;
    public float m_VisualConeAngle = 60.0f;
    public float m_SightDistance = 8.0f;
    public LayerMask m_SightLayerMask;
    public float m_EyesHeight = 1.8f;
    public float m_EyesPlayerHeight = 1.8f;
    public float m_rotationSpeed = 60.0f;
    public float m_Speed = 10.0f;
    public float m_MinDistance = 2.0f;
    public float m_MaxDistance = 6.0f;
    Vector3 m_PlayerPosition;
    Vector3 m_DistanceBetween;

    [Header("UI")]
    public Image m_LifeBarImage;
    public Transform m_LifeBarAnchorPosition;
    //RectTransform m_LifeBarRectTransform;
    float m_Life = 1.0f;

    //Attacking
    public float m_TimeBetweenAttacks;
    bool m_AlreadyAttacked;
    


    private void Awake()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
       
        //m_LifeBarImage.fillAmount = m_Life;
        SetIdleState();
    }

    private void Update()
    {
        switch(m_State)
        {
            case TState.IDLE:
                UpdateIdleState();
                break;
            case TState.PATROL:
                UpdatePatrolState();
                break;
            case TState.ALERT:
                UpdateAlertState();
                break;
            case TState.CHASE:
                UpdateChaseState();
                break;
            case TState.ATTACK:
                UpdateAttackState();
                break;
            case TState.HIT:
                UpdateHitState();
                break;
            case TState.DIE:
                UpdateDieState();
                break;
        }
        Vector3 l_PlayerPosition = GameController.GetGameController().GetPlayer().transform.position;
        Vector3 l_EyesPosition = transform.position + Vector3.up * m_EyesHeight;
        Vector3 l_PlayerEyesPosition = l_PlayerPosition + Vector3.up * m_EyesPlayerHeight;
        Vector3 l_DistanceBetween = m_NavMeshAgent.transform.position - l_PlayerPosition;
        m_PlayerPosition = l_PlayerPosition;
        m_DistanceBetween = l_DistanceBetween;
        //UpdateLifeBarPoition();
    }
    void SetIdleState()
    {
        m_State = TState.IDLE;
        SetPatrolState();
    }
    void UpdateIdleState()
    {
        SetPatrolState();
        
    }
    void SetPatrolState()
    {
        m_State = TState.PATROL;
        m_NavMeshAgent.destination = m_PatrolTargets[m_CurrentPatrolTargetID].position;
    }
    void UpdatePatrolState()
    {
        if (PatrolTargetPositionArrived())
            MoveToNextTargetPosition();
        if (HearsPlayer())
            SetAlertState();

        Debug.Log(PatrolTargetPositionArrived());
    }
    bool HearsPlayer()
    {
        Vector3 l_PlayerPosition = GameController.GetGameController().GetPlayer().transform.position;
        return Vector2.Distance(l_PlayerPosition, transform.position) <= m_HearingDistance;
    }
    bool SeesPlayers()
    {
        Vector3 l_PlayerPosition = GameController.GetGameController().GetPlayer().transform.position;
        Vector3 l_DirectionToPlayerXZ = l_PlayerPosition-transform.position;
        l_DirectionToPlayerXZ.y = 0.0f;
        l_DirectionToPlayerXZ.Normalize();
        Vector3 l_ForwardXZ = transform.forward;
        l_ForwardXZ.y = 0.0f;
        l_ForwardXZ.Normalize();

        Vector3 l_EyesPosition = transform.position + Vector3.up * m_EyesHeight;
        Vector3 l_PlayerEyesPosition = l_PlayerPosition + Vector3.up * m_EyesPlayerHeight;
        Vector3 l_Direction = l_PlayerPosition = l_PlayerPosition - l_EyesPosition;
        float l_Length = l_Direction.magnitude;
        l_Direction /= l_Length;
        Ray l_Ray = new Ray(l_EyesPosition, l_Direction);

        return Vector3.Distance(l_PlayerPosition, transform.position) <m_SightDistance && Vector3.Dot(l_ForwardXZ, l_DirectionToPlayerXZ)> 
            Mathf.Cos(m_VisualConeAngle * Mathf.Deg2Rad / 2.0f) &&
            !Physics.Raycast(l_Ray, l_Length, m_SightLayerMask.value);
    }
    bool PatrolTargetPositionArrived()
    {
        return !m_NavMeshAgent.hasPath && !m_NavMeshAgent.pathPending && m_NavMeshAgent.pathStatus == NavMeshPathStatus.PathComplete;
    }
    void MoveToNextTargetPosition()
    {
        ++m_CurrentPatrolTargetID;
        if (m_CurrentPatrolTargetID >= m_PatrolTargets.Count)
            m_CurrentPatrolTargetID = 0;
        m_NavMeshAgent.destination = m_PatrolTargets[m_CurrentPatrolTargetID].position;
    }
    void SetAlertState()
    {
        m_State = TState.ALERT;
    }
    void UpdateAlertState()
    {
        m_NavMeshAgent.isStopped = true;
        m_NavMeshAgent.transform.Rotate(Vector3.up * m_rotationSpeed * Time.deltaTime);
        if (SeesPlayers())
            SetChaseState();
    }
    void SetChaseState()
    {
        m_State = TState.CHASE;
    }
    void UpdateChaseState()
    {
        m_NavMeshAgent.SetDestination(m_PlayerPosition);

        if (m_DistanceBetween.x <= m_MinDistance)
        {
            SetAttackState();
        }
    }
    void SetAttackState()
    {
        m_State = TState.ATTACK;
    }
    void UpdateAttackState()
    {
        if(m_DistanceBetween.x<= m_MaxDistance)
        {
            m_NavMeshAgent.SetDestination(transform.position);
            transform.LookAt(m_PlayerPosition);

            if (m_AlreadyAttacked)
            {
                m_AlreadyAttacked = true;
                Invoke(nameof(ResetAttack), m_TimeBetweenAttacks);
            }
        }
    }

    private void ResetAttack()
    {
        m_AlreadyAttacked = false;
    }
    void SetHitState()
    {
        m_State = TState.HIT;
    }
    void UpdateHitState()
    {

    }
    void SetDieState()
    {
        m_State = TState.DIE;
    }
    void UpdateDieState()
    {

    }
    public void Hit(float Life)
    {
        m_Life -= Life;
        //m_LifeBarImage.fillAmount = m_Life;
        Debug.Log("hit life" + Life);
    }
    //void UpdateLifeBarPoition()
    //{
    //    Vector3 l_Position = GameController.GetGameController().GetPlayer().m_Camera.WorldToScreenPoint(m_LifeBarAnchorPosition.position);
    //    m_LifeBarRectTransform.anchoredPosition = new Vector3(l_Position.x * 1920.0f, - (1080.0f-l_Position.y*1080.0f), 0.0f);

    //}
}
