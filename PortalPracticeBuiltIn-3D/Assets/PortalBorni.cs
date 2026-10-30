using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Camera m_Camera;
    public Transform m_OtherPortalTransform;
    public Portal m_MirrorPortal;
    public float m_CameraOffset = 0.5f;

    private void Update()
    {
        Camera l_Camera = Camera.main;
        Vector3 l_CameraWorldPosition = l_Camera.transform.position;
        Vector3 l_DirectionWorldPosition = l_Camera.transform.forward;
        Vector3 l_CameraLocalPosition = m_MirrorPortal.transform.InverseTransformPoint(l_CameraWorldPosition);
        Vector3 l_DirectionLocalPosition = m_MirrorPortal.transform.InverseTransformDirection(l_CameraWorldPosition);
        m_Camera.transform.position = m_OtherPortalTransform.TransformPoint(l_CameraLocalPosition);
        m_Camera.transform.forward = m_OtherPortalTransform.TransformDirection(l_DirectionLocalPosition);
        float l_Distance = Vector3.Distance(m_Camera.transform.position, transform.position);
        m_Camera.nearClipPlane = l_Distance + m_CameraOffset;
    }
}
