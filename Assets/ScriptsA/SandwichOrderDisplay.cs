using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SandwichOrderDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI orderText;
    
    // Diccionario para almacenar los ingredientes del pedido actual
    private Dictionary<string, int> currentOrder = new Dictionary<string, int>();
    
    // Singleton para acceder fácilmente desde otros scripts
    public static SandwichOrderDisplay Instance { get; private set; }
    
    private void Awake()
    {
        // Configurar singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        // Asegurarse de que el componente TextMeshPro está asignado
        if (orderText == null)
        {
            Debug.LogError("TextMeshProUGUI no está asignado en SandwichOrderDisplay");
        }
        
        // Generar un pedido aleatorio al iniciar (puedes reemplazar esto con tu lógica)
        GenerateRandomOrder();
    }
    
    // Método para generar un pedido aleatorio
    public void GenerateRandomOrder()
    {
        // Limpiar pedido anterior
        currentOrder.Clear();
        
        // Ejemplos de ingredientes (ajustar según tu juego)
        string[] ingredients = { "Bread", "Cheese", "Pickles", "Ham", "Egg", "Bacon", "Tomato", "Lettuce", "Onion" };
        
        // Determinar número aleatorio de ingredientes (entre 2 y 5)
        int numIngredients = Random.Range(2, 6);
        
        // Añadir ingredientes aleatorios al pedido
        for (int i = 0; i < numIngredients; i++)
        {
            string ingredient = ingredients[Random.Range(0, ingredients.Length)];
            
            // Aumentar la cantidad si ya existe, sino añadirlo
            if (currentOrder.ContainsKey(ingredient))
            {
                currentOrder[ingredient]++;
            }
            else
            {
                currentOrder[ingredient] = 1;
            }
        }
        
        // Actualizar el texto del pedido
        UpdateOrderText();
    }
    
    // Método para actualizar el texto mostrado
    private void UpdateOrderText()
    {
        if (orderText == null) return;
        
        string orderString = "PEDIDO:\n";
        
        foreach (var item in currentOrder)
        {
            orderString += $"- {item.Key} x{item.Value}\n";
        }
        
        orderText.text = orderString;
    }
    
    // Método para verificar si un sandwich cumple con el pedido
    public bool CheckOrder(Dictionary<string, int> sandwich)
    {
        // Verificar que todos los ingredientes del pedido estén en el sandwich
        foreach (var item in currentOrder)
        {
            if (!sandwich.ContainsKey(item.Key) || sandwich[item.Key] < item.Value)
            {
                return false;
            }
        }
        
        return true;
    }
    
    // Método para obtener el pedido actual (útil para otros scripts)
    public Dictionary<string, int> GetCurrentOrder()
    {
        return new Dictionary<string, int>(currentOrder);
    }
}