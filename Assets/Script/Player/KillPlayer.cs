using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillPlayer : MonoBehaviour
{
    public GameObject player;
    public Transform respawnPoint;
    public ParticleSystem[] smokeParticle; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Die();
        }
    }

    void Die()
    {
        StartCoroutine(RespawnWithEffects(5f));
    }

    IEnumerator RespawnWithEffects(float particleDuration)
    {
        player.transform.position = respawnPoint.position;

        foreach (var particle in smokeParticle)
        {
            particle.Play();
        }

        yield return new WaitForSeconds(particleDuration);
        player.transform.position = respawnPoint.position;

        foreach (var particle in smokeParticle)
        {
            particle.Stop();
        }


    }
}
