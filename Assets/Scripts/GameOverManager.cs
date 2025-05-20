using UnityEngine;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText;

    void Start()
    {
        // Recupera la puntuación guardada
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0);

        // Muestra la puntuación en el TextMeshProUGUI
        if (finalScoreText != null)
        {
            finalScoreText.text = "" + finalScore;
        }
    }
}