using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RunGame : MonoBehaviour
{
    public float countdownDuration = 10f; // Duration of countdown in seconds
    public TextMeshPro countdownText; // Reference to the UI text for displaying countdown

    private bool gameStarted = false;
    private float countdownTimer;

    void Start()
    {
        countdownTimer = countdownDuration;
        Time.timeScale = 0f; // Pause the game at start
    }

    void Update()
    {
        if (!gameStarted)
        {
            countdownTimer -= Time.unscaledDeltaTime;
            UpdateCountdownText();

            if (countdownTimer <= 0f)
            {
                gameStarted = true;
                StartGame();
            }
        }
    }

    void StartGame()
    {
        Time.timeScale = 1f; // Resume the game
        DisablePlayerAndEnemyActions();
    }

    void DisablePlayerAndEnemyActions()
    {
        // Implement code to disable player and enemy actions here
        // For example, you can disable player movement, shooting, etc.
        // Similarly, disable enemy actions.

        // You can do this by accessing player and enemy scripts/components and toggling their active states.
    }

    void UpdateCountdownText()
    {
        int seconds = Mathf.CeilToInt(countdownTimer);
        countdownText.text = "Game starts in: " + seconds.ToString();
    }
}