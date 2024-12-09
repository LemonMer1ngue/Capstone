using UnityEngine;

public class RandomTransitionSelector : MonoBehaviour
{
    public Animator animator;
    public int minTransitionIndex = 1;
    public int maxTransitionIndex = 3;

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (animator != null)
        {
            // Pilih transisi secara acak
            int randomIndex = Random.Range(minTransitionIndex, maxTransitionIndex + 1);

            // Set parameter di Animator untuk memilih transisi
            animator.SetInteger("TransitionIndex", randomIndex);
        }
        else
        {
            Debug.LogWarning("Animator not assigned or found!");
        }
    }
}
