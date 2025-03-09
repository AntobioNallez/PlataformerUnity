using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puerta : MonoBehaviour, IItem
{
    public void Collect()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Tipo especial de item que solo aparece en un mapa especifico, si el jugador obtiene la llave y toca la puerta obtendra mas puntos de lo normal 
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            JugadorScript jugador = collision.gameObject.GetComponent<JugadorScript>();

            if (jugador != null)
            {
                if (jugador.llave)
                {
                    Collect();
                }
            }
        }
    }

}
