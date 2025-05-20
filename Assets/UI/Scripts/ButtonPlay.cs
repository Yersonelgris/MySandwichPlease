using UnityEngine;

public class ButtonPlay : MonoBehaviour
{
    public CountDownSprite countdown;
    public Timer timerScript; // <- Asumiendo que tienes un script de timer

    void Start()
    {
        Pause(); // Pausar juego al inicio

        if (countdown != null)
        {
            countdown.OnCountdownFinished += OnCountdownEnded; // <- Vinculamos evento
        }
    }

    public void Play()
    {
        if (countdown != null)
        {
            countdown.StartCountdown(); // Empieza cuenta regresiva
        }
        else
        {
            ResumeGame();
        }
    }

    void OnCountdownEnded()
    {
        ResumeGame();
    }

    void ResumeGame()
    {
        Time.timeScale = 1;
        if (timerScript != null)
        {
            timerScript.StartTimer(); // <- Asumiendo que tiene este método
        }
        Debug.Log("Juego reanudado y timer iniciado.");
    }

    public void Pause()
    {
        Time.timeScale = 0;
        Debug.Log("Juego pausado");
    }
}
