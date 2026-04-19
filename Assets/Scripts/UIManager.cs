using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_TimerText;
    [SerializeField] GameObject m_TipUI;
    [SerializeField] TextMeshProUGUI m_TipText;
    [SerializeField] TextMeshProUGUI m_ScoreText;

    [Header("Game Over Screen Variables")]
    [SerializeField] GameObject m_GameOverScreen;
    [SerializeField] TextMeshProUGUI m_CurrentScoreText;
    [SerializeField] TextMeshProUGUI m_HighScoreText;
    [SerializeField] TextMeshProUGUI m_TotalOrdersServedText;
    [SerializeField] GameObject m_NewBestUI;

    [Header("Controls Screen")]
    [SerializeField] GameObject m_ControlsScreen;

    private void Start()
    {
        m_GameOverScreen.SetActive(false);
        m_ControlsScreen.SetActive(true);
    }

    public void UpdateTimerText(float a_TimeLeft) 
    {
        int minutes = Mathf.FloorToInt(a_TimeLeft / 60);
        int seconds = Mathf.FloorToInt(a_TimeLeft % 60);
        string timerText = string.Format("{0:00}:{1:00}", minutes, seconds);
        m_TimerText.text = timerText;
    }

    public void GiveInteractionTip(bool a_Show, BaseStation a_Item = null) {
        m_TipUI.SetActive(a_Show);
        if (a_Show)
        {
            m_TipText.text = "Press <size=60><color=yellow>'E'" +
                "</color></size> to interact with " + a_Item.gameObject.name;
        }
    }

    public void UpdateScoreText(int a_Score) 
    {
        m_ScoreText.text = $"score:<size=60>{a_Score}</size>";
    }

    public void ShowGameOverScreen(int a_Score, int a_HiScore) 
    {
        m_GameOverScreen.SetActive(true);
        m_CurrentScoreText.text = a_Score.ToString();
        m_HighScoreText.text = a_HiScore.ToString();
        m_TotalOrdersServedText.text = GameManager.instance.TotalOrdersServed.ToString();

        m_NewBestUI.SetActive(a_Score > a_HiScore);
    }

    public void RetryGame() 
    {
        SceneManager.LoadScene(0);
    }

    public void StartGame() 
    {
        m_ControlsScreen.SetActive(false);
        GameManager.instance.StartGame();
    }
}
