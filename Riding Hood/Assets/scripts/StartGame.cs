using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public string sceneName = "escena1"; // Name of the scene to load

    // Update is called once per frame
    void Update()
    {
        // Check if the Enter key (Return) is pressed
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // Load the specified scene
            SceneManager.LoadScene(sceneName);
        }
    }
}