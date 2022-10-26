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
    FPPlayerController Player;
    public List<Transform> m_PatrolTargets;
    int m_CurrentPatrolTargetID = 0;
    public float m_HearingDistance = 1.0f;
    public float m_VisualConeAngle = 60.0f;
    public float m_SightDistance = 8.0f;
    public LayerMask m_SightLayerMask;
    public float m_EyesHeight = 1.8f;
    public float m_EyesPlayerHeight = 1.8f;
    public float m_rotationSpeed = 60.0f;
    public float m_Speed = 10.0f;

    Vector3 AttackDistance = new Vector3(3, 3);
    Vector3 m_PlayerPosition;
    Vector3 m_DistanceBetween;

    [Header("UI")]
    public Image m_LifeBarImage;
    public Transform m_LifeBarAnchorPosition;
    //RectTransform m_LifeBarRectTransform;
    float m_Life = 1.0f;

    //Attacking
    public float m_TimeBetweenAttacks = 3.0f;
    bool m_AlreadyAttacked = false;
    


    private void Awake()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<FPPlayerController>();
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

        
        Debug.DrawLine(l_EyesPosition, l_PlayerEyesPosition, SeesPlayer() ? Color.red : Color.blue);
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(gameObject.transform.position, m_HearingDistance);
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
        {
            SetAlertState();
        }

        Debug.Log(PatrolTargetPositionArrived());
    }

    
    bool HearsPlayer()
    {
        Vector3 l_PlayerPosition = GameController.GetGameController().GetPlayer().transform.position;
        return Vector2.Distance(l_PlayerPosition, transform.position) <= m_HearingDistance;
    }
    bool SeesPlayer()
    {
        Vector3 l_PlayerPosition = GameController.GetGameController().GetPlayer().transform.position;
        Vector3 l_DirectionPlayerXZ = l_PlayerPosition - transform.position;
        l_DirectionPlayerXZ.Normalize();
        Vector3 l_ForwardXZ = transform.forward;
        l_ForwardXZ.Normalize();

        Vector3 l_EyesPosition = transform.position + Vector3.up * m_EyesHeight;
        Vector3 l_PlayerEyesPosition = l_PlayerPosition + Vector3.up * m_EyesPlayerHeight;
        Vector3 l_Direction = l_PlayerPosition - l_EyesPosition;
        float l_Lenght = l_Direction.magnitude;
        l_Direction /= l_Lenght;

        Ray l_Ray = new Ray(l_PlayerEyesPosition, l_Direction);

        return Vector3.Distance(l_PlayerPosition, transform.position) < m_SightDistance && Vector3.Dot(l_ForwardXZ, l_DirectionPlayerXZ) > Mathf.Cos(m_VisualConeAngle * Mathf.Deg2Rad / 2.0f) &&
            !Physics.Raycast(l_Ray, l_Lenght, m_SightLayerMask.value);
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
        m_NavMeshAgent.transform.Rotate(Vector3.up * 60 * Time.deltaTime);
        if (SeesPlayer())
        {
            transform.LookAt(m_PlayerPosition);
            SetChaseState();
            Debug.Log("lo ve");
        }
        else
        {
            KeepPatrolling();
        }
    }
    IEnumerator KeepPatrolling()
    {
        yield return new WaitForSeconds(3.0f);
        m_NavMeshAgent.isStopped = false;
        SetPatrolState();
    }

    void SetChaseState()
    {
        m_State = TState.CHASE;
    }
    void UpdateChaseState()
    {
        m_NavMeshAgent.isStopped = false;
        m_NavMeshAgent.SetDestination(m_PlayerPosition);
        transform.LookAt(m_PlayerPosition);

        if (m_DistanceBetween.x <= AttackDistance.x && m_DistanceBetween.y <= AttackDistance.y)
        {
            SetAttackState();
        }
    }
    void SetAttackState()
    {
        m_State = TState.ATTACK;
        m_NavMeshAgent.isStopped = true;
    }
    void UpdateAttackState()
    {
        if(Vector3.Distance(m_NavMeshAgent.transform.position, m_PlayerPosition) <= 4)
        {
            
            transform.LookAt(m_PlayerPosition);
            
            if(!m_AlreadyAttacked)
            {
                Player.TakeDamage(0.2f);
                m_AlreadyAttacked = true;
                StartCoroutine(ResetDronAttack());
            }
        }
        else
        {
            SetChaseState();
        }
    }

    IEnumerator ResetDronAttack()
    {
        yield return new WaitForSeconds(m_TimeBetweenAttacks);
        m_AlreadyAttacked = false;
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
