using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableGate : MonoBehaviour
{
    Vector3 startPos;
    bool shaking;
    public float requiredSize = 1f;

    void Start()
    {
        startPos = transform.position;
        StartCoroutine(SwapShake());
    }

    IEnumerator SwapShake()
    {
        yield return new WaitForSeconds(.09f);
        shaking = !shaking;

        if (shaking) transform.position = startPos + new Vector3(0, 0, -.03f);
        else transform.position = startPos + new Vector3(0, 0, .03f);

        StartCoroutine(SwapShake());
    }

    private void OnCollisionEnter(Collision other)
    {
        // if (other.gameObject.tag == "Player")
        // {
        //     print(other.rigidbody.velocity.magnitude * other.transform.localScale.x);
        // }
        if (other.gameObject.tag == "Player" && other.transform.localScale.x >= requiredSize)
        {
            DestroyGate();
        }
    }

    void DestroyGate()
    {
        Destroy(gameObject);
    }
}
