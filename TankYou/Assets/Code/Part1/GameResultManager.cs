using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameResultManager : MonoBehaviour
{
  
    public TMP_Text playerScoreText; // Reference to the player score text
    public TMP_Text enemyScoreText; // Reference to the enemy score text
    public TMP_Text timerText; // Reference to the timer text
    public GameObject winImage; // Reference to the win image GameObject
    public GameObject loseImage; // Reference to the lose image GameObject

    private int playerScore;
    private int enemyScore;

    void Start()
    {
        playerScore = 0;
        enemyScore = 0;
    }

    public void UpdateScores(int newPlayerScore, int newEnemyScore)
    {
        playerScore = newPlayerScore;
        enemyScore = newEnemyScore;
        playerScoreText.text = playerScore.ToString();
        enemyScoreText.text = enemyScore.ToString();

        int timerValue;
        if (int.TryParse(timerText.text, out timerValue))
        {
            if (timerValue <= 0)
            {
                GameOver(false); // Player loses if timer reaches 0
            }
        }

        if (playerScore >= 5)
        {
            GameOver(true); // Player wins
        }
        else if (enemyScore >= 5)
        {
            GameOver(false); // Player loses
        }
    }

    void GameOver(bool playerWins)
    {
        if (playerWins)
        {
            winImage.SetActive(true); // Show win image
        }
        else
        {
            loseImage.SetActive(true); // Show lose image
        }

        // Wait for a moment before changing scene
        Invoke("ChangeScene", 3f);
    }

    void ChangeScene()
    {
        
        SceneManager.LoadScene(0);
    }
}

