using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public Transform m_DestroyObjects;
    public static GameController m_GameController = null;
    public KeyCode m_RestartKeyCode = KeyCode.X;
    FPSController m_Player;
    //InGameUI m_IngameUI;
    public GameObject gameOverUI;
    public bool GameOverScreen = false;

    private void Awake()
    {
        if (m_GameController == null)
        {
            m_GameController = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (gameOverUI != null)
            gameOverUI.SetActive(false);
    }
    static public GameController GetGameController()
    {
        return m_GameController;
    }
    //public void SetIngameUI(InGameUI _IngameUI)
    //{
    //    m_IngameUI = _IngameUI;
    //}
    //public InGameUI GetIngameUI()
    //{
    //    return m_IngameUI;
    //}
    

    IEnumerator RestartGame()
    {
        GameOverScreen = false;

        if (gameOverUI != null)
            gameOverUI.SetActive(false);

        if (m_DestroyObjects != null)
        {
            for (int i = m_DestroyObjects.childCount - 1; i >= 0; i--)
            {
                Destroy(m_DestroyObjects.GetChild(i).gameObject);
            }
        }

        if (m_Player != null)
            m_Player.Restart();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

  
            yield return null;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (GameOverScreen && Input.GetKeyDown(m_RestartKeyCode))
        {
            StartCoroutine(RestartGame());
        }
    }
    public FPSController GetPlayer()
    {
        return m_Player;
    }
    public void SetPlayer(FPSController Player)
    {
        m_Player = Player;
    }
    public void GameOverUI()
    {
        if (GameOverScreen)
            return;

        GameOverScreen = true;

        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
