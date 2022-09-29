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
    public GameObject titleScreen;
    public Button restartButton; 

    public List<GameObject> targetPrefabs;

    private int score;
    private float spawnRate = 2f;
    public bool isGameActive;

    private float spaceBetweenSquares = 2.5f; 
    private float minValueX = -3.75f; //  x value of the center of the left-most square
    private float minValueY = -3.75f; //  y value of the center of the bottom-most square

    private float TimerLeft = 60; //time variable to start countdown from 1minute(60 seconds)
    public bool TimerOn = false; //boolean variable to detect if timer is active or not
    public TextMeshProUGUI TimerText;

    // Start the game, remove title screen, reset score, and adjust spawnRate based on difficulty button clicked
    public void StartGame(int difficulty)
    {
        //activating timer on start 
        TimerOn = true;

        //an integer parameter which will divide the spawnRate and assign its new value as spawnRate, to control the difficulty levels
        spawnRate /= difficulty;
        isGameActive = true;
        StartCoroutine(SpawnTarget());
        score = 0;
        UpdateScore(0);
        titleScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (TimerOn)
        {
            //if time left is greater than zero it will keep working else it will turn off timer (make timer inactive)
            if (TimerLeft > 0)
            {
                TimerLeft -= Time.deltaTime;
                UpdateTimer(TimerLeft);
            }
            else
            {
                TimerLeft = 0;
                TimerOn = false;
            }
        }
    }

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
        scoreText.text = "Score: " + score;//this updates and displays the score after every addition
    }

    // Stop game, bring up game over text and restart button
    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        //setting to true activates the button then allows the player to restart after they lose
        restartButton.gameObject.SetActive(true);
        isGameActive = false;
        TimerOn = false;
    }

    // Restart game by reloading the scene
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void UpdateTimer(float countdown)
    {
        countdown += 1;
        //dividing the timeleft to get integer value
        int minutes = Mathf.FloorToInt(countdown / 1);

        //displaying the time remaining
        TimerText.text = "Timer: " + minutes;
    }
}
