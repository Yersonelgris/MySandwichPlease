using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class OrderManager : MonoBehaviour
{
    public List<string> possibleIngredients;
    private List<string> currentOrder;
    private int score = 0;
    public TextMeshProUGUI orderText;
    public TextMeshProUGUI scoreText;

    void Start()
    {
        if (scoreText != null)
            scoreText.text = "" + score;

        GenerateNewOrder();
    }

    public void CheckDelivery(List<string> deliveredTags)
    {
        deliveredTags.Sort();
        currentOrder.Sort();

        bool matches = deliveredTags.Count == currentOrder.Count;
        for (int i = 0; i < deliveredTags.Count && matches; i++)
        {
            if (deliveredTags[i] != currentOrder[i])
                matches = false;
        }

        if (matches)
        {
            Debug.Log("Correct delivery!");
            score++;
            Debug.Log("Puntuación actual: " + score);

            if (scoreText != null)
                scoreText.text = "" + score;

            GenerateNewOrder();
        }
    }

    private void GenerateNewOrder()
    {
        currentOrder = new List<string>();
        int numberOfIngredients = 3;

        while (currentOrder.Count < numberOfIngredients)
        {
            string ingredient = possibleIngredients[Random.Range(0, possibleIngredients.Count)];
            if (!currentOrder.Contains(ingredient))
                currentOrder.Add(ingredient);
        }

        string orderDisplay = "Order: " + string.Join(", ", currentOrder);
        Debug.Log("Nuevo pedido: " + string.Join(", ", currentOrder));

        if (orderText != null)
            orderText.text = orderDisplay;
    }

    public void EndGame()
    {
        PlayerPrefs.SetInt("FinalScore", score);
        PlayerPrefs.Save(); // Guarda los cambios
        SceneManager.LoadScene("GameOver"); // Cambia a la escena GameOver
    }
}
