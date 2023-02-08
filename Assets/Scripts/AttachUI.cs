using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachUI : MonoBehaviour
{
    void Start()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().fadeOut = GetComponent<Animator>();
    }
}
