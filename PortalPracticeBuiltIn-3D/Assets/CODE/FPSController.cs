using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    public Transform m_PitchController;

    public float m_MinPortalScale = 0.5f;
    public float m_MaxPortalScale = 2f;
    Vector3 m_BlueBaseScale;
    Vector3 m_OrangeBaseScale;
    public float m_ScaleScrollSpeed = 2f; 
    float m_PortalScale = 1f;

    float m_Yaw;
    float m_Pitch;
    public float m_YawSpeed;
    public float m_PitchSpeed;
    public bool m_InvertedYaw;
    public bool m_InvertedPitch;
    public float m_MinPitch;
    public float m_MaxPitch;
    public float m_Speed;
    CharacterController m_CharacterController;
    float m_VerticalSpeed = 0;
    public float m_JumpSpeed = 0;
    public float m_SprintSpeed = 0;
    bool angleLocked = false;
    bool cursorLocked = false;

    [Header("Shoot")]
    public Camera m_Camera;
    public float m_MaxShootDistance = 200.0f;
    public LayerMask m_ShootLayerMask;

    [Header("Input")]
    public KeyCode m_LeftKeyCode = KeyCode.A;
    public KeyCode m_RightKeyCode = KeyCode.D;
    public KeyCode m_UpKeyCode = KeyCode.W;
    public KeyCode m_DownKeyCode = KeyCode.S;
    public KeyCode m_JumpKeyCode = KeyCode.Space;
    public KeyCode m_SprintKeyCode = KeyCode.LeftShift;
    public KeyCode m_LockAngle = KeyCode.I;
    public KeyCode m_DebugLockCursorKeyCode = KeyCode.O;
    public KeyCode m_ReloadKey = KeyCode.R;
    public KeyCode m_Grab = KeyCode.E;

    public Animation m_Animation;
    public AnimationClip m_IdleAnimationClip;
    public AnimationClip m_ShootAnimationClip;

    Vector3 m_StartPosition;
    Quaternion m_StartRotation;

    int m_ShootButton = 0;
    int m_SecondShootButton = 1;

    public int m_Score = 0;
    public int m_MaxBullets = 30;
    public int m_CurrentBullets;
    bool reloading = false;
    public int m_DelayReload;
    public int m_InventoryBullets = 90;
    bool m_IsDead = false;

    public Portal PortalBlue;
    public Portal PortalOrange;
    public float m_portalOffset = 2f;
    Vector3 m_Direction;
    public float m_MaxValidAngle = 40f;

    bool m_ObjectAttached;
    bool m_AttachingObject;
    Rigidbody m_GrabObject;
    public Transform grabTransform;
    public float grabTime;
    float Timetograb = 0.8f;
    public float m_ShootAttachedObjectForce;
    bool m_CanTeleport = true;

    [Header("Previews")]
    public GameObject m_ValidBluePreview;
    public GameObject m_ValidOrangePreview;
    public GameObject m_InvalidPortalPreview;

    GameObject m_CurrentPreview;
    GameObject m_CurrentPreviewPrefab;
    Portal m_CurrentPortal;
    bool m_IsPreviewing = false;

    Vector3 m_PreviewPosition;
    Vector3 m_PreviewNormal;
    bool m_IsValidPosition;

    private void Awake()
    {
        m_CharacterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        m_Yaw = transform.rotation.eulerAngles.y;
        m_Pitch = m_PitchController.eulerAngles.x;
        m_StartPosition = transform.position;
        m_StartRotation = transform.rotation;
        m_CurrentBullets = m_MaxBullets;
        m_IsDead = false;
        m_BlueBaseScale = PortalBlue.transform.localScale;
        m_OrangeBaseScale = PortalOrange.transform.localScale;
    }

    private void Update()
    {
        if (GameController.GetGameController().GameOverScreen)
            return;

        if (Input.GetKeyDown(m_LockAngle))
            angleLocked = !angleLocked;
        if (Input.GetKeyDown(m_DebugLockCursorKeyCode))
        {
            Cursor.lockState = Cursor.lockState != CursorLockMode.Locked ? CursorLockMode.Locked : CursorLockMode.None;
        }

        float l_MouseX = Input.GetAxis("Mouse X");
        float l_MouseY = Input.GetAxis("Mouse Y");

        if (m_InvertedYaw)
            l_MouseX = -l_MouseX;
        if (m_InvertedPitch)
            l_MouseY = -l_MouseY;

        m_Yaw = m_Yaw + l_MouseX * m_YawSpeed * Time.deltaTime;
        m_Pitch = m_Pitch + l_MouseY * m_PitchSpeed * Time.deltaTime;

        m_Pitch = Mathf.Clamp(m_Pitch, m_MinPitch, m_MaxPitch);

        transform.rotation = Quaternion.Euler(0.0f, m_Yaw, 0.0f);
        m_PitchController.localRotation = Quaternion.Euler(m_Pitch, 0.0f, 0.0f);

        m_Direction = Vector3.zero;
        if (Input.GetKey(m_RightKeyCode))
            m_Direction += transform.right;
        else if (Input.GetKey(m_LeftKeyCode))
            m_Direction -= transform.right;

        if (Input.GetKey(m_UpKeyCode))
            m_Direction += transform.forward;
        else if (Input.GetKey(m_DownKeyCode))
            m_Direction -= transform.forward;

        float l_Speed = m_Speed;
        if (Input.GetKey(m_SprintKeyCode) && m_CharacterController.isGrounded)
            l_Speed = m_SprintSpeed;
        if (Input.GetKey(m_JumpKeyCode) && m_CharacterController.isGrounded)
            m_VerticalSpeed = m_JumpSpeed;

        if (CanGrabObject())
        {
            GrabObject();
        }

        m_VerticalSpeed = m_VerticalSpeed + Physics.gravity.y * Time.deltaTime;

        m_Direction.Normalize();
        Vector3 l_Direction = m_Direction * l_Speed * Time.deltaTime;
        l_Direction.y = m_VerticalSpeed * Time.deltaTime;
        CollisionFlags l_CollisionFlags = m_CharacterController.Move(l_Direction);
        if ((l_CollisionFlags & CollisionFlags.CollidedBelow) != 0)
            m_VerticalSpeed = 0.0f;

        bool l_CanShoot = CanShoot();
        UpdateAttachedObject();
        UpdateAttachingObject();

        if (l_CanShoot)
        {
            UpdatePortalPreview(m_ShootButton, PortalBlue);
            UpdatePortalPreview(m_SecondShootButton, PortalOrange);
        }
    }

    bool CanGrabObject()
    {
        if (Input.GetKeyDown(m_Grab) && !m_AttachingObject && !m_ObjectAttached) return true;
        return false;
    }

    void GrabObject()
    {
        Ray l_Ray = m_Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

        if (Physics.Raycast(l_Ray, out RaycastHit l_RaycastHit, m_MaxShootDistance))
        {
            if (l_RaycastHit.collider.CompareTag("Cube")|| l_RaycastHit.collider.CompareTag("RefractionCube")|| l_RaycastHit.collider.CompareTag("Turret"))
            {
                GrabObjectt(l_RaycastHit.collider);
            }
        }
    }

    void GrabObjectt(Collider _Collider)
    {
        m_GrabObject = _Collider.GetComponent<Rigidbody>();
        m_GrabObject.isKinematic = true;
        m_AttachingObject = true;
        m_GrabObject.transform.SetParent(grabTransform);
        grabTime = Time.time;
    }

    void UpdateAttachingObject()
    {
        if (m_AttachingObject)
        {
            float l_DiffTime = Time.time - grabTime;
            float l_Pct = Mathf.Clamp01(l_DiffTime / Timetograb);

            m_GrabObject.transform.position = Vector3.Lerp(m_GrabObject.transform.position, grabTransform.position, l_Pct);
            m_GrabObject.transform.rotation = Quaternion.Lerp(m_GrabObject.transform.rotation, grabTransform.rotation, l_Pct);

            if (l_Pct >= 1.0f)
            {
                m_AttachingObject = false;
                m_ObjectAttached = true;
            }
        }
    }

    void UpdateAttachedObject()
    {
        if (m_AttachingObject && Input.GetKeyUp(m_Grab))
        {
            ThrowAttachedObject(0.0f);
        }
        else if (m_ObjectAttached)
        {
            if (Input.GetKeyUp(m_Grab))
            {
                ThrowAttachedObject(0.0f);
            }
            else if (Input.GetMouseButtonDown(m_ShootButton))
            {
                ThrowAttachedObject(m_ShootAttachedObjectForce);
            }
        }
    }

    void ThrowAttachedObject(float Force)
    {
        m_GrabObject.isKinematic = false;
        m_GrabObject.linearVelocity = m_Camera.transform.forward * Force;
        m_GrabObject.transform.SetParent(null);

        m_ObjectAttached = false;
        m_AttachingObject = false;
    }

    bool CanShoot()
    {
        if (m_AttachingObject || m_ObjectAttached)
            return false;

        return true;
    }

    void UpdatePortalPreview(int Button, Portal _Portal)
    {
        if (Input.GetMouseButtonDown(Button))
        {
            m_IsPreviewing = true;
            m_CurrentPortal = _Portal;
        }

        if (!m_IsPreviewing || m_CurrentPortal != _Portal)
            return;

        if (Input.GetMouseButton(Button))
        {
            UpdatePortalScale();
            UpdatePreview();
        }

        if (Input.GetMouseButtonUp(Button))
        {
            FinishPortalPreview();
        }
    }

    void UpdatePreview()
    {
        Ray l_Ray = m_Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(l_Ray, out RaycastHit l_HitInfo, m_MaxShootDistance, m_ShootLayerMask.value))
        {
            m_PreviewPosition = l_HitInfo.point;
            m_PreviewNormal = l_HitInfo.normal;

            m_IsValidPosition = m_CurrentPortal.IsValidPosition(m_PreviewPosition, m_PreviewNormal, m_ShootLayerMask, GetPortalScale(m_CurrentPortal));
            GameObject prefabToUse;

            if (!m_IsValidPosition)
                prefabToUse = m_InvalidPortalPreview;
            else if (m_CurrentPortal == PortalBlue)
                prefabToUse = m_ValidBluePreview;
            else
                prefabToUse = m_ValidOrangePreview;

            if (prefabToUse != null)
            {
                if (m_CurrentPreview == null || m_CurrentPreviewPrefab != prefabToUse)
                {
                    if (m_CurrentPreview != null)
                        Destroy(m_CurrentPreview);

                    m_CurrentPreview = Instantiate(prefabToUse);
                    m_CurrentPreviewPrefab = prefabToUse;
                }

                m_CurrentPreview.transform.position = m_PreviewPosition + (m_PreviewNormal * 0.01f);
                m_CurrentPreview.transform.rotation = Quaternion.LookRotation(m_PreviewNormal);
                m_CurrentPreview.transform.localScale = prefabToUse.transform.localScale * m_PortalScale; 
            }
        }
        else
        {
            m_IsValidPosition = false;
            if (m_CurrentPreview != null)
            {
                Destroy(m_CurrentPreview);
                m_CurrentPreview = null;
            }
        }
    }

    void FinishPortalPreview()
    {
        if (m_CurrentPreview != null)
        {
            Destroy(m_CurrentPreview);
            m_CurrentPreview = null;
        }

        if (m_IsValidPosition)
        {
            m_CurrentPortal.gameObject.SetActive(true);
            m_CurrentPortal.transform.position = m_PreviewPosition;
            m_CurrentPortal.transform.rotation = Quaternion.LookRotation(m_PreviewNormal);
            m_CurrentPortal.transform.localScale = GetPortalScale(m_CurrentPortal);
        }
        else
        {
            m_CurrentPortal.gameObject.SetActive(false);
        }

        m_IsPreviewing = false;
        m_CurrentPortal = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeadZone"))
        {
            StartCoroutine(Kill());
        }

        if (other.CompareTag("Portal") && m_CanTeleport)
        {
            Portal l_Portal = other.GetComponent<Portal>();

            if (l_Portal != null && CanTeleport(l_Portal))
            {
                m_CanTeleport = false;
                Teleport(l_Portal);
                StartCoroutine(TeleportCooldown());
            }
        }
    }

    IEnumerator TeleportCooldown()
    {
        yield return new WaitForSeconds(0.2f);
        m_CanTeleport = true;
    }

    IEnumerator Kill()
    {
        if (m_IsDead)
            yield break;

        m_IsDead = true;
        yield return null;
    }

    public void SetCheckpoint(Vector3 position, Quaternion rotation)
    {
        m_StartPosition = position;
        m_StartRotation = rotation;
    }

    public void Restart()
    {
        m_CharacterController.enabled = false;
        transform.position = m_StartPosition;
        transform.rotation = m_StartRotation;
        m_CharacterController.enabled = true;

        m_IsDead = false;
    }

    private bool CanTeleport(Portal _Portal)
    {
        if (!_Portal.m_OtherPortal.gameObject.activeSelf)
            return false;

        float l_MaxDotAngle = Mathf.Cos(m_MaxValidAngle * Mathf.Deg2Rad);
        Vector3 l_Direction = m_Direction.normalized;

        if (l_Direction == Vector3.zero)
            return false;

        float l_DotAngle = Vector3.Dot(l_Direction, -_Portal.transform.forward);

        return l_DotAngle >= l_MaxDotAngle;
    }

    void UpdatePortalScale()
    {
        float l_Scroll = Input.GetAxis("Mouse ScrollWheel");
        if (l_Scroll != 0f)
            m_PortalScale = Mathf.Clamp(m_PortalScale + l_Scroll * m_ScaleScrollSpeed, m_MinPortalScale, m_MaxPortalScale);
    }

    Vector3 GetPortalScale(Portal _Portal)
    {
        Vector3 l_BaseScale = _Portal == PortalBlue ? m_BlueBaseScale : m_OrangeBaseScale;
        return l_BaseScale * m_PortalScale;
    }

    void Teleport(Portal _portal)
    {
        Transform l_In = _portal.transform;
        Transform l_Out = _portal.m_OtherPortalTransform;

        Vector3 l_LocalPosition = l_In.InverseTransformPoint(transform.position);
        Vector3 l_LocalForward = l_In.InverseTransformDirection(m_Camera.transform.forward);

        m_CharacterController.enabled = false;
        transform.position = l_Out.TransformPoint(new Vector3(-l_LocalPosition.x, l_LocalPosition.y, -l_LocalPosition.z));

        Vector3 l_NewForward = l_Out.TransformDirection(l_LocalForward);
        Vector3 l_Flat = new Vector3(l_NewForward.x, 0.0f, l_NewForward.z);
        if (l_Flat.sqrMagnitude > 0.0001f)
        {
            m_Yaw = Quaternion.LookRotation(l_Flat.normalized).eulerAngles.y;
            transform.rotation = Quaternion.Euler(0.0f, m_Yaw, 0.0f);
        }

        m_Pitch = -Mathf.Asin(Mathf.Clamp(l_NewForward.normalized.y, -1f, 1f)) * Mathf.Rad2Deg;
        m_Pitch = Mathf.Clamp(m_Pitch, m_MinPitch, m_MaxPitch);
        m_PitchController.localRotation = Quaternion.Euler(m_Pitch, 0.0f, 0.0f);

        Vector3 l_LocalMove = l_In.InverseTransformDirection(m_Direction);
        m_Direction = l_Out.TransformDirection(l_LocalMove);
        m_Direction.y = 0.0f;

        m_CharacterController.enabled = true;
    }
}