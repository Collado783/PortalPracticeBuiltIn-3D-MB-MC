using Unity.VisualScripting;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public LineRenderer m_Laser;
    public float m_MaxDistance;
    public LayerMask m_LayerMask;
    public float m_AngleToFall = 30.0f;
    GameController m_gameController;


    public void Start()
    {
        m_gameController = FindObjectOfType<GameController>();

    }
    void Update()
    {bool l_Fallen = Vector3.Dot(transform.up, Vector3.up)<Mathf.Cos(m_AngleToFall*Mathf.Deg2Rad);
        m_Laser.gameObject.SetActive(!l_Fallen);
        if (!l_Fallen)
        {
               Vector3 l_Postion = Vector3.forward * m_MaxDistance;
            Ray l_Ray = new Ray(m_Laser.transform.position, m_Laser.transform.forward);

            if (Physics.Raycast(l_Ray, out RaycastHit l_RaycastHit, m_MaxDistance, m_LayerMask.value, QueryTriggerInteraction.Ignore))
            {
                l_Postion = Vector3.forward * l_RaycastHit.distance;
                if (l_RaycastHit.collider.CompareTag("RefractionCube"))
                {
                    l_RaycastHit.collider.GetComponent<RefractionCube>().Refract();
                }
                if (l_RaycastHit.collider.CompareTag("Player"))
                {
                    m_gameController.GameOverUI();

                }
                if (l_RaycastHit.collider.CompareTag("Turret"))
                {
                    l_RaycastHit.collider.gameObject.SetActive(false);
                     
                }

            }

            m_Laser.SetPosition(1, l_Postion);
        }
       
        
    }
}
