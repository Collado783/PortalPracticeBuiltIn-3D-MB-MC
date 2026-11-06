using UnityEngine;

public class CompanionSpawner : MonoBehaviour
{
    public Transform m_SpawnPoint;
    public GameObject m_CompanionCube;

    public void Execute() 
    {
        GameObject l_GameObject = GameObject.Instantiate(m_CompanionCube);
        l_GameObject.transform.position = m_SpawnPoint.position;
    }
}

