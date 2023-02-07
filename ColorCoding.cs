using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorCoding : MonoBehaviour
{
    public Text msgText;
    private int totalBlocks;
    private int coloredBlocks = -1;
   
    // Start is called before the first frame update
    void Start()
    {
        totalBlocks = GameObject.FindGameObjectsWithTag("Box").Length;
    }

    public void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Box")
        {
            
            col.gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
            coloredBlocks = coloredBlocks + 1;
        }
        else
        {
            // Destroy(col.gameObject.);
        }
        if(coloredBlocks == totalBlocks)
        {
            print("colored all boxes");
            // msgText.text = "Colored!!!!";
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    
}
