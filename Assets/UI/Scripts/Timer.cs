using UnityEngine;
using TMPro;
using System.Collections;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public GameObject finalPanel; // ← Asigna este panel desde el inspector

    private int remainingTime = 60;
    private bool isRunning = false;
    private bool isPaused = false;
    private Coroutine countdownCoroutine;

    public void StartTimer()
    {
        if (!isRunning)
        {
            remainingTime = 60;
            timerText.text = remainingTime.ToString() + " s";
            isRunning = true;
            isPaused = false;
            if (finalPanel != null) finalPanel.SetActive(false); // Asegura que el panel esté oculto al iniciar
            countdownCoroutine = StartCoroutine(CountdownRoutine());
        }
    }

    IEnumerator CountdownRoutine()
    {
        while (remainingTime > 0)
        {
            yield return new WaitUntil(() => !isPaused); // Espera si está en pausa
            yield return new WaitForSeconds(1f);
            remainingTime--;
            timerText.text = remainingTime.ToString() + " s";
        }

        isRunning = false;

        // ⏰ ¡Tiempo agotado!
        Debug.Log("🛑 ¡Tiempo agotado!");
        Time.timeScale = 0f;

        if (finalPanel != null)
            finalPanel.SetActive(true); // ← Activa el panel final
    }

    public void PauseTimer()
    {
        isPaused = true;
    }

    public void ResumeTimer()
    {
        isPaused = false;
    }

    public void StopTimer()
    {
        isRunning = false;
        isPaused = false;
        if (countdownCoroutine != null)
            StopCoroutine(countdownCoroutine);
    }
}
