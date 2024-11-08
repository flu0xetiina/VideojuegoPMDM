using System.Collections; 
using System.Collections.Generic; 
using UnityEngine;

public class PlayerController : MonoBehaviour // Clase para el control del jugador.
{
    public float runSpeed; // Velocidad de movimiento del jugador.
    public float jumpSpeed; // Velocidad del salto del jugador.
    public GameObject ballPrefab; // Prefabricado de la bola que el jugador disparará.

    private Rigidbody2D rb2D; // Componente Rigidbody2D para gestionar las físicas del jugador.
    private Animator anim; // Componente Animator para gestionar las animaciones del jugador.
    private SpriteRenderer sprtrRnd; // Componente SpriteRenderer para controlar la orientación visual del jugador (flip horizontal).
    private Transform trfm; // Transform del jugador para acceder a su posición y otras propiedades.
    private float lastShoot; // Tiempo de la última vez que el jugador disparó.
    private float waitShootTime; // Tiempo de espera entre disparos.

    private bool isSlashing; // Bandera que indica si el jugador está en medio de un ataque de corte.
    public float slashRange = 1f; // Rango del ataque de corte.
    public int slashDamage = 1;   // Daño del ataque de corte.

    public Transform firePoint; // Punto desde donde se dispara el proyectil.

    private int hitCount = 0; // Contador de los golpes que el jugador ha recibido.
    private const int maxHits = 5; // Número máximo de golpes antes de que el jugador sea respawneado.

    public AudioSource jump; // Fuente de audio para el sonido de salto.
    public AudioSource slash; // Fuente de audio para el sonido de corte.
    public AudioSource muertejugador; // Fuente de audio para el sonido de muerte del jugador.

    void Start()
    {
        // Inicializa los componentes del jugador.
        rb2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprtrRnd = GetComponent<SpriteRenderer>();
        trfm = GetComponent<Transform>();
        waitShootTime = 0.5f; // Tiempo de espera entre disparos.
    }

    void Update()
    {
        checkJump(); // Comprueba si el jugador puede saltar.
        checkMovement(); // Comprueba si el jugador está moviéndose.
        Shoot(); // Comprueba si el jugador quiere disparar.
        Slash(); // Comprueba si el jugador quiere hacer un ataque de corte.
    }

    private void FixedUpdate()
    {
        // Lógica solo para físicas (no se usa en este caso).
    }

    private void Shoot()
    {
        // Si se presiona la tecla "E" y ha pasado el tiempo de espera desde el último disparo
        if (Input.GetKeyDown(KeyCode.E) && (Time.time > lastShoot + waitShootTime))
        {
            // Dirección del disparo dependiendo de si el jugador está mirando hacia la izquierda o derecha
            float direction = sprtrRnd.flipX ? -1f : 1f;

            // Instancia un nuevo proyectil (bola) en el punto de disparo
            GameObject newBall = Instantiate(ballPrefab, firePoint.position, Quaternion.identity);

            // Inicializa el proyectil con la dirección adecuada
            newBall.GetComponent<BallController>().InitializeBall(new Vector2(direction, 0));

            // Actualiza el tiempo del último disparo
            lastShoot = Time.time;
        }
    }

    private void checkJump() 
    {
        // Si se presiona la tecla "Space" y el jugador está en el suelo (verificado por el script 'ChechGround')
        if (Input.GetKeyDown(KeyCode.Space) && ChechGround.isGrounded)
        {
            Jump(); // Llama al método de salto.
        }
        else if (ChechGround.isGrounded && rb2D.velocity.y <= 0) 
        {
            // Si el jugador ha aterrizado y no está subiendo, detiene la animación de salto
            anim.SetBool("isJumping", false);
        }
    }

    private void Jump()
    {
        // Modifica la velocidad del Rigidbody2D para realizar el salto
        rb2D.velocity = new Vector2(rb2D.velocity.x, jumpSpeed);

        // Activa la animación de salto
        anim.SetBool("isJumping", true);

        // Reproduce el sonido de salto
        jump.Play();
    }

   private void Slash()
   {
        // Si se presiona la tecla "C" y el jugador no está ya realizando un corte
        if (Input.GetKeyDown(KeyCode.C) && !isSlashing)
        {
            isSlashing = true; // Establece que el jugador está realizando un corte.
            anim.SetTrigger("isSlashing"); // Activa la animación de corte.
            slash.Play(); // Reproduce el sonido del corte.

            // Detecta los enemigos en el rango del corte
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(trfm.position + new Vector3(sprtrRnd.flipX ? -slashRange : slashRange, 0, 0), slashRange);

            // Recorre todos los enemigos detectados
            foreach (Collider2D enemy in hitEnemies)
            {
                if (enemy.CompareTag("Enemy")) // Si el enemigo tiene la etiqueta "Enemy"
                {
                    // Aplica el daño del corte al enemigo
                    enemy.GetComponent<EnemyController>().TakeDamage(slashDamage);
                }
            }

            // Espera un tiempo antes de permitir un nuevo corte
            StartCoroutine(ResetSlash());
        }
    }

    private IEnumerator ResetSlash()
    {
        // Espera un tiempo (0.933 segundos) y luego resetea el estado de corte
        yield return new WaitForSeconds(0.933f);
        isSlashing = false;
    }

    public void TakeDamage(int damage)
    {
        // Incrementa el contador de golpes por el daño recibido
        hitCount += damage;

        // Si el contador de golpes alcanza el máximo, reaparece al jugador
        if (hitCount >= maxHits)
        {
            RespawnPlayer();
        }
    }

    private void RespawnPlayer()
    {
        // Resetea el contador de golpes y reubica al jugador en la posición de reaparición
        hitCount = 0; // Reset hit count after respawn
        transform.position = new Vector3(0, 0, 0); // Ejemplo de posición de reaparición (ajustar según el escenario)
    }

    private void checkMovement() 
    {
        bool isRunning = false; // Bandera para verificar si el jugador está corriendo

        // Si se mantiene presionada la tecla "A", el jugador se mueve a la izquierda
        if (Input.GetKey(KeyCode.A))
        {
            rb2D.velocity = new Vector2(-runSpeed, rb2D.velocity.y); // Mueve al jugador hacia la izquierda
            isRunning = true;
            sprtrRnd.flipX = true; // Voltea el sprite para que mire a la izquierda
        }
        // Si se mantiene presionada la tecla "D", el jugador se mueve a la derecha
        else if (Input.GetKey(KeyCode.D))
        {
            rb2D.velocity = new Vector2(runSpeed, rb2D.velocity.y); // Mueve al jugador hacia la derecha
            isRunning = true;
            sprtrRnd.flipX = false; // Voltea el sprite para que mire a la derecha
        }
        else
        {
            rb2D.velocity = new Vector2(0, rb2D.velocity.y); // Detiene al jugador si no se presionan las teclas de movimiento
        }

        // Activa o desactiva la animación de correr según el movimiento
        anim.SetBool("isRunning", isRunning);
    }
}
