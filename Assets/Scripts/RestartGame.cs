using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    // Llama a este método para reiniciar el juego
    public void Restart()
    {
        // Recarga la escena activa (reinicia el juego)
        SceneManager.LoadScene("UI");
    }
}