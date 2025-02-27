using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JugadorScript : MonoBehaviour
{
    public Rigidbody2D rb;
    bool isFacingRight = false;

    [Header("Movimiento")]
    public float moveSpeed = 5f;
    float horizontalMovement;

    [Header("Doble Salto")]
    public float jumpPower = 3f;
    public int saltoMax = 2;
    int saltos;

    [Header("Ground Check")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;

    //TODO FINALIZAR EL SALTO DE PARED
    [Header("Wall Check")]
    public Transform wallCheckPos;
    public Vector2 wallCheckSize = new Vector2(0.49f, 0.03f);
    public LayerMask wallLayer;
    

    [Header("Gravedad")]
    public float gravedadBase = 2f;
    public float speedCaidaMax = 15f;
    public float multiplicadorSpeedCaida = 1.3f;

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(horizontalMovement * moveSpeed, rb.velocity.y);
        GroundCheck();
        Gravedad();
        Flip();
    }

    public void Mover(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void Salto(InputAction.CallbackContext context)
    {
        if (saltos > 0)
        {
            if (context.performed)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                saltos--;
            }
            else if (context.canceled)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0);
                saltos--;
            }
        }
    }

    //NO FUNCIONA CORRECTAMENTE REVISAR
    private void Gravedad()
    {
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = gravedadBase * multiplicadorSpeedCaida;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -speedCaidaMax));
        }
        else
        {
            rb.gravityScale = gravedadBase;
        }
    }

    private void Flip() {
        if(isFacingRight && horizontalMovement < 0 || !isFacingRight && horizontalMovement > 0) {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x = -1f;
        }
    }

    private void GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            saltos = saltoMax;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(wallCheckPos.position, wallCheckSize);
    }
}
