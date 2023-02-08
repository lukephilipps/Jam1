using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;    // Default game state is: not paused
    public GameObject pauseMenuUI;            // The UI object itself

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameObject.Find("GameManager").GetComponent<GameManager>().canPause)       // Esc key pauses and unpauses game
        {


            if (gameIsPaused)              // When game is not paused
            {

                ResumeGame();               // When called, method resumes game
            }
            else                         // When game is paused
            {

                PauseGame();               // When called, method pauses game
            }
        }
    }

    public void ResumeGame()             // This method, when called, changes game from the paused state back to normal
    {
        GameObject.Find("Player").GetComponent<PlayerController>().freezeMovement = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenuUI.SetActive(false);  // UI is in its normal state which is off (not active)
        Time.timeScale = 1f;             // Time is set back to normal, which is: 1f
        gameIsPaused = false;              // Game state is: not paused

    }

    void PauseGame()                    // Method controls what happens when game is in paused state
    {
        GameObject.Find("Player").GetComponent<PlayerController>().freezeMovement = true;
        Cursor.lockState = CursorLockMode.None;
        pauseMenuUI.SetActive(true);     // UI gets switched from default of off (not active) to on (active)
        Time.timeScale = 0f;             // Freezes the game completely while in pause, goes from normal time of 1f to 0f
        gameIsPaused = true;               // Game state is now in pause
    }

    public void RetryGame()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().RestartLevel();
        ResumeGame();
       
    }

    public void QuitGame()               //  Once called, completely quits game
    {
        Application.Quit();
    }
}
