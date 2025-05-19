using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryArea : MonoBehaviour
{
    // Lista para almacenar los ingredientes que están en el área de entrega
    private List<GameObject> ingredientsInArea = new List<GameObject>();
    
    // Referencia al sistema de pedidos
    public OrderMenu orderManager;

    // Método que se llama cuando un objeto entra en el collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar si el objeto que entró es un ingrediente
        if (IsIngredient(other.gameObject))
        {
            // Añadir el ingrediente a la lista
            ingredientsInArea.Add(other.gameObject);
            
            // Notificar al ingrediente que está en el área de entrega
            Ingredient ingredient = other.GetComponent<Ingredient>();
            if (ingredient != null)
            {
                ingredient.SetInDeliveryArea(true);
            }
            
            // Comprobar si se ha completado un pedido
            CheckOrder();
        }
    }

    // Método que se llama cuando un objeto sale del collider
    private void OnTriggerExit2D(Collider2D other)
    {
        // Verificar si el objeto que salió es un ingrediente
        if (IsIngredient(other.gameObject) && ingredientsInArea.Contains(other.gameObject))
        {
            // Eliminar el ingrediente de la lista
            ingredientsInArea.Remove(other.gameObject);
            
            // Notificar al ingrediente que ya no está en el área de entrega
            Ingredient ingredient = other.GetComponent<Ingredient>();
            if (ingredient != null)
            {
                ingredient.SetInDeliveryArea(false);
            }
        }
    }

    // Método para verificar si un objeto es un ingrediente válido
    private bool IsIngredient(GameObject obj)
    {
        // Verificar si el objeto tiene uno de los tags de ingrediente
        string[] ingredientTags = new string[] 
        { 
            "Pan", "Queso", "Mayonesa", "Jamon", 
            "Huevo", "Tocino", "Tomate", "Lechuga", "Cebolla" 
        };
        
        foreach (string tag in ingredientTags)
        {
            if (obj.CompareTag(tag))
            {
                return true;
            }
        }
        
        return false;
    }

    // Método para comprobar si se ha completado un pedido
    private void CheckOrder()
    {
        if (orderManager != null)
        {
            // Crear una lista con los tipos de ingredientes en el área
            List<string> ingredientTypes = new List<string>();
            foreach (GameObject ingredient in ingredientsInArea)
            {
                ingredientTypes.Add(ingredient.tag);
            }
            
            // Verificar con el sistema de pedidos si se ha completado un pedido
            if (orderManager.CheckOrder(ingredientTypes))
            {
                // Si se completó un pedido, limpiar el área de entrega
                ClearDeliveryArea();
            }
        }
    }

    // Método para limpiar el área de entrega
    private void ClearDeliveryArea()
    {
        // Devolver todos los ingredientes al pool
        IngredientPool pool = IngredientPool.Instance;
        
        if (pool != null)
        {
            foreach (GameObject ingredient in ingredientsInArea)
            {
                pool.ReturnToPool(ingredient);
            }
        }
        
        // Limpiar la lista
        ingredientsInArea.Clear();
    }
}