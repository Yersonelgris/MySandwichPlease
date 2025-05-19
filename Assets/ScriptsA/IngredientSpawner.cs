using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    // Referencia al IngredientPool
    private IngredientPool ingredientPool;
    
    // Puntos de aparición para cada tipo de ingrediente
    [System.Serializable]
    public class SpawnPoint
    {
        public string ingredientTag;
        public Transform spawnPosition;
    }
    
    // Lista de puntos de aparición
    public List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
    
    // Tiempo entre la aparición de ingredientes
    public float spawnInterval = 3f;
    private float spawnTimer;
    
    // Tiempo de vida de los ingredientes (después de este tiempo vuelven al pool)
    public float ingredientLifetime = 15f;
    
    // Dirección en la que se moverán los ingredientes
    public Vector2 moveDirection = Vector2.down;
    public float moveSpeed = 2f;

    // Start se llama antes del primer frame
    private void Start()
    {
        // Obtener referencia al pool de ingredientes
        ingredientPool = IngredientPool.Instance;
        
        // Inicializar el temporizador
        spawnTimer = spawnInterval;
    }

    // Update se llama una vez por frame
    private void Update()
    {
        // Actualizar el temporizador
        spawnTimer -= Time.deltaTime;
        
        // Si el temporizador llega a cero, generar un ingrediente
        if (spawnTimer <= 0)
        {
            SpawnRandomIngredient();
            spawnTimer = spawnInterval;
        }
    }

    // Método para generar un ingrediente aleatorio
    private void SpawnRandomIngredient()
    {
        // Si no hay puntos de aparición configurados, salir
        if (spawnPoints.Count == 0 || ingredientPool == null)
            return;
            
        // Seleccionar un punto de aparición aleatorio
        int randomIndex = Random.Range(0, spawnPoints.Count);
        SpawnPoint selectedPoint = spawnPoints[randomIndex];
        
        // Obtener un ingrediente del pool
        GameObject ingredient = ingredientPool.GetIngredient(selectedPoint.ingredientTag);
        
        if (ingredient != null && selectedPoint.spawnPosition != null)
        {
            // Posicionar el ingrediente en el punto de aparición
            ingredient.transform.position = selectedPoint.spawnPosition.position;
            
            // Configurar el movimiento del ingrediente
            Ingredient ingredientComponent = ingredient.GetComponent<Ingredient>();
            if (ingredientComponent != null)
            {
                ingredientComponent.SetMoveDirection(moveDirection);
                ingredientComponent.speed = moveSpeed;
            }
            
            // Programar la devolución del ingrediente al pool después de un tiempo
            StartCoroutine(ReturnIngredientAfterTime(ingredient, ingredientLifetime));
        }
    }

    // Método para generar un ingrediente específico
    public void SpawnSpecificIngredient(string tag)
    {
        // Buscar un punto de aparición para este tipo de ingrediente
        SpawnPoint spawnPoint = null;
        foreach (SpawnPoint point in spawnPoints)
        {
            if (point.ingredientTag == tag)
            {
                spawnPoint = point;
                break;
            }
        }
        
        // Si no hay un punto específico, usar el primero disponible
        if (spawnPoint == null && spawnPoints.Count > 0)
        {
            spawnPoint = spawnPoints[0];
        }
        
        // Si no hay puntos de aparición, salir
        if (spawnPoint == null || ingredientPool == null)
            return;
            
        // Obtener un ingrediente del pool
        GameObject ingredient = ingredientPool.GetIngredient(tag);
        
        if (ingredient != null && spawnPoint.spawnPosition != null)
        {
            // Posicionar el ingrediente en el punto de aparición
            ingredient.transform.position = spawnPoint.spawnPosition.position;
            
            // Configurar el movimiento del ingrediente
            Ingredient ingredientComponent = ingredient.GetComponent<Ingredient>();
            if (ingredientComponent != null)
            {
                ingredientComponent.SetMoveDirection(moveDirection);
                ingredientComponent.speed = moveSpeed;
            }
            
            // Programar la devolución del ingrediente al pool después de un tiempo
            StartCoroutine(ReturnIngredientAfterTime(ingredient, ingredientLifetime));
        }
    }

    // Corrutina para devolver el ingrediente al pool después de un tiempo
    private IEnumerator ReturnIngredientAfterTime(GameObject ingredient, float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        
        // Solo devolver al pool si el ingrediente sigue activo
        if (ingredient != null && ingredient.activeSelf)
        {
            ingredientPool.ReturnToPool(ingredient);
        }
    }
}