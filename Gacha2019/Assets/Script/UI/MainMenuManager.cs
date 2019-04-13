using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> m_SetActiveObjectsToSwitch = null;

    public void PlayClicked()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void CreditsClicked()
    {
        for (int i = 0; i < m_SetActiveObjectsToSwitch.Count; i++)
        {
            bool isActive = m_SetActiveObjectsToSwitch[i].activeInHierarchy;
            m_SetActiveObjectsToSwitch[i].SetActive(!isActive);
        }
    }    

    public void QuitClicked()
    {
        Application.Quit();
    }
}
