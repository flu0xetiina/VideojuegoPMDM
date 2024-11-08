using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speedBall = 5f; // Velocidad de la bola
    public float vidaFuego = 2f; // Duración de vida de la bola en segundos

    private Vector2 direccionFuego; // Dirección en la que se moverá la bola

    private Rigidbody2D rb2Dball; //obtiene las físicas de la bola

    void Start()
    {
        rb2Dball = GetComponent<Rigidbody2D>();
        Destroy(gameObject, vidaFuego); // Destruye la bola después de vidaFuego segundos
    }

    // Inicializa la bola con una dirección
    public void InitializeBall(Vector2 direction)
    {
        direccionFuego = direction.normalized; // Normaliza la dirección para que sea un vector unitario
        GetComponent<SpriteRenderer>().flipX = (direccionFuego == Vector2.left); //invierte el sprite para que se corresponda con la dirección en la que se mueve la bola
    }

    // Movimiento de la bola
    void FixedUpdate()
    {
        rb2Dball.MovePosition(rb2Dball.position + direccionFuego * speedBall * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
{
    // Verifica si la bola de fuego colisiona con un enemigo
    if (collision.CompareTag("Enemy"))
    {
        // Obtiene el componente EnemyController del enemigo
        EnemyController enemy = collision.GetComponent<EnemyController>();
        if (enemy != null)
        {
            // Aplica daño al enemigo
            enemy.TakeDamage(1); // Cambia el 1 por la cantidad de daño que quieras
        }

        // Destruye la bola de fuego después de hacer daño
        Destroy(gameObject);
    }
}
}