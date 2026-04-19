using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreIndicator : MonoBehaviour
{
    [SerializeField] TextMeshPro m_TMP;

    public void Initialize(float a_Score) 
    {
        if(m_TMP == null)
            m_TMP = GetComponent<TextMeshPro>();
        string l_Score = a_Score > 0 ? "+" + a_Score : a_Score.ToString();
        m_TMP.text = l_Score;
        m_TMP.color = a_Score < 0 ? Color.red : Color.green;
        StartCoroutine(FlyAway(2f));
    }

    IEnumerator FlyAway(float a_Duration) 
    {
        float l_Timer = 0.0f;
        Color l_C = m_TMP.color;
        while (l_Timer < a_Duration)
        {
            l_Timer += Time.deltaTime;
            yield return null;
            transform.position += new Vector3(0, Time.deltaTime, 0);
            l_C.a = (1 - l_Timer / a_Duration);
            m_TMP.color = l_C;
        }
        Destroy(gameObject);
    }
}
