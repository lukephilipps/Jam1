using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;    // Default game state is: not paused
    public GameObject pauseMenuUI;            // The UI object itself

    public GameObject frozen;
    private PlayerController frozenState;

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))       // Esc key pauses and unpauses game
        {

            frozenState = frozen.GetComponent<PlayerController>();

            if (gameIsPaused == false)              // When game is not paused
            {
                frozenState.freezeMovement = false;
                resumeGame();               // When called, method resumes game
            }
            else                         // When game is paused
            {
                frozenState.freezeMovement = true;
                pauseGame();               // When called, method pauses game
            }
        }
    }

    public void resumeGame()             // This method, when called, changes game from the paused state back to normal
    {
        pauseMenuUI.SetActive(false);  // UI is in its normal state which is off (not active)
        Time.timeScale = 1f;             // Time is set back to normal, which is: 1f
        gameIsPaused = false;              // Game state is: not paused

        frozenState.freezeMovement == false;
    }

    void pauseGame()                    // Method controls what happens when game is in paused state
    {
        pauseMenuUI.SetActive(true);     // UI gets switched from default of off (not active) to on (active)
        Time.timeScale = 0f;             // Freezes the game completely while in pause, goes from normal time of 1f to 0f
        gameIsPaused = true;               // Game state is now in pause
    }

    public void QuitGame()               //  Once called, completely quits game
    {
        Application.Quit();
    }
}
