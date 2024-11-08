using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyController : MonoBehaviour 
{
    public int runEnemy; // Velocidad de movimiento del enemigo, que puede configurarse en el Inspector.
    public Transform transformPlayer;  // Se asigna el transform del jugador en el Inspector.
    [Range(1, 5)] public int vida; // Define la vida del enemigo, ajustable entre 1 y 5 en el Inspector.
    public float attackRange = 1f; // Rango mínimo para que el enemigo pueda atacar al jugador.
    public Transform spawnPoint; // Se asigna un Transform que define el punto de reaparición del enemigo.

    private Rigidbody2D rb2D; // Componente Rigidbody2D para el control del movimiento físico del enemigo.
    private Transform transformEnemy; // Transform del enemigo para obtener su posición.
    private SpriteRenderer sprtEnemy; // Componente SpriteRenderer para manipular la apariencia visual del enemigo.
    private Animator animEnemy; // Componente Animator para controlar las animaciones del enemigo.
    private int factorX; // Variable para invertir la dirección del enemigo según la posición del jugador.
    private bool isPlayerInRange = false; // Bandera que indica si el jugador está dentro del rango del enemigo.

    public int fireballDamage = 1; // Daño que el enemigo recibe de una bola de fuego (ajustable).
    public int slashDamage = 2;  // Daño que el enemigo recibe del ataque con espada del jugador (ajustable).

    void Start()
    {
        // Inicializa los componentes del enemigo
        rb2D = GetComponent<Rigidbody2D>();
        transformEnemy = GetComponent<Transform>();
        animEnemy = GetComponent<Animator>();
        sprtEnemy = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        // Si el jugador está dentro del rango, el enemigo revisa su movimiento.
        if (isPlayerInRange)
        {
            checkMov();
        }
        else
        {
            // Si el jugador no está dentro del rango, el enemigo deja de moverse.
            rb2D.velocity = new Vector2(0, rb2D.velocity.y);
            animEnemy.SetBool("isRunning", false);
        }
    }

    private void checkMov()
    {
        // Calcula la distancia entre el enemigo y el jugador
        float distanceToPlayer = Vector2.Distance(transformPlayer.position, transformEnemy.position);

        // Determina la dirección en el eje X en función de la posición del jugador
        if (transformPlayer.position.x < transformEnemy.position.x)
        {
            factorX = -1; // El enemigo se mueve hacia la izquierda
            sprtEnemy.flipX = false; // El sprite del enemigo no se voltea.
        }
        else
        {
            factorX = 1; // El enemigo se mueve hacia la derecha
            sprtEnemy.flipX = true; // El sprite del enemigo se voltea.
        }

        // Si la distancia al jugador es mayor que el rango de ataque, el enemigo se mueve hacia él.
        if (distanceToPlayer > attackRange)
        {
            rb2D.velocity = new Vector2(factorX * runEnemy, rb2D.velocity.y);
            animEnemy.SetBool("isRunning", true); // Activa la animación de correr.
        }
        else
        {
            // Si el jugador está dentro del rango de ataque, el enemigo se detiene.
            rb2D.velocity = new Vector2(0, rb2D.velocity.y);
            animEnemy.SetBool("isRunning", false); // Detiene la animación de correr.
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si el objeto que entra en el trigger es el jugador, el enemigo comienza a perseguirlo.
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true; // Establece que el jugador está dentro del rango del enemigo.
            Debug.Log("Player entered range."); // Imprime un mensaje en la consola.
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Si el jugador sale del rango del trigger, el enemigo deja de perseguirlo.
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false; // Establece que el jugador ya no está dentro del rango del enemigo.
            Debug.Log("Player exited range."); // Imprime un mensaje en la consola.
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Si el enemigo colisiona con el jugador, inflige daño al jugador.
        if (other.gameObject.CompareTag("Player"))
        {
            // El jugador recibe 1 de daño al colisionar con el enemigo.
            other.gameObject.GetComponent<PlayerController>().TakeDamage(1);
        }
    }

    private void RespawnPlayer(Transform player)
    {
        // Reaparece al jugador en el punto de reaparición.
        player.position = spawnPoint.position;
    }

    public void TakeDamage(int damage)
    {
        // Reduce la vida del enemigo por el valor de daño recibido.
        vida -= damage;

        // Si la vida del enemigo llega a 0 o menos, el enemigo muere.
        if (vida <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Activa la animación de muerte del enemigo.
        animEnemy.SetTrigger("isDead");
        rb2D.velocity = Vector2.zero; // Detiene el movimiento del enemigo.
        GetComponent<Collider2D>().enabled = false; // Desactiva el collider del enemigo, lo hace invulnerable.
        Destroy(gameObject, animEnemy.GetCurrentAnimatorStateInfo(0).length); // Destruye al enemigo después de que termine la animación de muerte.
    }
}
