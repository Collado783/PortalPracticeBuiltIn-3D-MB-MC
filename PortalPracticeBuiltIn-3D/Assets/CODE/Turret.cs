using UnityEngine;

public class Turret : MonoBehaviour
{
    public LineRenderer m_Laser;
    public float m_MaxDistance;
    public LayerMask m_LayerMask;


    void Update()
    {
        Vector3 l_Postion = Vector3.forward * m_MaxDistance;
        Ray l_Ray = new Ray(m_Laser.transform.position, m_Laser.transform.forward);

        if (Physics.Raycast(l_Ray, out RaycastHit l_RaycastHit, m_MaxDistance, m_LayerMask.value, QueryTriggerInteraction.Ignore))
        {
            l_Postion = Vector3.forward * l_RaycastHit.distance;
        }

        m_Laser.SetPosition(1, l_Postion);
    }
}
