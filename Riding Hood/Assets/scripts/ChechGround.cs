using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 

public class ChechGround : MonoBehaviour // Define una clase llamada 'ChechGround' que hereda de MonoBehaviour (base para todos los scripts de Unity).
{
    public static bool isGrounded; // Define una variable estática pública llamada 'isGrounded', que indica si el objeto está tocando el suelo.

    private void OnTriggerEnter2D(Collider2D collision) // Función que se llama cuando otro collider entra en contacto con el trigger de este objeto en 2D.
    {
        if (collision.CompareTag("Ground")) // Si el objeto con el que colisionamos tiene la etiqueta 'Ground'.
        {
            isGrounded = true; // Si estamos tocando el suelo, establece 'isGrounded' a true.
        }
    }

    private void OnTriggerExit2D(Collider2D collision) // Función que se llama cuando otro collider deja de estar en contacto con el trigger de este objeto en 2D.
    {
        if (collision.CompareTag("Ground")) 
        {
            isGrounded = false; // Si dejamos de tocar el suelo, establece 'isGrounded' a false.
        }
    }
}