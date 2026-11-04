using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CheckpointZone : MonoBehaviour
{
    public Transform m_RespawnPoint;
    public bool m_OnlyOnce = true;

    bool m_Activated = false;

    void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (m_Activated && m_OnlyOnce)
            return;

        Portal l_Player = other.GetComponentInParent<Portal>();
        if (l_Player == null)
            return;

        Transform l_Point = m_RespawnPoint != null ? m_RespawnPoint : transform;
        l_Player.SetCheckpoint(l_Point.position, l_Point.rotation);
        m_Activated = true;
    }
}