using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    //-------------------------------------------

    public void StartGame()
    {
        SceneManager.LoadScene("Level 01");
    }

    //-------------------------------------------

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            Destroy(GameObject.Find("GameManager"));
            Destroy(GameObject.Find("AudioManager"));
        }
    }

    //-------------------------------------------

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (SceneManager.GetActiveScene().name == "MainMenu")
                StartGame();
        }
    }

    //-------------------------------------------
}
