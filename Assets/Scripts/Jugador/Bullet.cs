using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletDamage = 1;

    void OnTriggerEnter2D(Collider2D collision)
    {
        EnemigoController enemy = collision.GetComponent<EnemigoController>();
        if (enemy)
        {
            enemy.TakeDamage(bulletDamage);
            Destroy(gameObject);
        }
    }
}
