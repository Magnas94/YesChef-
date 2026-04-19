using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float m_GameDuration = 180f;

    private float m_Timer;
    private int m_Score;
    private int m_HighScore;

    public static GameManager instance;
    [SerializeField] UIManager m_UIManager;
    [SerializeField] bool m_GameRunning = false;

    [SerializeField] CustomerStation[] m_AllCustomerStations;

    public int TotalOrdersServed = 0;

    private void Awake()
    {
        instance = this;
    }

    private void OnDestroy()
    {
        instance = null;
    }

    void Start()
    {
        m_GameRunning = false;
        m_Timer = m_GameDuration;
        m_HighScore = PlayerPrefs.GetInt("HighScore", 0);
        m_UIManager.UpdateScoreText(0);
    }

    void Update()
    {
        if (!m_GameRunning)
            return;

        m_Timer -= Time.deltaTime;
        m_UIManager.UpdateTimerText(m_Timer);
        if (m_Timer <= 0)
        {
            EndGame();
        }
    }

    public void AddScore(int amount)
    {
        m_Score += amount;

        if (m_Score > m_HighScore)
        {
            m_HighScore = m_Score;
            PlayerPrefs.SetInt("HighScore", m_HighScore);
        }
        m_UIManager.UpdateScoreText(m_Score);
    }

    public void StartGame()
    {
        m_GameRunning = true;
        TotalOrdersServed = 0;
    }

    void EndGame()
    {
        Debug.Log("Game Over");
        m_GameRunning = false;
    }
}