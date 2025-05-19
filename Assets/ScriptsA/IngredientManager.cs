using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientManager : MonoBehaviour
{
    // Referencia al IngredientPool
    private IngredientPool ingredientPool;

    // Lista de ingredientes disponibles
    private List<string> availableIngredients = new List<string>()
    {
        "Bread",
        "Cheese",
        "Pickles",
        "Ham",
        "Egg",
        "Bacon",
        "Tomato",
        "Lettuce",
        "Onion"
    };

    // Configuración para los sándwiches aleatorios
    [System.Serializable]
    public class SandwichConfig
    {
        public int minIngredients = 3;
        public int maxIngredients = 6;
        public bool alwaysIncludePan = true; // Siempre incluir pan arriba y abajo
    }

    public SandwichConfig sandwichConfig;

    // Referencias a las posiciones de inicio y fin
    public Transform ingredientSpawnPoint;
    public Transform ingredientEndPoint;

    // Tiempo entre generación de sándwiches
    public float timeBetweenSandwiches = 10f;
    private float sandwichTimer;

    // Tiempo de vida de los ingredientes
    public float ingredientLifetime = 15f;

    private void Start()
    {
        // Obtener referencia al pool de ingredientes
        ingredientPool = IngredientPool.Instance;
        
        // Iniciar el temporizador
        sandwichTimer = timeBetweenSandwiches;
    }

    private void Update()
    {
        // Actualizar el temporizador
        sandwichTimer -= Time.deltaTime;

        // Generar un nuevo sándwich cuando el temporizador llegue a cero
        if (sandwichTimer <= 0)
        {
            GenerateRandomSandwich();
            sandwichTimer = timeBetweenSandwiches;
        }
    }

    // Método para generar un sándwich aleatorio
    private void GenerateRandomSandwich()
    {
        // Determinar el número de ingredientes para este sándwich
        int ingredientCount = Random.Range(sandwichConfig.minIngredients, sandwichConfig.maxIngredients + 1);
        
        // Lista temporal de ingredientes disponibles para este sándwich
        List<string> tempIngredients = new List<string>(availableIngredients);

        // Si siempre se debe incluir pan, asegurarse de que esté en la lista
        if (sandwichConfig.alwaysIncludePan)
        {
            SpawnIngredient("Pan"); // Pan inferior
            ingredientCount -= 1; // Restar 1 para que el conteo sea correcto
        }

        // Generar ingredientes aleatorios
        for (int i = 0; i < ingredientCount; i++)
        {
            // Si ya no hay ingredientes disponibles, terminar
            if (tempIngredients.Count == 0)
                break;

            // Seleccionar un ingrediente aleatorio de la lista
            int randomIndex = Random.Range(0, tempIngredients.Count);
            string selectedIngredient = tempIngredients[randomIndex];

            // Si es el último ingrediente y se debe incluir pan, asegurarse de que sea pan
            if (i == ingredientCount - 1 && sandwichConfig.alwaysIncludePan)
            {
                SpawnIngredient("Pan"); // Pan superior
            }
            else
            {
                // Generar el ingrediente
                SpawnIngredient(selectedIngredient);
                
                // Eliminar el ingrediente de la lista temporal (para evitar duplicados)
                tempIngredients.RemoveAt(randomIndex);
            }
        }
    }

    // Método para generar un ingrediente específico
    private void SpawnIngredient(string tag)
    {
        // Obtener el ingrediente del pool
        GameObject ingredient = ingredientPool.GetIngredient(tag);
        
        if (ingredient != null)
        {
            // Posicionar el ingrediente en el punto de inicio
            ingredient.transform.position = ingredientSpawnPoint.position;
            
            // Programar la devolución del ingrediente al pool después de un tiempo
            StartCoroutine(ReturnIngredientAfterTime(ingredient, ingredientLifetime));
        }
    }

    // Corrutina para devolver el ingrediente al pool después de un tiempo
    private IEnumerator ReturnIngredientAfterTime(GameObject ingredient, float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        
        // Devolver el ingrediente al pool
        ingredientPool.ReturnToPool(ingredient);
    }
}