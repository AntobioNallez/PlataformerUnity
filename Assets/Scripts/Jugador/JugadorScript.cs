using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JugadorScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    bool isFacingRight = true;
    public ParticleSystem efectoAndar;
    public ParticleSystem efectoSpeedBoost;
    public bool llave = false;
    public static event Action<int> DeathBarrier;
    BoxCollider2D playerCollider;

    [Header("Movimiento")]
    public float moveSpeed = 5f;
    float horizontalMovement;
    float speedMultiplier = 1f;

    // [Header("Dash")]
    // public float dashSpeed = 20f;
    // public float dashDuration = 0.1f;
    // public float dashCooldown = 0.1f;
    // bool isDashing;
    // bool canDash = true;
    TrailRenderer trailRenderer;

    [Header("Doble Salto")]
    public float jumpPower = 3f;
    public int saltoMax = 2;
    int saltos;

    [Header("Ground Check")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new(0.5f, 0.04f);
    public LayerMask groundLayer;
    bool isGrounded;
    bool isOnPlatform;

    //TODO FINALIZAR EL SALTO DE PARED
    [Header("Wall Check")]
    public Transform wallCheckPos;
    public Vector2 wallCheckSize = new(0.05f, 0.5f);
    public LayerMask wallLayer;

    [Header("Gravedad")]
    public float gravedadBase = 2f;
    public float speedCaidaMax = 15f;
    public float multiplicadorSpeedCaida = 1.3f;

    [Header("Movimiento Pared")]
    public float velocidadPared = 2f;
    // bool deslizamientoPared;

    //Salto de pared
    // bool isWallJumping;
    // float wallJumpDirection;
    // float wallJumpTime = 0.5f;
    // float wallJumpTimer;
    // public Vector2 wallJumpPower = new(5f, 10f);

    void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        playerCollider = GetComponent<BoxCollider2D>();
        SpeedItem.OnSpeedCollected += SpeedBoost;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(horizontalMovement * moveSpeed * speedMultiplier, rb.velocity.y);
        GroundCheck();
        Gravedad();
        DeslizamientoPared();
        // ProcessWallJump();

        // if (!isWallJumping)
        // {
        Flip();
        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetFloat("magnitude", rb.velocity.magnitude);
        // }
    }

    void SpeedBoost(float multiplier)
    {
        StartCoroutine(SpeedBoostCoroutine(multiplier));
    }

    private IEnumerator SpeedBoostCoroutine(float multiplier)
    {
        speedMultiplier = multiplier;
        efectoSpeedBoost.Play();
        yield return new WaitForSeconds(1f);
        speedMultiplier = 1f;
        efectoSpeedBoost.Stop();
    }

    public void Mover(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    // public void Dash(InputAction.CallbackContext context)
    // {
    //     if (context.performed && canDash)
    //     {
    //         StartCoroutine(DashCoroutine());
    //     }
    // }

    public void Drop(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded && isOnPlatform && playerCollider.enabled)
        {
            StartCoroutine(DisablePlayerCollider(0.3f));
        }
    }

    private IEnumerator DisablePlayerCollider(float disableTime)
    {
        playerCollider.enabled = false;
        yield return new WaitForSeconds(disableTime);
        playerCollider.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isOnPlatform = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isOnPlatform = false;
        }
    }

    // private IEnumerator DashCoroutine()
    // {
    //     canDash = false;
    //     isDashing = true;
    //     trailRenderer.emitting = true;

    //     float dashDirection = isFacingRight ? 1f : -1f;

    //     rb.velocity = new(dashDirection * dashSpeed, rb.velocity.y);

    //     yield return new WaitForSeconds(dashDuration);

    //     rb.velocity = new(0f, rb.velocity.y);

    //     isDashing = false;
    //     trailRenderer.emitting = false;

    //     yield return new WaitForSeconds(dashCooldown);
    //     canDash = true;
    // }

    public void Salto(InputAction.CallbackContext context)
    {
        if (saltos > 0)
        {
            if (context.performed)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                saltos--;
                animator.SetTrigger("jump");
                efectoAndar.Play();
            }
            else if (context.canceled)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0);
                saltos--;
                animator.SetTrigger("jump");
                efectoAndar.Play();
            }
        }

        //salto de pared
        // if (context.performed && wallJumpTimer > 0f)
        // {
        //     isWallJumping = true;
        //     rb.velocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y);
        //     wallJumpTimer = 0;

        //     if (transform.localScale.x != wallJumpDirection)
        //     {
        //         isFacingRight = !isFacingRight;
        //         Vector3 ls = transform.localScale;
        //         ls.x *= -1f;
        //         transform.localScale = ls;
        //     }
        //     Invoke(nameof(CancelWallJump), wallJumpTime + 0.1f);
        // }
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DeathBarrier"))
        {
            ResetPosicion();
        }
        else if (collision.gameObject.CompareTag("Llave"))
        {
            llave = true;
        }
    }

    public void ResetPosicion()
    {
        DeathBarrier.Invoke(1);
        gameObject.transform.position = new(0, 0, 0);
    }

    private void DeslizamientoPared()
    {
        //No tiene que estar tocando el suelo y tiene que estar en una pared
        if (!isGrounded && WallCheck())
        {
            // deslizamientoPared = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -velocidadPared));
        }
        else
        {
            // deslizamientoPared = false;
        }
    }

    //TODO: ARREGLAR WALL JUMP SI LLEGO A DESCUBRIR EL ERROR
    // private void ProcessWallJump()
    // {
    //     if (deslizamientoPared)
    //     {
    //         isWallJumping = false;
    //         wallJumpDirection = -transform.localScale.x;
    //         wallJumpTimer = wallJumpTime;

    //         CancelInvoke(nameof(CancelWallJump));
    //     }
    //     else if (wallJumpTimer > 0f)
    //     {
    //         wallJumpTimer -= Time.deltaTime;
    //     }
    // }

    // private void CancelWallJump()
    // {
    //     isWallJumping = false;
    // }

    private void Flip()
    {
        if (isFacingRight && horizontalMovement < 0 || !isFacingRight && horizontalMovement > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
            efectoSpeedBoost.transform.localScale = ls;

            if (rb.velocity.y == 0)
            {
                efectoAndar.Play();
            }
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
