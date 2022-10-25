using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FPPlayerController : MonoBehaviour
{
    float m_Yaw;
    float m_Pitch;
    public float m_YawRotationalSpeed;
    public float m_PitchRotationalSpeed;

    public float m_MinPitch;
    public float m_MaxPitch;

    public Transform m_PitchController;
    public bool m_UseYawInverted;
    public bool m_UsePitchInverted;

    public CharacterController m_CharacterController;
    public float m_Speed;
    public float m_FastSpeedMultiplier = 3f;
    public KeyCode m_LeftKeyCode;
    public KeyCode m_RightKeyCode;
    public KeyCode m_UpKeyCode;
    public KeyCode m_DownKeyCode;
    public KeyCode m_JumpKeyCode;
    public KeyCode m_RunKeycode = KeyCode.LeftShift;
    public KeyCode m_ReloadKeyCode = KeyCode.R;

    public Camera m_Camera;
    public float m_NormalMovementFOV = 60;
    public float m_RunMovementFOV = 70;

    float m_VerticalSpeed = 0.0f;
    bool m_OnGround = true;

    public float m_JumpSpeed = 10.0f;


    public KeyCode m_DebugLockAngleKeyCode = KeyCode.I;
    public KeyCode m_DebugLockKeyCode = KeyCode.O;
    bool m_AngleLocked = false;
    bool m_AimLocked = true;

    Vector3 m_StartPosition;
    Quaternion m_StartRotation;

    [Header("Shoot")]
    public float m_MaxShootDistance = 50.0f;
    public LayerMask m_ShootingLayerMask;
    public GameObject m_DecalPrefab;
    TCObjectPool m_DecalsPool;
    bool CanShoot = true;

    [Header("Animations")]
    public Animation m_Animation;
    public AnimationClip m_IdleAnimationClip;
    public AnimationClip m_ShootAnimationClip;
    public AnimationClip m_ReloadAnimationClip;

    [Header("Life")]
    public float m_Life;
    float m_MaxLife = 1.0f;
    public float m_Shield;
    public float m_DroneDamage;

    [Header("HUD")]
    public Canvas HUD;
    public HudPoints PointCounter;
    public DestroyDummy TheDummy;

    [Header("Ammo")]
    public int m_MaxAmmo = 50;
    public int m_CurrentAmmo = 10;
    public int m_TimesShot;

    [Header("Keys")]
    public bool HasAKey = false;

    void Start()
    {
        m_DroneDamage = GameController.GetGameController().GetDroneDamage();
        m_Life = GameController.GetGameController().GetPlayerLife();
        m_Shield = GameController.GetGameController().GetPlayerShield();
        GameController.GetGameController().SetPlayer(this);
        Debug.Log(m_Life);
        m_Yaw = transform.rotation.y;
        m_Pitch = m_PitchController.localRotation.x;
        Cursor.lockState = CursorLockMode.Locked;
        m_AimLocked = Cursor.lockState == CursorLockMode.Locked;
        SetIdleWeaponAnimation();
        m_StartPosition = transform.position;
        m_StartRotation = transform.rotation;
        m_DecalsPool = new TCObjectPool(5, m_DecalPrefab);
    }

#if UNITY_EDITOR
    void UpdateInputDebug()
    {
        if (Input.GetKeyDown(m_DebugLockAngleKeyCode))
            m_AngleLocked = !m_AngleLocked;
        if (Input.GetKeyDown(m_DebugLockKeyCode))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
                    Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
            m_AimLocked = Cursor.lockState == CursorLockMode.Locked;
        }
    }
#endif

    // Update is called once per frame
    void Update()
    {

#if UNITY_EDITOR
        UpdateInputDebug();
#endif

        Debug.Log(m_Life);
        Debug.Log(m_Shield);
        Vector3 l_RightDirection = transform.right;
        Vector3 l_ForwardDirection = transform.forward;
        Vector3 l_Direction = Vector3.zero;
        float l_Speed = m_Speed;

        if(Input.GetKey(m_UpKeyCode))
            l_Direction = l_ForwardDirection;
        if (Input.GetKey(m_DownKeyCode))
            l_Direction = -l_ForwardDirection;
        if (Input.GetKey(m_RightKeyCode))
            l_Direction += l_RightDirection;
        if (Input.GetKey(m_LeftKeyCode))
            l_Direction -= l_RightDirection;
        if (Input.GetKeyDown(m_JumpKeyCode) && m_OnGround)
            m_VerticalSpeed = m_JumpSpeed;
        float l_FOV = m_NormalMovementFOV;
        if (Input.GetKey(m_RunKeycode))
        {
            l_Speed = m_Speed * m_FastSpeedMultiplier;
            l_FOV = m_RunMovementFOV;
        }
        m_Camera.fieldOfView = l_FOV;

        l_Direction.Normalize();
        Vector3 l_Movement = l_Direction * l_Speed * Time.deltaTime;

        //Rotation
        float l_MouseX = Input.GetAxis("Mouse X");
        float l_MouseY = Input.GetAxis("Mouse Y");
        float l_YawRotationalSpeed = m_YawRotationalSpeed;
        float l_PitchRotationalSpeed = m_PitchRotationalSpeed;

        if(m_AngleLocked)
        {
            l_MouseX = 0.0f;
            l_MouseY = 0.0f;
        }

        m_Yaw = m_Yaw + l_MouseX * m_YawRotationalSpeed *Time.deltaTime * (m_UseYawInverted ? -1.0f : 1.0f);
        m_Pitch = m_Pitch + l_MouseY * m_PitchRotationalSpeed *Time.deltaTime * (m_UsePitchInverted ? -1.0f : 1.0f);
        m_Pitch = Mathf.Clamp(m_Pitch, m_MinPitch, m_MaxPitch);

        transform.rotation = Quaternion.Euler(0.0f, m_Yaw, 0.0f);
        m_PitchController.localRotation = Quaternion.Euler(m_Pitch, 0.0f, 0.0f);

        m_VerticalSpeed = m_VerticalSpeed + Physics.gravity.y * Time.deltaTime;
        l_Movement.y = m_VerticalSpeed * Time.deltaTime;

        CollisionFlags l_CollisionFlags =  m_CharacterController.Move(l_Movement);

        if ((l_CollisionFlags & CollisionFlags.Above) != 0 && m_VerticalSpeed > 0.0f)
        {
            m_VerticalSpeed = 0.0f;
        }
        if ((l_CollisionFlags & CollisionFlags.Below)!=0)
        {
            m_VerticalSpeed = 0.0f;
            m_OnGround = true;
        }
        else
        {
            m_OnGround = false;
        }

        if(Input.GetMouseButtonDown(0) && CanShoot)
        {
            Shoot();
        }
        if (Input.GetKey(m_ReloadKeyCode))
        {
            SetReloadAnimation();
            Reload();
        }
    }
    

    void Shoot()
    {
        Ray l_ray = m_Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        RaycastHit l_RaycastHit;
        if(Physics.Raycast(l_ray, out l_RaycastHit, m_MaxShootDistance, m_ShootingLayerMask.value))
        {
            if (l_RaycastHit.collider.tag == "DroneCollider")
            {
                l_RaycastHit.collider.GetComponent<HitCollider>()?.Hit();
            }
            if (l_RaycastHit.collider.tag == "Target")
            {
                PointCounter.AddPoint(25);
                TheDummy.NormalShot();
                Debug.Log("normalshot: " + TheDummy.TimesShooted);
            }
            else if(l_RaycastHit.collider.tag == "SmallTarget")
            {
                PointCounter.AddPoint(50);
                TheDummy.HeadShot();
                Debug.Log("headshot: " + TheDummy.TimesShooted);
            }
            
            CreateShootHitParticles(l_RaycastHit.collider, l_RaycastHit.point, l_RaycastHit.normal);
        }

        SetShootWeaponAnimation();
        
        m_TimesShot++;
        if(m_TimesShot >= 10)
        {
            CanShoot = false;
        }
    }
    void Reload()
    {
        m_TimesShot = 0;
        StartCoroutine(ShootAfterReload());
    }

    IEnumerator ShootAfterReload()
    {
        yield return new WaitForSeconds(m_ReloadAnimationClip.length);
        CanShoot = true;
    }

    private void CreateShootHitParticles(Collider _Collider, Vector3 Position, Vector3 Normal)
    {
        //Debug.DrawRay(Position, Normal * 5.0f, Color.red, 2.0f);
        GameObject.Instantiate(m_DecalPrefab, Position, Quaternion.LookRotation(Normal));
        //GameObject l_Decal = m_DecalsPool.GetNextElement();
        //l_Decal.SetActive(true);
        //l_Decal.transform.position = Position;
        //l_Decal.transform.rotation = Quaternion.LookRotation(Normal);
    }

    void SetIdleWeaponAnimation()
    {
        m_Animation.CrossFade(m_IdleAnimationClip.name);
    }
    void SetShootWeaponAnimation()
    {
        m_Animation.CrossFade(m_ShootAnimationClip.name, 0.1f);
        m_Animation.CrossFadeQueued(m_IdleAnimationClip.name, 0.1f);
        StartCoroutine(EndShoot());
    }
    void SetReloadAnimation()
    {
        m_Animation.CrossFade(m_ReloadAnimationClip.name, 0.1f);
        m_Animation.CrossFadeQueued(m_IdleAnimationClip.name, 0.5f);
    }

    IEnumerator EndShoot()
    {
        yield return new WaitForSeconds(m_ShootAnimationClip.length);
        
    }
    public float GetLife()
    {
        return m_Life;
    }
    public void AddLife(float Life)
    {
        m_Life = Mathf.Clamp(m_Life + Life, 0.0f, m_MaxLife);
    }

    public void AddAmmo(int Ammo)
    {

    }

    public void RestLife()
    {
        m_Life = m_Life - 0.2f;
    }
    public void GetHit(float damage)
    {
        
        if (m_Shield > 0)
        {
            m_Shield = m_Shield - (damage * 0.75f);
            m_Life = m_Life - (damage * 0.75f);
        }
        else
        {
            m_Life = m_Life - damage;
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Item")
        {
            other.GetComponent<Item>().Pick(this);
        }
        else if(other.tag == "DeadZone")
        {
            Kill();
        }
    }

    void Kill()
    {
        m_Life = 0.0f;
        GameController.GetGameController().RestartGame();
    }
    public void RestartGame()
    {
        m_Life = 1.0f;
        m_CharacterController.enabled = false;
        transform.position = m_StartPosition;
        transform.rotation = m_StartRotation;
        m_CharacterController.enabled = true;
    }
}
