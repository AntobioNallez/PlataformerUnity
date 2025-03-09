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

    /// <summary>
    /// Cuando el jugador entra en contacto con el area el efecto de "invisibilidad" se deshace revalando la zona
    /// Este efecto se ha logrado creando un fondo igual al del juego y escondiendo cosas detras
    /// </summary>
    /// <param name="collision"></param>
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

    /// <summary>
    /// Al salir la pared invisible vuelve a ocultarse
    /// </summary>
    /// <param name="collision"></param>
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
            //El color pase a ser del color inicial al color objetivo
            //En este caso el color inicial es el "invisible"
            //Y el color objetivo es aquel que permita ver dentro
            spriteRenderer.color = Color.Lerp(startColor, targetColor, timeFading / fadeDuration);
            timeFading += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = targetColor;
    }
}
