using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Portal : MonoBehaviour
{
    public Camera m_Camera;
    public Transform m_OtherPortalTransform;
    public Portal m_OtherPortal;
    public float m_CameraOffSet = 0.5f;
    public List<Transform> m_ValidPositions;
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
        for (int i=0; i<m_ValidPositions.Count; i++)
        {

        }
        return true;
    }

    public void SetCheckpoint(Vector3 pos, Quaternion rot)
    {
        Debug.Log("Checkpoint setted");
    }
}
