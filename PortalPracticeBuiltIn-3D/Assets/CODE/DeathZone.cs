using System.Net.Sockets;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    //GameController m_gameController;
    //PlayerHealth player;
    private void Start()
    {
    //m_gameController= FindObjectOfType<GameController>();
    //    player = FindObjectOfType<PlayerHealth>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //m_gameController.GameOverUI();
            
            Debug.Log("Muerte");
        }

    }
}
