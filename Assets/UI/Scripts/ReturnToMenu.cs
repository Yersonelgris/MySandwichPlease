using UnityEngine;
using UnityEngine.SceneManagement;
public class ReturnToMenu : MonoBehaviour
{
   public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // Muy importante para evitar que el juego se quede pausado
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Recarga la escena actual
    }
}
