using UnityEngine;

public class Portal : MonoBehaviour
{
    public Camera m_Camera;
    public Transform m_OtherPortalTransform;
    public Portal m_OtherPortal;
    public float m_CameraOffSet = 0.5f;
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
}
