using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableObject : MonoBehaviour
{
    [SerializeField] float minSizeRequired = 1f;
    [SerializeField] float growAmount = 1f;

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<PlayerController>().curSize >= minSizeRequired)
        {
            other.gameObject.GetComponent<PlayerController>().GrowAmount(growAmount);
            transform.SetParent(other.gameObject.transform);
            gameObject.GetComponent<Collider>().enabled = false;
            Destroy(this);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<PlayerController>().curSize >= minSizeRequired)
        {
            other.gameObject.GetComponent<PlayerController>().GrowAmount(growAmount);
            transform.SetParent(other.gameObject.transform);
            gameObject.GetComponent<Collider>().enabled = false;
            Destroy(this);
        }
    }
}
