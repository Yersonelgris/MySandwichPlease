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
        // Selecciona un prefab al azar y busca una instancia inactiva de ese tipo
        int prefabIndex = Random.Range(0, prefabs.Length);
        string nombrePrefab = prefabs[prefabIndex].name;

        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy && obj.name.StartsWith(nombrePrefab))
            {
                return obj;
            }
        }

        return null; // Si no hay objetos disponibles
    }
}
