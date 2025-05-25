using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Asegúrate de incluir este namespace

public class GameTimerTMP : MonoBehaviour
{
    [Header("Configuración del Temporizador")]
    public float gameDuration = 120f; // 2 minutos = 120 segundos
    private float timeRemaining;

    [Header("UI Configuración (TMP)")]
    public TMP_Text timerText; // Referencia al componente TextMeshPro

    [Header("Escena al Terminar")]
    public string gameOverScene = "GameOver";

    void Start()
    {
        timeRemaining = gameDuration;
        UpdateTimerDisplay();
    }

    void Update()
    {
        if (timeRemaining > 0f)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerDisplay();

            if (timeRemaining <= 0f)
            {
                timeRemaining = 0f;
                TimeUp();
            }
        }
    }

    void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60f);
            int seconds = Mathf.FloorToInt(timeRemaining % 60f);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    void TimeUp()
    {
        // Cargar la escena de Game Over
        SceneManager.LoadScene(gameOverScene);
        // Ejemplo de cómo llamar a EndGame() desde otro script
        OrderManager orderManager = FindFirstObjectByType<OrderManager>();
        if (orderManager != null)
        {
            orderManager.EndGame();
        }
    }

    // Método opcional para reiniciar el temporizador
    public void ResetTimer()
    {
        timeRemaining = gameDuration;
    }
}