using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Companioncube : MonoBehaviour
{
    public float m_MaxValidAngle=40f;
    bool m_CanTeleport = false;
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
            StartCoroutine(TeleportCooldown());
        }


    }

    private bool CanTeleport(Portal _Portal)
    {
        float l_MaxDotAngle = Mathf.Cos(m_MaxValidAngle * Mathf.Deg2Rad);
        Vector3 l_Direction = m_Rigidbody.linearVelocity.normalized;
        float l_DotAngle = Vector3.Dot(l_Direction, _Portal.transform.forward);
        Debug.Log(l_DotAngle >= l_MaxDotAngle);
        return l_DotAngle >= l_MaxDotAngle;
    }


    void Teleport(Portal _portal)
    {
        //Vector3 l_Direction = m_Rigidbody.linearVelocity.normalized;
        //Vector3 l_WorldPosition = transform.position + l_Direction * m_portalOffset;
        //Vector3 l_WorldForward = transform.forward;
        //Vector3 l_WorldLinealVelocity = m_Rigidbody.linearVelocity;
        ////Vector3 l_LocalPosition = _portal.m_OtherPortalTransform.InverseTransformPoint(l_WorldPosition);
        //Vector3 l_localForward = _portal.m_OtherPortalTransform.InverseTransformDirection(l_WorldForward);
        //Vector3 l_LocalLinealVelocity = _portal.m_OtherPortal.transform.InverseTransformDirection(l_WorldLinealVelocity);

        //m_Rigidbody.isKinematic = true;
        ////m_CharacterController.enabled = false;
        //transform.position = _portal.m_OtherPortal.transform.TransformPoint(l_LocalPosition);
        //transform.forward = _portal.m_OtherPortal.transform.TransformDirection(l_localForward);
        //m_Rigidbody.isKinematic = false;
        //m_Rigidbody.linearVelocity = _portal.m_OtherPortal.transform.TransformDirection(l_LocalLinealVelocity);
        //m_Yaw = transform.rotation.eulerAngles.y;
        //m_CharacterController.enabled = true;

        float l_Scale = _portal.m_OtherPortal.transform.localScale.x / _portal.transform.localScale.x; ;
        transform.localScale = Vector3.one * transform.localScale.x * l_Scale;

        Vector3 l_LocalPosition = _portal.transform.InverseTransformPoint(transform.position);

        transform.position = _portal.m_OtherPortalTransform.TransformPoint(new Vector3(-l_LocalPosition.x, l_LocalPosition.y, -l_LocalPosition.z));

        transform.forward = -_portal.m_OtherPortalTransform.forward;

        Transform l_In = _portal.transform;
        Transform l_Out = _portal.m_OtherPortal.transform;

        // Posición: x espejada, y igual, y SIEMPRE delante del portal de salida
        Vector3 l_LocalPos = l_In.InverseTransformPoint(m_Rigidbody.position);
        Vector3 l_NewPos = l_Out.TransformPoint(new Vector3(-l_LocalPos.x, l_LocalPos.y, 0));

        // Velocidad: misma magnitud, pero z forzada hacia fuera de la pared
        Vector3 l_LocalVel = l_In.InverseTransformDirection(m_Rigidbody.linearVelocity);
        Vector3 l_NewVel = l_Out.TransformDirection(new Vector3(-l_LocalVel.x, l_LocalVel.y, Mathf.Abs(l_LocalVel.z)));

        // Rotación completa
        Quaternion l_NewRot = l_Out.rotation * Quaternion.Euler(0f, 180f, 0f)
                              * Quaternion.Inverse(l_In.rotation) * transform.rotation;

        m_Rigidbody.position = l_NewPos;
        m_Rigidbody.rotation = l_NewRot;
        transform.SetPositionAndRotation(l_NewPos, l_NewRot);
        Physics.SyncTransforms();

        m_Rigidbody.linearVelocity = l_NewVel;
        m_Rigidbody.angularVelocity = Vector3.zero;
    }
    IEnumerator TeleportCooldown()
    {
        Debug.Log("Cooling down");
        yield return new WaitForSeconds(1f);
        m_CanTeleport = true;
    }

}

