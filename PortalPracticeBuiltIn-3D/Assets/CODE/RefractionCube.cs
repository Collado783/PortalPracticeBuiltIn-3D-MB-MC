using UnityEngine;

public class RefractionCube : MonoBehaviour
{
    public LineRenderer m_Laser;
    bool disableLaserOnNextFrame=true;
    public float m_MaxDistance;
    public LayerMask m_LayerMask;
    void Update()
    {
        if (disableLaserOnNextFrame)
        {
            m_Laser.gameObject.SetActive(false);

            disableLaserOnNextFrame = true;
            Debug.Log("Update");
        }
    }
    public void Refract()
    {
        if (!disableLaserOnNextFrame) return;

        disableLaserOnNextFrame = false;
        Vector3 l_Postion = Vector3.forward * m_MaxDistance;
        Ray l_Ray = new Ray(m_Laser.transform.position, m_Laser.transform.forward);
        Debug.Log("Antes del collider");
        if (Physics.Raycast(l_Ray, out RaycastHit l_RaycastHit, m_MaxDistance, m_LayerMask.value, QueryTriggerInteraction.Ignore))
        {
            Debug.Log("EntroAlcollider");
            l_Postion = Vector3.forward * l_RaycastHit.distance;
            if (l_RaycastHit.collider.CompareTag("RefractionCube"))
            {
                l_RaycastHit.collider.GetComponent<RefractionCube>().Refract();
                Debug.Log("Primer if");
            }
            Debug.Log("SalgoDelIf");
            m_Laser.SetPosition(1, l_Postion);
            m_Laser.gameObject.SetActive(true);
        }
    }
}
