using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    List<int> m_ButterflyObjectiveCount = null;

    [SerializeField]
    Text m_ButterflyText = null;

    int m_CurrentLayer = 0;

    int m_CurrentButterflyKilled = 0;

    bool isGameOver = false;

    float m_TargetTimeScale = 1f;

    float m_TimeScaleDuration = 0f;

    //public GameObject gameoverPlaceholder;


    void Start()
    {

    }

    void Update()
    {
        if (isGameOver)
        {
            Restart();
        }
        else
        {
            if (m_TimeScaleDuration > 0f)
            {
                m_TimeScaleDuration -= Time.deltaTime;
                Time.timeScale = Mathf.Lerp(1f, m_TargetTimeScale, 4f * (-Mathf.Pow(m_TimeScaleDuration, 2f) + m_TimeScaleDuration));
            }
            else if (Time.timeScale != 1f)
            {
                Time.timeScale = 1f;
            }
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
        }
    }

    public bool IsButterflyObjectiveDone()
    {
        m_CurrentButterflyKilled++;
        if (m_CurrentButterflyKilled >= m_ButterflyObjectiveCount[m_CurrentLayer])
        {
            m_CurrentLayer++;
            m_CurrentButterflyKilled = 0;
            m_ButterflyText.text = "Butterfly : " + m_CurrentButterflyKilled + " / " + m_ButterflyObjectiveCount[m_CurrentLayer];
            return true;
        }
        m_ButterflyText.text = "Butterfly : " + m_CurrentButterflyKilled + " / " + m_ButterflyObjectiveCount[m_CurrentLayer];
        return false;
    }


    public void GameOver(GameObject player)
    {
        isGameOver = true;
        player.GetComponent<Player>().enabled = false;
        //Instantiate(gameoverPlaceholder, transform.position, Quaternion.identity);
    }

    public void Restart()
    {
        Debug.LogWarning("Restart !");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ChangeTimeScale(float _TimeScale, float _Duration)
    {
        m_TargetTimeScale = _TimeScale;
        m_TimeScaleDuration = _Duration;
    }

    //SINGLETON________________________________________________________________________________________________
    private static GameManager s_Instance = null;

    // This defines a static instance property that attempts to find the manager object in the scene and
    // returns it to the caller.
    public static GameManager instance
    {
        get
        {
            if (s_Instance == null)
            {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first AManager object in the scene.
                s_Instance = FindObjectOfType(typeof(GameManager)) as GameManager;
            }

            // If it is still null, create a new instance
            if (s_Instance == null)
            {
                Debug.Log("error");
                GameObject obj = new GameObject("Error");
                s_Instance = obj.AddComponent(typeof(GameManager)) as GameManager;
            }

            return s_Instance;
        }
    }
}
