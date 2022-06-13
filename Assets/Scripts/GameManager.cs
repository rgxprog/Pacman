using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

//-----------------------------------------------

public enum GameState
{
    Menu,
    InGame,
    LevelWin,
    GameOver
}

//-----------------------------------------------

public class GameManager : MonoBehaviour
{
    //-------------------------------------------

    public static GameManager Instance { get; private set; }

    public Pacman pacman;
    public Ghost[] ghosts;
    public Transform pellets;


    public GameObject gameOverPanel;
    public TextMeshProUGUI scoreText, readyToStart, livesText, levelText;

    public int currentLevel { get; private set; } = 1;
    public int totalLevels { get; private set; } = 3;
    public int lives { get; private set; }
    public int score { get; private set; }
    public int ghostMultiplier { get; private set; } = 1;
    public bool stopMovement { get; private set; } = false;

    private AudioManager audioManager;
    private GameState state;

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

        audioManager = FindObjectOfType<AudioManager>();

        currentLevel = LevelManager.Instance.currentLevel;
        lives = LevelManager.Instance.lives;
        score = LevelManager.Instance.score;
    }

    //-------------------------------------------

    private void Start()
    {
        NewGame();
    }

    //-------------------------------------------

    private void Update()
    {
        if (state == GameState.GameOver && Input.GetKeyDown(KeyCode.Return))
            RestartGame();
    }

    //-------------------------------------------
    private void NewGame()
    {
        SetScore(score);
        SetLives(lives);

        ResetObjects();
        StartCoroutine(ReadyToStartTransition());
    }

    //-------------------------------------------

    private IEnumerator ReadyToStartTransition()
    {
        readyToStart.text = "Un momento";
        yield return new WaitForSeconds(2f);

        readyToStart.text = "Inicio";
        audioManager.Play("GameStart");
        yield return new WaitForSeconds(1f);

        state = GameState.InGame;
        yield return new WaitForSeconds(0.5f);

        readyToStart.text = "";
    }

    //-------------------------------------------

    // Resetear fantasmas y pacman
    private void ResetObjects()
    {
        ResetGhostMultiplier();

        foreach (var ghost in ghosts)
        {
            ghost.ResetState();
        }

        pacman.ResetState();

        levelText.text = currentLevel.ToString();
    }

    //-------------------------------------------

    // Para resetear posiciones de pacman y fantasmas
    private void ResetAfterLoseLive()
    {
        ResetObjects();        

        audioManager.Play("GameStart");
        Invoke(nameof(RestartMovement), 1.0f);
    }

    //-------------------------------------------

    public GameState GetState()
    {
        return state;
    }

    //-------------------------------------------

    private void SetScore(int points)
    {
        score = points;
        scoreText.text = score.ToString("N0");
    }

    //-------------------------------------------

    private void SetLives(int quantity)
    {
        lives = quantity;
        livesText.text = lives.ToString();

        if (lives <= 0)
            SetGameOver();
    }

    //-------------------------------------------

    public void PelletEaten(Pellet pellet, string sound = "Pellet" )
    {
        audioManager.Play(sound);
        SetScore(score + pellet.points);
        pellet.gameObject.SetActive(false);

        if (!HasRemainingPellets())
        {
            stopMovement = true;
            state = GameState.LevelWin;
            audioManager.Play("LevelWin");
            Invoke(nameof(NextLevel), 2.5f);
        }
    }

    //-------------------------------------------

    public void PowerPelletEaten(PowerPellet pellet)
    {
        for (int i = 0; i < ghosts.Length; i++)
            ghosts[i].frightened.Enable(pellet.duration);

        PelletEaten(pellet, "PowerPellet");
        audioManager.Play("GhostScared");

        CancelInvoke();
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);
    }

    //-------------------------------------------

    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in pellets)
        {
            if (pellet.gameObject.activeSelf)
                return true;
        }

        return false;
    }

    //-------------------------------------------

    // Si pacman come un fantasma
    public void GhostEaten(Ghost ghost)
    {
        audioManager.Play("GhostEaten");
        int points = ghost.points * ghostMultiplier;
        SetScore(score + points);
        ghostMultiplier++;

        stopMovement = true;
        Invoke(nameof(RestartMovement), 0.3f);
    }

    //-------------------------------------------

    // Para retornar movimiento de pacman y fantasmitas
    private void RestartMovement()
    {
        stopMovement = false;
    }

    //-------------------------------------------

    // Si pacman toca un fantasma (pierde)
    public void PacmanEaten()
    {
        stopMovement = true;
        audioManager.Play("PacmanLost");
        pacman.TouchLiveGhost();
        SetLives(lives - 1);

        if (state != GameState.GameOver)
            Invoke(nameof(ResetAfterLoseLive), audioManager.GetClipLength("PacmanLost"));
    }

    //-------------------------------------------

    private void ResetGhostMultiplier()
    {
        ghostMultiplier = 1;
        audioManager.Stop("GhostScared");
    }

    //-------------------------------------------

    private void SetGameOver()
    {
        state = GameState.GameOver;
        Invoke(nameof(ShowGameOver), 1.5f);
    }

    private void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }

    //-------------------------------------------

    public void RestartGame()
    {
        LevelManager.Instance.ResetAll();
        SceneManager.LoadScene("MainMenu");
    }

    //-------------------------------------------

    private void NextLevel()
    {
        LevelManager.Instance.SetDataForNextLevel(score, lives);
        LevelManager.Instance.LoadNextLevel();
    }

    //-------------------------------------------
}
