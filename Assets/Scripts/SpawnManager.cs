using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ObjectPoolSpawnManager : MonoBehaviour
{
    [Header("Prefabs para el Pool")]
    public GameObject[] prefabs;

    [Header("Configuración del Pool")]
    public int cantidadPorPrefab = 5;

    [Header("Posición de Spawn")]
    public Vector2 posicionSpawn = new Vector2(0, 0);

    [Header("Intervalo de Spawns")]
    public float intervalo = 2f;

    private List<GameObject> pool = new List<GameObject>();
    private int currentPrefabIndex = 0; // Índice para el prefab actual

    void Start()
    {
        CrearPool();
        StartCoroutine(SpawnearCadaIntervalo());
    }

    void CrearPool()
    {
        foreach (GameObject prefab in prefabs)
        {
            for (int i = 0; i < cantidadPorPrefab; i++)
            {
                GameObject obj = Instantiate(prefab, Vector2.zero, Quaternion.identity);
                obj.SetActive(false);
                pool.Add(obj);
            }
        }
    }

    IEnumerator SpawnearCadaIntervalo()
    {
        while (true)
        {
            GameObject objeto = ObtenerObjetoDelPool();

            if (objeto != null)
            {
                objeto.transform.position = posicionSpawn;
                objeto.SetActive(true);
            }

            yield return new WaitForSeconds(intervalo);
        }
    }

    GameObject ObtenerObjetoDelPool()
    {
        // Obtener el nombre del prefab actual
        string nombrePrefab = prefabs[currentPrefabIndex].name;

        // Buscar una instancia inactiva del prefab actual
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy && obj.name.StartsWith(nombrePrefab))
            {
                // Avanzar al siguiente prefab para la próxima vez
                currentPrefabIndex = (currentPrefabIndex + 1) % prefabs.Length;
                return obj;
            }
        }

        // Si no hay objetos disponibles del prefab actual, avanzar al siguiente y retornar null
        currentPrefabIndex = (currentPrefabIndex + 1) % prefabs.Length;
        return null;
    }
}