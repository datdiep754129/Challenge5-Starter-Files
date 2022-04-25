using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerX : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI livesTime;

    public GameObject titleScreen;
    public GameObject pauseUI;
    public Button restartButton;
    public Button resumeButton;

    public List<GameObject> targetPrefabs;


    private int score;
    public int lives;
    private float spawnRate = 1.5f;

    public bool timeOn = false;
    public bool isGameActive;
    public bool gameIsPaused;
    public int currentTime;
    public TextMeshProUGUI timeOver;
    private float spaceBetweenSquares = 2.5f; 
    private float minValueX = -3.75f; //  x value of the center of the left-most square
    private float minValueY = -3.75f; //  y value of the center of the bottom-most square

    // Start the game, remove title screen, reset score, and adjust spawnRate based on difficulty button clicked
    public void StartGame(int difficulty)
    {
        spawnRate /= difficulty;
        isGameActive = true;
        StartCoroutine(SpawnTarget());
        score = 0;
        UpdateScore(0);
        StartCoroutine(CountDownTime());
        titleScreen.SetActive(false);
        pauseUI.SetActive(false);
        UpdateLives(lives);
    }

    void Update()
    {
        if(isGameActive)
        {
            if (!gameIsPaused)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    pauseUI.gameObject.SetActive(true);
                    gameIsPaused = !gameIsPaused;
                    PauseGame();
                  
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    pauseUI.gameObject.SetActive(false);
                    gameIsPaused = !gameIsPaused;
                    PauseGame();
                }
            }
            
        }

        if (isGameActive && !true)
        {
            Debug.Log("Success12312");
        }
    }

    /**
    void OnGUI()
    {
        Vector3 point = new Vector3();
        Event currentEvent = Event.current;
        Vector2 mousePos = new Vector2();

        mousePos.x = currentEvent.mousePosition.x;
        mousePos.y = camera.pixelHeight - currentEvent.mousePosition.y;

        point = camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, camera.nearClipPlane));

        GUILayout.BeginArea(new Rect(20, 20, 250, 120));
        GUILayout.Label("Screen pixels: " + camera.pixelWidth + ":" + camera.pixelHeight);
        GUILayout.Label("Mouse position: " + mousePos);
        GUILayout.Label("World position: " + point.ToString("F3"));
        GUILayout.EndArea();
    }
    **/


    // While game is active spawn a random target
    IEnumerator SpawnTarget()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, targetPrefabs.Count);

            if (isGameActive)
            {
                Instantiate(targetPrefabs[index], RandomSpawnPosition(), targetPrefabs[index].transform.rotation);
            }
            
        }
    }

    // Generate a random spawn position based on a random index from 0 to 3
    Vector3 RandomSpawnPosition()
    {
        float spawnPosX = minValueX + (RandomSquareIndex() * spaceBetweenSquares);
        float spawnPosY = minValueY + (RandomSquareIndex() * spaceBetweenSquares);

        Vector3 spawnPosition = new Vector3(spawnPosX, spawnPosY, 0);
        return spawnPosition;

    }

    // Generates random square index from 0 to 3, which determines which square the target will appear in
    int RandomSquareIndex()
    {
        return Random.Range(0, 4);
    }

    // Update score with value from target clicked
    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score + "+ score;
    }

    public void UpdateLives(int lives)
    {
        livesTime.text = "Lives: " + lives;
    }


    IEnumerator CountDownTime()
    {
        while (isGameActive)
        {
            if (currentTime >= 0)
            {
                timeOver.text = "Time: " + currentTime;
                yield return new WaitForSeconds(1);
                currentTime--;
            }
            else
            {
                GameOver();
            }
        }
    }

    // Stop game, bring up game over text and restart button
    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        isGameActive = false;
    }


    public void PauseGame()
    {
        if (gameIsPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    // Restart game by reloading the scene
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
