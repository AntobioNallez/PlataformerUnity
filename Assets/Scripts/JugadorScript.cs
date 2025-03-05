using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JugadorScript : MonoBehaviour
{
    public Rigidbody2D rb;
    bool isFacingRight = true;

    [Header("Movimiento")]
    public float moveSpeed = 5f;
    float horizontalMovement;

    [Header("Doble Salto")]
    public float jumpPower = 3f;
    public int saltoMax = 2;
    int saltos;

    [Header("Ground Check")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.04f);
    public LayerMask groundLayer;
    bool isGrounded;

    //TODO FINALIZAR EL SALTO DE PARED
    [Header("Wall Check")]
    public Transform wallCheckPos;
    public Vector2 wallCheckSize = new Vector2(0.05f, 0.5f);
    public LayerMask wallLayer;

    [Header("Gravedad")]
    public float gravedadBase = 2f;
    public float speedCaidaMax = 15f;
    public float multiplicadorSpeedCaida = 1.3f;

    [Header("Movimiento Pared")]
    public float velocidadPared = 2f;
    bool deslizamientoPared;

    //Salto de pared
    bool isWallJumping;
    float wallJumpDirection;
    float wallJumpTime = 0.5f;
    float wallJumpTimer;
    Vector2 wallJumpPower = new Vector2(5f, 10f);

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(horizontalMovement * moveSpeed, rb.velocity.y);
        GroundCheck();
        Gravedad();
        DeslizamientoPared();
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

        //salto de pared
        if(context.performed && wallJumpTimer > 0f) {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y);
        } 
    }

    //NO FUNCIONA CORRECTAMENTE REVISAR (CREO QUE FUNCIONA HE DE REVISAR)
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

    private void DeslizamientoPared()
    {
        //No tiene que estar tocando el suelo y tiene que estar en una pared
        if (!isGrounded && WallCheck() && horizontalMovement != 0)
        {
            deslizamientoPared = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -velocidadPared));
        }
        else
        {
            deslizamientoPared = false;
        }
    }

    private void ProcessWallJump() {
        if(deslizamientoPared) {
            deslizamientoPared = false;
        }
    }

    private void Flip()
    {
        if (isFacingRight && horizontalMovement < 0 || !isFacingRight && horizontalMovement > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }

    private void GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            saltos = saltoMax;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private bool WallCheck()
    {
        return Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0, wallLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(wallCheckPos.position, wallCheckSize);
    }
}
