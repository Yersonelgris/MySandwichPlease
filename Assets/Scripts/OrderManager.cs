// OrderManager.cs
using UnityEngine;
using System.Collections.Generic;

public class OrderManager : MonoBehaviour
{
    public List<string> possibleIngredients;
    private List<string> currentOrder;
    private int score = 0; // Variable para llevar la puntuación

    private void Start()
    {
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
            GenerateNewOrder();
        }
        else
        {
            Debug.Log("Incorrect delivery.");
        }
        deliveredTags.Clear();
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

        Debug.Log("Nuevo pedido: " + string.Join(", ", currentOrder));
    }
}