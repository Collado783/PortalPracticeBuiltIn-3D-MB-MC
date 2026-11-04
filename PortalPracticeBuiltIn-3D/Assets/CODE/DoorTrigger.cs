using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public Door door;

    public bool locked = false;

    public bool lockedByScore = false;
    public int requiredScore = 50;
    public Light pointLight;
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        FPSController player = other.GetComponent<FPSController>();

        if (locked)
        {
            if (true)
            {
                locked = false;
                door.OpenDoor();
                pointLight.color = new Color(0f, 1f, 0f);
            }
        }
        else if (lockedByScore)
        {
            if (player != null && player.m_Score >= requiredScore)
            {
                lockedByScore = false;
                door.OpenDoor();
                pointLight.color = new Color(0f, 1f, 0f);
            }
        }
        else
        {
            door.OpenDoor();
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !locked && !lockedByScore)
        {
            door.CloseDoor();
        }
    }
}