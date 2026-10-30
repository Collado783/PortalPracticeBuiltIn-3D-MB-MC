using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Portal : MonoBehaviour
{
    FPSController m_Player;
    public Camera m_Camera;
    public Transform m_OtherPortalTransform;
    public Portal m_OtherPortal;
    public float m_CameraOffSet = 0.5f;
    public List<Transform> m_ValidPositions;
    public float m_RayCastValidationOffSetDistance = 0.1f;
    public float m_MaxAngleValidation = 5f;
    void Start()
    {
        
    }

    void Update()
    {
        Camera l_Camera = Camera.main;
        Vector3 l_CameraWorldPosition = l_Camera.transform.position;
        Vector3 l_DirectionWorldPosition = l_Camera.transform.forward;
        Vector3 l_CameraLocalPosition = m_OtherPortal.transform.InverseTransformPoint(l_CameraWorldPosition);
        Vector3 l_DirectionLocalPosition = m_OtherPortal.transform.InverseTransformDirection(l_DirectionWorldPosition);

        m_Camera.transform.position = m_OtherPortalTransform.transform.TransformPoint(l_CameraLocalPosition);
        float l_Distance = Vector3.Distance(m_Camera.transform.position, transform.position);
        m_Camera.nearClipPlane = m_CameraOffSet+l_Distance;

    }
    public bool IsValidPosition(Vector3 Position, Vector3 Normal)
    {
        transform.position = Position;
        transform.rotation = Quaternion.LookRotation(Normal);
        float l_MaxValidationDotAngle = Mathf.Cos(m_MaxAngleValidation + Mathf.Deg2Rad);
        for (int i = 0; i < m_ValidPositions.Count; i++)
        {
            Vector3 l_Direction = m_ValidPositions[i].position - m_Camera.transform.position;
            float l_Distance = l_Direction.magnitude + m_RayCastValidationOffSetDistance;
            l_Direction.Normalize();
            Ray l_ray = new Ray(m_Camera.transform.position, l_Direction);
            if (Physics.Raycast(l_ray, out RaycastHit l_HitInfo, l_Distance, m_Player.m_ShootLayerMask.value, QueryTriggerInteraction.Ignore))
            {
                if (l_HitInfo.collider.CompareTag("PortalEnabled"))
                {
                    float l_DistanceToHitPoint = Vector3.Distance(l_HitInfo.point, m_ValidPositions[i].position);
                    if (l_DistanceToHitPoint < m_RayCastValidationOffSetDistance)
                    {
                        float l_DotAngle = Vector3.Dot(Normal, l_HitInfo.normal);
                        if (l_DotAngle >= l_MaxValidationDotAngle)
                        {
                            return false;
                        }
                    }
                    else return false;
                }
                else return false;
            }
            else return false;
        }
        return true;
    }
}
