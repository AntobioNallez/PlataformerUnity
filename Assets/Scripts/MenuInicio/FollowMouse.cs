using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowMouse : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public ParticleSystem efectoAndar;
    BoxCollider2D playerCollider;

    [Header("Movimiento")]
    public float moveSpeed = 5f;
    float speedMultiplier = 1f;
    private float stopDistance = 0.1f;

    bool isFacingRight = true;

    [Header("Cooldown Movimiento")]
    public float moveCooldown = 0.5f; // Tiempo de cooldown antes de moverse
    private float moveCooldownTimer = 0f; // Temporizador del cooldown

    void Start()
    {
        playerCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        moveCooldownTimer -= Time.deltaTime; // Disminuir el temporizador del cooldown
        MoveTowardsMouse();

        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetFloat("magnitude", rb.velocity.magnitude);
    }

    void MoveTowardsMouse()
    {
        // Obtener la posición del ratón en el mundo
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Asegúrate de que la posición Z sea 0

        // Mantener la altura del jugador constante
        mousePosition.y = transform.position.y; // Fijar la altura del ratón a la del jugador

        // Calcular la dirección hacia el ratón
        Vector2 direction = (mousePosition - transform.position).normalized;

        // Verificar si el jugador está dentro de la distancia de parada
        if (Vector2.Distance(transform.position, mousePosition) > stopDistance)
        {
            // Mover al jugador en la dirección calculada
            rb.velocity = new Vector2(direction.x * moveSpeed * speedMultiplier, rb.velocity.y);

            // Controlar la dirección y la animación
            if (direction.x != 0)
            {
                Flip(direction.x);
            }

            // Actualizar las animaciones basadas en la velocidad
            animator.SetFloat("magnitude", Mathf.Abs(rb.velocity.x)); // Usar solo la componente X para la magnitud
        }
        else
        {
            // Detener al jugador
            rb.velocity = new Vector2(0, rb.velocity.y); // Mantener la componente Y

            // Actualizar animaciones para que no se alteren
            animator.SetFloat("magnitude", 0); // Detener animaciones si está parado
        }
    }



    private void Flip(float directionX)
    {
        // Cambiar la dirección del jugador según la dirección del movimiento
        if (directionX > 0 && !isFacingRight)
        {
            isFacingRight = true;
            Vector3 ls = transform.localScale;
            ls.x = Mathf.Abs(ls.x);
            transform.localScale = ls;
        }
        else if (directionX < 0 && isFacingRight)
        {
            isFacingRight = false;
            Vector3 ls = transform.localScale;
            ls.x = -Mathf.Abs(ls.x);
            transform.localScale = ls;
        }
    }
}
