using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageFill : MonoBehaviour
{
    public Image image; 
    public float fillDuration = 0.5f; 

    void Start()
    {
        StartCoroutine(FillImageEffect());
    }

    IEnumerator FillImageEffect()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fillDuration)
        {
            image.fillAmount = Mathf.Lerp(0, 1, elapsedTime / fillDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        image.fillAmount = 1;
    }
}
