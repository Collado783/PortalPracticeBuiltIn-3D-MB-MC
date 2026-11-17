using UnityEngine;

public class LaserDoorButton : MonoBehaviour
{
    public Door m_Door;

    void Start()
    {
      
    }

    public void ReceiveLaser()
    {
        m_Door.OpenDoor();
    }
}