using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float fallWait = 1.2f;
    public float destroyWait = 1f;

    bool isFalling;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(!isFalling && collision.gameObject.CompareTag("Player")) {
            StartCoroutine(Fall());
        }
    }

    /// <summary>
    /// Después de que el jugador toque la plataforma, esta se caera y más tarde destruira 
    /// </summary>
    /// <returns></returns>
    private IEnumerator Fall() {
        isFalling = true;
        yield return new WaitForSeconds(fallWait);
        rb.bodyType = RigidbodyType2D.Dynamic;
        Destroy(gameObject, destroyWait);
    }
}
