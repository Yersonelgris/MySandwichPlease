using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Velocidad de movimiento del chef
    public float velocidad = 5.0f;

    // Componente Rigidbody2D que se usará para el movimiento
    private Rigidbody2D rb;

    // Variable para almacenar la dirección del movimiento
    private float movimientoHorizontal;

    // SpriteRenderer para voltear el sprite
    private SpriteRenderer spriteRenderer;

    // Inicialización
    void Start()
    {
        // Obtener componente Rigidbody2D adjunto al objeto
        rb = GetComponent<Rigidbody2D>();

        // Obtener componente SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Asegurar que el Rigidbody2D esté configurado para un juego 2D
        if (rb != null)
        {
            rb.gravityScale = 1; // Mantener la gravedad para que el personaje se mantenga en el suelo
            rb.freezeRotation = true; // Evitar que rote al moverse
        }
    }

    // Se llama una vez por frame
    void Update()
    {
        // Capturar input horizontal (teclas A/D o flechas izquierda/derecha)
        movimientoHorizontal = Input.GetAxisRaw("Horizontal");
    }

    // FixedUpdate se usa para cálculos de física
    void FixedUpdate()
    {
        // Mover al personaje aplicando velocidad
        Vector2 movimiento = new Vector2(movimientoHorizontal * velocidad, rb.linearVelocity.y);
        rb.linearVelocity = movimiento;
    }
}

