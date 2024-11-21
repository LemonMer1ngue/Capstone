using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractBox : MonoBehaviour
{
    public int idBox;
    public bool beingPushed; 
    private float xPos;
    public bool hasBeenActivated =  false;
    void Start()
    {
        xPos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        switch (beingPushed)
        {
            case false:
                transform.position = new Vector3(xPos, transform.position.y); break;
            case true:
                xPos = transform.position.x; break;
        }
    }
}
