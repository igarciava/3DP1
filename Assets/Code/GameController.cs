using UnityEngine;

public class GameController : MonoBehaviour
{
    static GameController m_GameController = null;
    FPPlayerController m_Player;
    float m_PlayerLife;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public static GameController GetGameController()
    {
        if(m_GameController == null)
        {
            m_GameController = new GameObject("GameController").AddComponent<GameController>();
            GameControllerData l_GameControllerData = Resources.Load <GameControllerData>("GameControllerData");
            m_GameController.m_PlayerLife = l_GameControllerData.m_lifes;
            Debug.Log("Data loaded with life" + m_GameController.m_PlayerLife);
        }
        return m_GameController;
    }
    public static void DestroySingleton()
    {
        if(m_GameController != null)
        {
            GameObject.Destroy(m_GameController.gameObject);
        }
        m_GameController = null;
    }

    public void SetPLayerLife(float PlayerLife)
    {
        m_PlayerLife = PlayerLife;
    }
    public float GetPlayerLife()
    {
        return m_PlayerLife;
    }

    public FPPlayerController GetPlayer()
    {
        return m_Player;
    }
    public void SetPlayer(FPPlayerController Player)
    {
        m_Player = Player;
    }
    public void RestartGame()
    {
        m_Player.RestartGame();
    }
}
