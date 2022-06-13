using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //-------------------------------------------

    public static LevelManager Instance;

    public int currentLevel { get; private set; } = 1;
    public int totalLevels { get; private set; } = 3;
    public int score { get; private set; }
    public int lives { get; private set; }

    //-------------------------------------------

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        ResetAll();
    }

    //-------------------------------------------

    public void LoadNextLevel ()
    {
        int next = (currentLevel % totalLevels + 1);
        currentLevel++;
        SceneManager.LoadScene("Level 0" + next.ToString());
    }

    //-------------------------------------------

    public void SetDataForNextLevel(int score, int lives)
    {
        this.score = score;
        this.lives = lives;
    }

    //-------------------------------------------

    public void ResetAll()
    {
        currentLevel = 1;
        score = 0;
        lives = 3;
    }

    //-------------------------------------------
}
