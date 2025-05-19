using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour
{
    // Lista de pedidos activos
    private List<Order> activeOrders = new List<Order>();
    
    // Número máximo de pedidos activos al mismo tiempo
    public int maxActiveOrders = 3;
    
    // Tiempo entre generación de nuevos pedidos
    public float timeBetweenOrders = 15f;
    private float orderTimer;
    
    // Referencia al panel de UI donde se mostrarán los pedidos
    public Transform ordersPanel;
    
    // Prefab para la UI de un pedido
    public GameObject orderUIPrefab;
    
    // Puntuación del jugador
    private int score = 0;
    public Text scoreText;

    // Clase para representar un pedido
    [System.Serializable]
    public class Order
    {
        public List<string> requiredIngredients = new List<string>();
        public float timeLimit;
        public int pointsValue;
        public GameObject uiElement;
    }

    // Inicialización
    private void Start()
    {
        // Iniciar el temporizador
        orderTimer = timeBetweenOrders;
        
        // Actualizar el texto de puntuación
        UpdateScoreUI();
    }

    // Update se llama una vez por frame
    private void Update()
    {
        // Actualizar el temporizador
        orderTimer -= Time.deltaTime;
        
        // Generar un nuevo pedido cuando el temporizador llegue a cero y haya espacio
        if (orderTimer <= 0 && activeOrders.Count < maxActiveOrders)
        {
            GenerateRandomOrder();
            orderTimer = timeBetweenOrders;
        }
        
        // Actualizar los tiempos límite de los pedidos activos
        UpdateOrderTimers();
    }

    // Método para generar un pedido aleatorio
    private void GenerateRandomOrder()
    {
        Order newOrder = new Order();
        
        // Lista de todos los ingredientes disponibles
        List<string> allIngredients = new List<string>()
        {
            "Pan", "Queso", "Mayonesa", "Jamon", 
            "Huevo", "Tocino", "Tomate", "Lechuga", "Cebolla"
        };
        
        // Siempre incluir pan en el pedido (arriba y abajo)
        newOrder.requiredIngredients.Add("Pan");
        
        // Determinar cuántos ingredientes adicionales tendrá el pedido (entre 1 y 5)
        int additionalIngredients = Random.Range(1, 6);
        
        // Seleccionar ingredientes aleatorios
        for (int i = 0; i < additionalIngredients; i++)
        {
            // Si ya no quedan ingredientes disponibles, terminar
            if (allIngredients.Count <= 1) // Solo queda Pan
                break;
                
            // Seleccionar un ingrediente aleatorio que no sea Pan
            string selectedIngredient;
            do
            {
                int randomIndex = Random.Range(0, allIngredients.Count);
                selectedIngredient = allIngredients[randomIndex];
            } while (selectedIngredient == "Pan");
            
            // Añadir el ingrediente al pedido
            newOrder.requiredIngredients.Add(selectedIngredient);
            
            // Eliminar el ingrediente de la lista disponible para evitar duplicados
            allIngredients.Remove(selectedIngredient);
        }
        
        // Añadir el segundo pan (parte superior del sándwich)
        newOrder.requiredIngredients.Add("Pan");
        
        // Establecer el tiempo límite y los puntos del pedido en función de su complejidad
        newOrder.timeLimit = 30f + (newOrder.requiredIngredients.Count * 5f);
        newOrder.pointsValue = 50 + (newOrder.requiredIngredients.Count * 10);
        
        // Crear la UI del pedido
        newOrder.uiElement = CreateOrderUI(newOrder);
        
        // Añadir el pedido a la lista de pedidos activos
        activeOrders.Add(newOrder);
    }

    // Método para crear la UI de un pedido
    private GameObject CreateOrderUI(Order order)
    {
        if (ordersPanel == null || orderUIPrefab == null)
            return null;
            
        // Instanciar el prefab de UI
        GameObject orderUI = Instantiate(orderUIPrefab, ordersPanel);
        
        // Configurar el texto del pedido
        Text orderText = orderUI.GetComponentInChildren<Text>();
        if (orderText != null)
        {
            string ingredientsList = string.Join("\n", order.requiredIngredients);
            orderText.text = "Pedido:\n" + ingredientsList;
        }
        
        return orderUI;
    }

    // Método para actualizar los timers de los pedidos
    private void UpdateOrderTimers()
    {
        List<Order> ordersToRemove = new List<Order>();
        
        foreach (Order order in activeOrders)
        {
            // Reducir el tiempo límite
            order.timeLimit -= Time.deltaTime;
            
            // Si el tiempo se agotó, marcar el pedido para eliminación
            if (order.timeLimit <= 0)
            {
                ordersToRemove.Add(order);
            }
        }
        
        // Eliminar los pedidos expirados
        foreach (Order expiredOrder in ordersToRemove)
        {
            // Destruir la UI del pedido
            if (expiredOrder.uiElement != null)
            {
                Destroy(expiredOrder.uiElement);
            }
            
            // Eliminar el pedido de la lista de activos
            activeOrders.Remove(expiredOrder);
        }
    }

    // Método para verificar si un conjunto de ingredientes completa un pedido
    public bool CheckOrder(List<string> ingredients)
    {
        foreach (Order order in activeOrders)
        {
            // Verificar si los ingredientes coinciden con los requeridos
            if (CompareIngredientLists(ingredients, order.requiredIngredients))
            {
                // Pedido completado
                CompleteOrder(order);
                return true;
            }
        }
        
        return false;
    }

    // Método para comparar dos listas de ingredientes
    private bool CompareIngredientLists(List<string> actual, List<string> required)
    {
        // Si las cantidades no coinciden, no son iguales
        if (actual.Count != required.Count)
            return false;
            
        // Crear copias de las listas para no modificar las originales
        List<string> actualCopy = new List<string>(actual);
        List<string> requiredCopy = new List<string>(required);
        
        // Para cada ingrediente requerido, comprobar si está en la lista actual
        foreach (string ingredient in requiredCopy)
        {
            if (actualCopy.Contains(ingredient))
            {
                // Eliminar el ingrediente de la copia de la lista actual
                actualCopy.Remove(ingredient);
            }
            else
            {
                // Si falta un ingrediente requerido, no coinciden
                return false;
            }
        }
        
        // Si no quedan ingredientes por verificar, las listas coinciden
        return actualCopy.Count == 0;
    }

    // Método para completar un pedido
    private void CompleteOrder(Order order)
    {
        // Añadir puntos
        score += order.pointsValue;
        UpdateScoreUI();
        
        // Destruir la UI del pedido
        if (order.uiElement != null)
        {
            Destroy(order.uiElement);
        }
        
        // Eliminar el pedido de la lista de activos
        activeOrders.Remove(order);
    }

    // Método para actualizar la UI de puntuación
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Puntos: " + score;
        }
    }
}