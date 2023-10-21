using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPiece : MonoBehaviour
{
    public bool isColored = false;


    // change the color of ground when we touch it
    public void Colored(Color color)
    {
        
        GetComponent<MeshRenderer>().material.color = color;
        isColored = true;
        GameManager.singleton.CheckComplete();



        //FindObjectOfType<GameManager>().CheckComplete();

    }
}
