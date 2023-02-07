using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void RestartLevel()
    {
        //do animation
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
