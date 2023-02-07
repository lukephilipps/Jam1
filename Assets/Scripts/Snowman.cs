using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowman : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().WinSequence();
            GameObject.Find("PlayerCameraRoot").transform.rotation = transform.rotation;
            GameObject.Find("PlayerCameraRoot").transform.Rotate(0, 90f, 0);

            other.gameObject.transform.position = transform.position + new Vector3(0, 1f + other.gameObject.transform.localScale.x * .5f, 0);
        }
    }
}
