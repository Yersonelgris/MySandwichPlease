// DeliveryZone.cs
using UnityEngine;
using System.Collections.Generic;

public class DeliveryZone : MonoBehaviour
{
    public OrderManager orderManager;
    private List<string> deliveredIngredients = new List<string>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsIngredientTag(other.tag))
        {
            other.gameObject.SetActive(false);
            deliveredIngredients.Add(other.tag);
            Debug.Log("Ingrediente entregado: " + other.tag);

            if (deliveredIngredients.Count == 3)
            {
                // Evaluar solo estos 3 ingredientes
                orderManager.CheckDelivery(new List<string>(deliveredIngredients));
                deliveredIngredients.Clear(); // Limpiar para el siguiente grupo
            }
        }
    }


    private bool IsIngredientTag(string tag)
    {
        return tag == "Cheese" || tag == "Pickles" || tag == "Bacon"
            || tag == "Ham" || tag == "Egg" || tag == "Tomato" || tag == "Lettuce" || tag == "Onion";
    }
}