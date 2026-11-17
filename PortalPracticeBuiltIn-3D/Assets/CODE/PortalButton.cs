using UnityEngine;
using UnityEngine.Events;

public class PortalButton : MonoBehaviour
{
    public UnityEvent m_Event;
    public string m_Interactor = "Player";

    public AudioManager m_AudioManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(m_Interactor))
        {
            m_AudioManager.PlaySounds(m_AudioManager.ButtonActivationClip);
            m_Event.Invoke();
        }
    }
}
