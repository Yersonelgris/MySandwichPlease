using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientPool : MonoBehaviour
{
    // Singleton para acceder fácilmente desde otros scripts
    public static IngredientPool Instance { get; private set; }

    // Lista de prefabs de ingredientes
    [System.Serializable]
    public class IngredientPrefab
    {
        public string tag;
        public GameObject prefab;
        public int poolSize = 5;
    }

    // Lista de prefabs de ingredientes que queremos en el pool
    public List<IngredientPrefab> ingredientPrefabs;

    // Diccionario para almacenar los pools de cada ingrediente
    private Dictionary<string, Queue<GameObject>> ingredientPools;

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Inicializar el diccionario de pools
        ingredientPools = new Dictionary<string, Queue<GameObject>>();

        // Crear los pools para cada ingrediente
        foreach (var ingredient in ingredientPrefabs)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            // Crear los objetos del pool
            for (int i = 0; i < ingredient.poolSize; i++)
            {
                GameObject obj = Instantiate(ingredient.prefab);
                obj.SetActive(false);
                obj.tag = ingredient.tag;
                objectPool.Enqueue(obj);
            }

            // Agregar el pool al diccionario
            ingredientPools.Add(ingredient.tag, objectPool);
        }
    }

    // Método para obtener un ingrediente del pool
    public GameObject GetIngredient(string tag)
    {
        if (!ingredientPools.ContainsKey(tag))
        {
            Debug.LogWarning("No hay pool para el ingrediente con tag: " + tag);
            return null;
        }

        // Verificar si hay objetos disponibles en el pool
        if (ingredientPools[tag].Count == 0)
        {
            // Si no hay, crear uno nuevo
            foreach (var ingredient in ingredientPrefabs)
            {
                if (ingredient.tag == tag)
                {
                    GameObject obj = Instantiate(ingredient.prefab);
                    obj.tag = tag;
                    return obj;
                }
            }
        }

        // Obtener un objeto del pool
        GameObject pooledObject = ingredientPools[tag].Dequeue();
        pooledObject.SetActive(true);
        
        return pooledObject;
    }

    // Método para devolver un ingrediente al pool
    public void ReturnToPool(GameObject obj)
    {
        string tag = obj.tag;
        
        if (!ingredientPools.ContainsKey(tag))
        {
            Debug.LogWarning("No hay pool para el ingrediente con tag: " + tag);
            Destroy(obj);
            return;
        }

        // Desactivar el objeto y devolverlo al pool
        obj.SetActive(false);
        ingredientPools[tag].Enqueue(obj);
    }
}