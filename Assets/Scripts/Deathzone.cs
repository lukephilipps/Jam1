using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deathzone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().RestartLevel();
        }
    }
}
