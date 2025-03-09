using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretAreaScript : MonoBehaviour
{
    public float fadeDuration = 1f;

    SpriteRenderer spriteRenderer;
    Color hiddenColour;
    Coroutine currentCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        hiddenColour = spriteRenderer.color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(FadeSprite(true));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(FadeSprite(false));
        }
    }

    private IEnumerator FadeSprite(bool fadeOut)
    {
        Color startColor = spriteRenderer.color;
        Color targetColor = fadeOut ? new Color(hiddenColour.r, hiddenColour.g, hiddenColour.b, 0f) : hiddenColour;
        float timeFading = 0f;

        while (timeFading < fadeDuration)
        {
            spriteRenderer.color = Color.Lerp(startColor, targetColor, timeFading / fadeDuration);
            timeFading += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = targetColor;
    }
}
