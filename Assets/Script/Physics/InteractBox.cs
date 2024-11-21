using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractBox : MonoBehaviour
{
    public int idBox;
    public bool beingPushed; 
    private float xPos;
    public bool hasBeenActivated =  false;
    [SerializeField] private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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



