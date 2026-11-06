using UnityEngine;

public class Companioncube : MonoBehaviour
{
    public float m_MaxValidAngle=40f;
    Rigidbody m_Rigidbody;
    float m_portalOffset=1f;
    private void Awake()
    {
        m_Rigidbody=GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Portal"))
        {
            Portal l_Portal = other.GetComponent<Portal>();
            if(CanTeleport(l_Portal))
            Teleport(l_Portal);
        }


    }

    private bool CanTeleport(Portal _Portal)
    {
        float l_MaxDotAngle = Mathf.Cos(m_MaxValidAngle * Mathf.Deg2Rad);
        Vector3 l_Direction = m_Rigidbody.linearVelocity.normalized;
        float l_DotAngle = Vector3.Dot(l_Direction, _Portal.transform.forward);
        return l_DotAngle >= l_MaxDotAngle;
    }

    void Teleport(Portal _portal)
    {
        Vector3 l_Direction=m_Rigidbody.linearVelocity.normalized;
        Vector3 l_WorldPosition = transform.position + l_Direction * m_portalOffset;
        Vector3 l_WorldForward = transform.forward;
        Vector3 l_WorldLinealVelocity=m_Rigidbody.linearVelocity;
        Vector3 l_LocalPosition = _portal.m_OtherPortalTransform.InverseTransformPoint(l_WorldPosition);
        Vector3 l_localForward = _portal.m_OtherPortalTransform.InverseTransformDirection(l_WorldForward);
        Vector3 l_LocalLinealVelocity = _portal.m_OtherPortal.transform.InverseTransformDirection(l_WorldLinealVelocity);

        m_Rigidbody.isKinematic = true;
        //m_CharacterController.enabled = false;
        transform.position = _portal.m_OtherPortal.transform.TransformPoint(l_LocalPosition);
        transform.forward = _portal.m_OtherPortal.transform.TransformDirection(l_localForward);
        m_Rigidbody.isKinematic = false;
        m_Rigidbody.linearVelocity=_portal.m_OtherPortal.transform.TransformDirection(l_LocalLinealVelocity);
        //m_Yaw = transform.rotation.eulerAngles.y;
        //m_CharacterController.enabled = true;
    }
}

