using UnityEngine;

public class SetByTrigger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Destroyer"))
        {
            gameObject.SetActive(false); // Desactiva el objeto en lugar de destruirlo
        }
    }
}
