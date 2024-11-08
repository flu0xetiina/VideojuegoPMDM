using System.Collections; 
using System.Collections.Generic; 
using UnityEngine;

public class CheckBad : MonoBehaviour 
{
    public Transform spawnPoint; // Define una variable pública que representa el punto de aparición (Transform) donde el jugador será reubicado.
    public GameObject player; // Define una variable pública que hace referencia al objeto del jugador (GameObject).

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.CompareTag("Bad")) // Si el objeto con el que hemos colisionado tiene la etiqueta 'Bad'.
        {
            Debug.Log("Collision with 'Bad' object detected."); // Imprime un mensaje en la consola indicando que ha habido una colisión con un objeto de la etiqueta 'Bad'.
            RespawnPlayer(); // Llama al método 'RespawnPlayer' para reubicar al jugador en el punto de aparición.
        }
    }

    private void RespawnPlayer() // Método para respawnear al jugador en el punto de aparición especificado.
    {
        player.transform.position = spawnPoint.position; // Establece la posición del jugador en la misma posición que el punto de aparición.
        Debug.Log("Player has been respawned."); // Imprime un mensaje en la consola indicando que el jugador ha reaparecido.
    }
}
