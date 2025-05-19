using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    // Velocidad de movimiento del ingrediente
    public float speed = 3f;
    
    // Dirección de movimiento (desde spawn point hasta end point)
    private Vector2 moveDirection;
    
    // Flag para saber si el ingrediente está en la zona de entrega
    private bool isInDeliveryArea = false;
    
    // El tipo de ingrediente (basado en su tag)
    private string ingredientType;

    // Inicialización
    private void Start()
    {
        // Guardar el tipo de ingrediente basado en el tag
        ingredientType = gameObject.tag;
    }

    // Update se llama una vez por frame
    private void Update()
    {
        // Mover el ingrediente en la dirección establecida
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    // Método para establecer la dirección de movimiento
    public void SetMoveDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;
    }

    // Método para establecer la dirección basada en un punto destino
    public void SetDestination(Vector3 destination)
    {
        Vector3 direction = destination - transform.position;
        moveDirection = new Vector2(direction.x, direction.y).normalized;
    }

    // Método para manejar cuando el ingrediente es recogido por el jugador
    public void PickUp()
    {
        // Detener el movimiento
        moveDirection = Vector2.zero;
        
        // Hacer que el ingrediente siga al jugador
        // Este método sería llamado por el controlador del jugador
    }

    // Método para obtener el tipo de ingrediente
    public string GetIngredientType()
    {
        return ingredientType;
    }

    // Método para marcar que está en la zona de entrega
    public void SetInDeliveryArea(bool inArea)
    {
        isInDeliveryArea = inArea;
    }

    // Método para verificar si está en la zona de entrega
    public bool IsInDeliveryArea()
    {
        return isInDeliveryArea;
    }

    // Cuando el objeto es desactivado (devuelto al pool)
    private void OnDisable()
    {
        // Resetear variables
        moveDirection = Vector2.zero;
        isInDeliveryArea = false;
    }
}