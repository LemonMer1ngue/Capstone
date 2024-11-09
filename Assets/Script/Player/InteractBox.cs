using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractBox : MonoBehaviour
{
    public bool beingPushed; 
    private float xPos;
    void Start()
    {
        xPos = transform.position.x;

    }
    void Update()
    {
        switch (beingPushed)
        {
            case false:
                transform.position = new Vector3(xPos, transform.position.y);
                break;
            case true:
                xPos = transform.position.x;
                break;
        }
    }
}



