using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DeathController : MonoBehaviour
{
    public static DeathController Instance;

    public GameObject respawnAnimObject;
    public Animator anim;
    Vector2 checkpointPos;
    Rigidbody2D playerRb;

    void Start()
    {
        checkpointPos = transform.position;
        playerRb = GetComponent<Rigidbody2D>();
        respawnAnimObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            Die();
        }
    }

    public void UpdateCheckpoint(Vector3 pos)
    {
        checkpointPos = pos;
    } 

    void Die()
    {
        StartCoroutine(Respawn(1f));
    }

    IEnumerator Respawn(float duration)
    {
        playerRb.velocity = new Vector2(0, 0);
        respawnAnimObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        transform.position = checkpointPos;
        anim.SetTrigger("Respawn");
        yield return new WaitForSeconds(duration);
        respawnAnimObject.SetActive(false);
       
       
    }
}
