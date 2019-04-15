using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForMainMenu : MonoBehaviour
{

    public MainMenuManager manager = null;
    public float timer = 3f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            manager.ChangeToMainMenu(gameObject);
        }
    }
}
