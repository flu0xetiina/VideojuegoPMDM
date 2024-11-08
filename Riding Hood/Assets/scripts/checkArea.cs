using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkArea : MonoBehaviour // Clase que acompaña al enemigo
{
    public static bool checkFollow;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") 
        {
            checkFollow = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            checkFollow = false;
        }
    }
}
