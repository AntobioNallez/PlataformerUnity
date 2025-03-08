using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoController : MonoBehaviour
{
    private Transform player;
    public float chaseSpeed = 2f;
    public float jumpForce = 2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool shouldJump;

    public int damage = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);

        float direction = Mathf.Sign(player.position.x - transform.position.x);

        bool isPlayerAbove = Physics2D.Raycast(transform.position, Vector2.up, 5f, 1 << player.gameObject.layer);

        if ( isGrounded)
        {
            rb.velocity = new Vector2(direction * chaseSpeed, rb.velocity.y);

            RaycastHit2D groundInFront = Physics2D.Raycast(transform.position, new Vector2(direction, 0), 2f, groundLayer);

            RaycastHit2D gapAhead = Physics2D.Raycast(transform.position + new Vector3(direction, 0, 0), Vector2.down, 2f, groundLayer);

            RaycastHit2D platformAbove = Physics2D.Raycast(transform.position, Vector2.up, 3f, groundLayer);

            if(!groundInFront.collider && !gapAhead.collider) {
                shouldJump = true;
            } else if (isPlayerAbove && platformAbove.collider) {
                shouldJump = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if(isGrounded && shouldJump) {
            shouldJump = false;
            Vector2 direction = (player.position - transform.position).normalized;
            Vector2 jumpDirection = direction * jumpForce;

            rb.AddForce(new Vector2(jumpDirection.x, jumpForce), ForceMode2D.Impulse);
        }
    }
}
