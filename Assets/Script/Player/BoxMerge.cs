using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMerge : MonoBehaviour
{
    int ID;
    public GameObject MergedObject;
    Transform Block1;
    Transform Block2;
    public float Distance;
    public float MergeSpeed;
    bool CanMerge;
    private PlayerMovement playerMovement;
    // Start is called before the first frame update
    void Start()
    {
        ID = GetInstanceID();
        playerMovement = FindObjectOfType<PlayerMovement>();

    }
    private void FixedUpdate()
    {
        MoveTowards();
    }
    public void MoveTowards()
    {
        if (CanMerge)
        {
            transform.position = Vector2.MoveTowards(Block1.position, Block2.position, MergeSpeed);

            float distance = Vector2.Distance(Block1.position, Block2.position);

            switch (distance < Distance)
            {
                case true:
                    switch (ID < Block2.gameObject.GetComponent<BoxMerge>().ID)
                    {
                        case true:
                            return;
                        case false:
                            Debug.Log($"SENDING MESSAGE FROM {gameObject.name} With The ID Number of {ID}");
                            GameObject mergedBox = Instantiate(MergedObject, transform.position, Quaternion.identity);

                            Destroy(Block2.gameObject);
                            Destroy(gameObject);
                            break;
                    }
                    break;
            }
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("InteractAble"))
        {
            if (collision.gameObject.GetComponent<SpriteRenderer>().sprite == GetComponent<SpriteRenderer>().sprite)
            {
                Block1 = transform;
                Block2 = collision.transform;
                CanMerge = true;

                FixedJoint2D fixedJointBlock2 = collision.gameObject.GetComponent<FixedJoint2D>();
                switch (fixedJointBlock2)
                {
                    case null:
                        break;
                    default:
                        Destroy(fixedJointBlock2);
                        break;
                }
                FixedJoint2D fixedJointBlock1 = GetComponent<FixedJoint2D>();
                switch (fixedJointBlock2)
                {
                    case null:
                        break;
                    default:
                        Destroy(fixedJointBlock1);
                        break;
                }

                Destroy(collision.gameObject.GetComponent<Rigidbody2D>());
                Destroy(GetComponent<Rigidbody2D>());
            }
        }
    }
}
