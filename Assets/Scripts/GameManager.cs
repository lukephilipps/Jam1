using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Animator fadeOut;
    public AudioSource audioSource;
    public AudioClip music;
    public AudioClip victory;

    public bool canPause = true;

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Manager");

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void WinLevel()
    {
        audioSource.PlayOneShot(victory, .8f);
        canPause = false;
        StartCoroutine(LoadNextLevel());
    }

    public void RestartLevel()
    {
        fadeOut.SetTrigger("FadeOut");
        canPause = false;
        StartCoroutine(Restart());
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(2f);
        fadeOut.SetTrigger("FadeOut");
        yield return new WaitForSeconds(2f);
        canPause = true;
        if (SceneManager.GetActiveScene().name == "Stage1")
        {
            SceneManager.LoadScene("Stage2");
        }
    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(2f);
        canPause = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
