using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int lives;
    private int score;
    private int countDownTime;

    public TextMeshProUGUI scoreUI;
    public TextMeshProUGUI startCountDown;
    public TextMeshProUGUI lifeSpan;
    public PlayerController _playerController;
    public EnemySpawner _enemySpawner;
    public EnemyController _enemyController;
    public AudioClip countDownClip;
    public GameObject startScreen;
    public GameObject restartScreen;
    public GameObject continueScreen;
    public GameObject gameStatScreen;
    public GameObject player;
    public GameObject portal;

    public int maxScore;

    // Called when gameobject is created
    private void Awake()
    {
        var gms = GameObject.FindObjectsOfType<GameManager>();
        if (gms.Length > 1)
        {
            Destroy(gms[0].gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
        startScreen.SetActive(true);
        gameStatScreen.SetActive(true); 
        restartScreen.SetActive(false);
        continueScreen.SetActive(false);
    }

    // Restarts the game manager values
    private void Start()
    {
        _playerController.enabled = false;
        _enemySpawner.enabled = false;
        lives = 5;
        lifeSpan.text = "x" + lives.ToString();
        score = 0;
        scoreUI.text = "SCORE: " + score;
        countDownTime = 3;
        _playerController.UpdateBulletLives(30);
        player.transform.position = new Vector2(0, 0);
    }

    // Calls StartGame function for countdown & start
    public void GameInitiation()
    {
        StartCoroutine(StartGame());
    }

    // Adding shot enemy points to our score
    public void AddScore(int points)
    {
        score += points;
        scoreUI.text = "SCORE: " + score;
    }

    // Player calls UpdateLives if triggered
    public void UpdateLives(int lifePoint)
    {
        if (lives > 0)
        {
            lives += lifePoint;
            _playerController.UpdateBulletLives(30);
            lifeSpan.text = "x" + lives.ToString();
        }
    }

    // Starts the game counting down and enabling controller scripts
    private IEnumerator StartGame()
    {
        AudioSource.PlayClipAtPoint(countDownClip, transform.position, 1f);
        // countdown
        for (int i = countDownTime; i > 0; i--)
        {
            startCountDown.text = i.ToString()+"!";
            yield return new WaitForSeconds(1f);
        }
        startCountDown.color = Color.green;
        startCountDown.text =  "Go!";
        yield return new WaitForSeconds(1f);
        startCountDown.enabled = false;
        _playerController.enabled = true;
        _enemySpawner.enabled = true;
        countDownTime = 3;
    }

    // Update function for every frame
    private void Update()
    {
        if (lives <= 0)
        {
            StartCoroutine(WaitSeconds(2));
            _playerController.enabled = false;
            _enemySpawner.enabled = false;
            startScreen.SetActive(false);
            gameStatScreen.SetActive(false);
            restartScreen.SetActive(true);
        }
        if(score >= maxScore)
        {
            portal.SetActive(true);
        }

#if !Unity_WEBGL
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
#endif
    }

    public void RestartScene()
    {
        Start();
        StartCoroutine(WaitForSceneLoad(SceneManager.GetActiveScene().name));
    }

    private IEnumerator WaitForSceneLoad(string sceneName)
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator WaitSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}

