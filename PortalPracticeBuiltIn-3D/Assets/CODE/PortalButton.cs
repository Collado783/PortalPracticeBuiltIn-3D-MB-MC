using UnityEngine;
using UnityEngine.Events;

public class PortalButton : MonoBehaviour
{
    public UnityEvent m_Event;
    public string m_Interactor = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(m_Interactor))
        {
            m_Event.Invoke();
        }
    }
}
