using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal m_OtherPortal;
    public Transform m_OtherPortalTransform;

    public Transform[] m_PortalCorners;

    public bool IsValidPosition(Vector3 position, Vector3 normal, LayerMask mask)
    {
        if (normal == Vector3.zero)
            return false;

        normal.Normalize();

        Vector3 originalPosition = transform.position;
        Quaternion originalRotation = transform.rotation;

        transform.position = position;
        transform.rotation = Quaternion.LookRotation(normal);
        Physics.SyncTransforms(); //Update de fisicas para el raycast, que sino no lo hace bien idk why

        bool allPointsValid = m_PortalCorners != null && m_PortalCorners.Length > 0;

        if (allPointsValid)
        {
            Vector3 n = transform.forward;

            foreach (Transform corner in m_PortalCorners)
            {
                if (corner == null)
                {
                    allPointsValid = false;
                    break;
                }

                Vector3 onPlane = corner.position - n * Vector3.Dot(corner.position - position, n);
                Vector3 origin = onPlane + n * 0.1f;

                if (!Physics.Raycast(origin, -n, 0.3f, mask, QueryTriggerInteraction.Ignore))
                {
                    allPointsValid = false;
                    break;
                }
            }
        }

        transform.position = originalPosition;
        transform.rotation = originalRotation;
        Physics.SyncTransforms();

        return allPointsValid;
    }
}