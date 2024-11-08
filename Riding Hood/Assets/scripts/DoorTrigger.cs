using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private string sceneToLoad; // Define una variable privada, pero visible en el editor, que almacena el nombre de la escena a cargar cuando el jugador entra en el trigger.

    private void OnTriggerEnter2D(Collider2D other) 
    {
         
        if (other.CompareTag("Player")) // Verifica si el objeto con el que la puerta ha colisionado tiene la etiqueta 'Player'.
        {
            Debug.Log("Player entered the trigger zone."); // Imprime un mensaje en la consola indicando que el jugador ha entrado en la zona del trigger.
            SceneManager.LoadScene(sceneToLoad); // Carga la escena cuyo nombre está almacenado en la variable 'sceneToLoad'.
        }
    }
}