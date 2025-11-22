using UnityEngine;

public class AnimationkillerWhale : MonoBehaviour
{
    public float speed = 3f;
    public float patrolDistance = 2f;

    // Componentes
    private Rigidbody2D rb;
    private Vector2 startPosition;

    private float currentDirection = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("El componente Rigidbody2D es requerido en el objeto para este script.");
            enabled = false;
            return;
        }

        startPosition = transform.position;
    }

    void Update()
    {
        float currentX = transform.position.x;

        if (currentDirection > 0 && currentX >= startPosition.x + patrolDistance)
        {
            currentDirection = -1f;
        }
        else if (currentDirection < 0 && currentX <= startPosition.x - patrolDistance)
        {
            currentDirection = 1f;
        }

        FlipSprite(currentDirection);
    }

    void FixedUpdate()
    {
        Vector2 direction = new Vector2(currentDirection, 0);
        Vector2 movement = new Vector2(direction.x * speed, rb.linearVelocity.y);

        rb.linearVelocity = movement;
    }

    void FlipSprite(float directionX)
    {
        if (directionX < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (directionX > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}