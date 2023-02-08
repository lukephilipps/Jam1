using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorCoding : MonoBehaviour
{

    private int totalBlocks;
    private int coloredBlocks = -1;
    [SerializeField] Material floorMat;
    [SerializeField] Material coloredMat;
   
    // Start is called before the first frame update
    void Start()
    {
        totalBlocks = GameObject.FindGameObjectsWithTag("Box").Length;
    }

    public void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Box")
        {
            
            col.gameObject.GetComponent<MeshRenderer>().material = coloredMat;
            coloredBlocks = coloredBlocks + 1;
        }
        else if(col.gameObject.tag == "Destroy")
        {
            Destroy(col.gameObject, 0.5f);
        }
        if(coloredBlocks == totalBlocks)
        {
            print("colored all boxes");
            // msgText.text = "Colored!!!!";
        }
    }
    

    
}
