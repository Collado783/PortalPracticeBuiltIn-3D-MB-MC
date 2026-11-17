using System.Collections;
using UnityEngine;

public class Companioncube : MonoBehaviour
{
    public float m_MaxValidAngle = 90f;
    public float m_ExitOffset = 0.05f;
    public float m_TeleportCooldown = 0.2f;

    bool m_CanTeleport = true;
    Rigidbody m_Rigidbody;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Portal") || !m_CanTeleport)
            return;

        // Si el jugador lo lleva agarrado (kinemático e hijo del jugador), que se mueva con él
        if (m_Rigidbody.isKinematic)
            return;

        Portal l_Portal = other.GetComponent<Portal>();
        if (l_Portal != null && CanTeleport(l_Portal))
        {
            m_CanTeleport = false;
            Teleport(l_Portal);
            StartCoroutine(TeleportCooldown());
        }
    }

    private bool CanTeleport(Portal _Portal)
    {
        // El portal de salida tiene que estar colocado/activo
        if (_Portal.m_OtherPortal == null || !_Portal.m_OtherPortal.gameObject.activeSelf)
            return false;

        Vector3 l_Velocity = m_Rigidbody.linearVelocity;
        if (l_Velocity.sqrMagnitude < 0.0001f)
            return false;

        // El portal "mira" hacia fuera de la pared, así que entrar = ir en contra de su forward
        float l_MaxDotAngle = Mathf.Cos(m_MaxValidAngle * Mathf.Deg2Rad);
        float l_DotAngle = Vector3.Dot(l_Velocity.normalized, -_Portal.transform.forward);

        return l_DotAngle > l_MaxDotAngle;
    }

    void Teleport(Portal _Portal)
    {
        Transform l_In = _Portal.transform;
        Transform l_Out = _Portal.m_OtherPortal.transform;

        float l_Scale = l_Out.localScale.x / l_In.localScale.x;
        transform.localScale *= l_Scale;

        Vector3 l_LocalPos = l_In.InverseTransformPoint(m_Rigidbody.position);
        Vector3 l_NewLocalPos = new Vector3(-l_LocalPos.x, l_LocalPos.y, Mathf.Abs(l_LocalPos.z) + m_ExitOffset);
        Vector3 l_NewPos = l_Out.TransformPoint(l_NewLocalPos);

        Quaternion l_PortalRotation = l_Out.rotation * Quaternion.Euler(0f, 180f, 0f) * Quaternion.Inverse(l_In.rotation);

        Vector3 l_NewVel = l_PortalRotation * m_Rigidbody.linearVelocity;
        Quaternion l_NewRot = l_PortalRotation * m_Rigidbody.rotation;

        m_Rigidbody.position = l_NewPos;
        m_Rigidbody.rotation = l_NewRot;
        transform.SetPositionAndRotation(l_NewPos, l_NewRot);
        Physics.SyncTransforms();

        m_Rigidbody.linearVelocity = l_NewVel;
        m_Rigidbody.angularVelocity = Vector3.zero;
    }

    IEnumerator TeleportCooldown()
    {
        yield return new WaitForSeconds(m_TeleportCooldown);
        m_CanTeleport = true;
    }
}