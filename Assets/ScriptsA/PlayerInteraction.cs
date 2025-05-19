using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    // Radio de detección para interactuar con ingredientes
    public float interactionRadius = 1.5f;
    
    // Capa en la que están los ingredientes
    public LayerMask ingredientLayer;
    
    // Ingrediente que actualmente está siendo transportado
    private GameObject carriedIngredient;
    
    // Posición relativa para llevar el ingrediente
    public Vector3 carryOffset = new Vector3(0, 0.5f, 0);
    
    // Referencia al objeto del área de entrega
    public Transform deliveryArea;

    // Update se llama una vez por frame
    private void Update()
    {
        // Si se pulsa el botón de interacción (E o Espacio)
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
        {
            // Si ya estamos llevando un ingrediente, intentar dejarlo
            if (carriedIngredient != null)
            {
                DropIngredient();
            }
            // Si no llevamos ningún ingrediente, intentar recoger uno
            else
            {
                PickUpIngredient();
            }
        }
        
        // Si estamos llevando un ingrediente, actualizamos su posición
        if (carriedIngredient != null)
        {
            carriedIngredient.transform.position = transform.position + carryOffset;
        }
    }

    // Método para recoger un ingrediente
    private void PickUpIngredient()
    {
        // Detectar ingredientes cercanos
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactionRadius, ingredientLayer);
        
        // Si hay algún ingrediente cerca
        if (colliders.Length > 0)
        {
            // Tomar el primer ingrediente encontrado
            carriedIngredient = colliders[0].gameObject;
            
            // Desactivar el collider del ingrediente mientras lo llevamos
            Collider2D ingredientCollider = carriedIngredient.GetComponent<Collider2D>();
            if (ingredientCollider != null)
            {
                ingredientCollider.enabled = false;
            }
            
            // Desactivar cualquier movimiento propio del ingrediente
            Ingredient ingredient = carriedIngredient.GetComponent<Ingredient>();
            if (ingredient != null)
            {
                ingredient.PickUp();
            }
        }
    }

    // Método para dejar un ingrediente
    private void DropIngredient()
    {
        // Reactivar el collider del ingrediente
        Collider2D ingredientCollider = carriedIngredient.GetComponent<Collider2D>();
        if (ingredientCollider != null)
        {
            ingredientCollider.enabled = true;
        }
        
        // Si estamos cerca del área de entrega, enviamos el ingrediente allí
        if (deliveryArea != null && Vector3.Distance(transform.position, deliveryArea.position) < interactionRadius * 1.5f)
        {
            // Configurar el ingrediente para que se mueva hacia el área de entrega
            Ingredient ingredient = carriedIngredient.GetComponent<Ingredient>();
            if (ingredient != null)
            {
                ingredient.SetDestination(deliveryArea.position);
            }
        }
        
        // Dejamos de llevar el ingrediente
        carriedIngredient = null;
    }

    // Método para dibujar el radio de interacción en el editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}