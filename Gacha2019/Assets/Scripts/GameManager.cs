using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    bool isGameOver=false;

    public GameObject gameoverPlaceholder;


    void Start()
    {
        
    }

    void Update()
    {
        if(isGameOver)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Restart();
            }
        }
    }


    public void GameOver(GameObject player)
    {
        isGameOver = true;
        player.GetComponent<Player>().enabled = false;
        Instantiate(gameoverPlaceholder, transform.position, Quaternion.identity);
    }

    public void Restart()
    {
        SceneManager.LoadScene(2);
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
