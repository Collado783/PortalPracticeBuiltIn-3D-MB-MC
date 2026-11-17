
using UnityEngine;

public class RefractionCube : MonoBehaviour
{
    public LineRenderer m_Laser;
    bool disableLaserOnNextFrame = true;

    public float m_MaxDistance;
    public LayerMask m_LayerMask;

    GameController m_gameController;

    public void Start()
    {
        m_gameController = FindObjectOfType<GameController>();

    }
    void Update()
    {
        if (disableLaserOnNextFrame)
        {
            m_Laser.gameObject.SetActive(false);
        }

        disableLaserOnNextFrame = true;
    }

    public void Refract()
    {
        if (!disableLaserOnNextFrame)
            return;

        disableLaserOnNextFrame = false;

        Vector3 l_Position = Vector3.forward * m_MaxDistance;

        Ray l_Ray = new Ray(m_Laser.transform.position, m_Laser.transform.forward);

        if (Physics.Raycast(l_Ray, out RaycastHit l_RaycastHit, m_MaxDistance, m_LayerMask, QueryTriggerInteraction.Ignore))
        {
            l_Position = Vector3.forward * l_RaycastHit.distance;

            RefractionCube l_RefractionCube = l_RaycastHit.collider.GetComponent<RefractionCube>();
            if (l_RaycastHit.collider.CompareTag("Player"))
            {
                m_gameController.GameOverUI();
            }
            if (l_RefractionCube != null)
            {
                l_RefractionCube.Refract();
            }
            if (l_RaycastHit.collider.CompareTag("LaserDoorButton"))
            {
                l_RaycastHit.collider.GetComponent<LaserDoorButton>().ReceiveLaser();
            }
        }

        m_Laser.gameObject.SetActive(true);
        m_Laser.SetPosition(1, l_Position);
    }
}
