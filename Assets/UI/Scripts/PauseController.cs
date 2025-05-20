using UnityEngine;

public class PauseController : MonoBehaviour
{
    public Timer timer;

    public void PauseGame()
    {
        Time.timeScale = 0f;
        timer.PauseTimer();
        Debug.Log("Juego en pausa");
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        timer.ResumeTimer();
        Debug.Log("Juego reanudado");
    }
}
