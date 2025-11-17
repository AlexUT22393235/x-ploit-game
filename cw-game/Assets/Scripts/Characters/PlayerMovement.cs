using UnityEngine;
using UnityEngine.InputSystem; // Si usas el nuevo sistema, si no, usa el Input.GetAxis antiguo

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private float moveInput;

    [Header("Movement")]
    public float moveSpeed = 6f; // Ajustado a tu gusto anterior
    public float jumpForce = 14f;

    [Header("Double Jump")] // --- NUEVA SECCIÓN ---
    public bool canDoubleJump; // Para ver en inspector si está activo
    private bool doubleJumpAvailable; 

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    private bool isGrounded;

    [Header("Wall Mechanics")]
    public Transform wallCheck;
    public LayerMask wallLayer;
    private bool isWalled;
    private bool isWallSliding;
    public float wallSlideSpeed = 2f;
    
    [Header("Wall Fatigue")] // --- NUEVA SECCIÓN ---
    public float maxWallTime = 2f; // Tiempo máximo que puedes aguantar en la pared
    private float wallTimer;

    [Header("Wall Jump")]
    public float wallJumpDuration = 0.2f;
    public Vector2 wallJumpForce = new Vector2(10f, 18f);
    private bool isWallJumping;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        wallTimer = maxWallTime; // Inicializar temporizador
    }

    void Update()
    {
        // --- Input (Sistema Híbrido para asegurar que funcione) ---
        moveInput = Input.GetAxisRaw("Horizontal"); 

        // --- Checks ---
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        isWalled = Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);

        // --- Lógica de Reset (Suelo) ---
        if (isGrounded)
        {
            doubleJumpAvailable = true; // Recargar doble salto
            wallTimer = maxWallTime;    // Recargar energía de pared
        }

        // --- Wall Slide con Fatiga ---
        if (isWalled && !isGrounded && moveInput != 0)
        {
            // Solo deslizamos lento si nos queda tiempo en el "reloj de pared"
            if (wallTimer > 0)
            {
                isWallSliding = true;
                wallTimer -= Time.deltaTime; // Restar tiempo
                
                // Deslizar lento
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -wallSlideSpeed, float.MaxValue));
            }
            else
            {
                // Se acabó el tiempo: Caída normal (la gravedad actúa)
                isWallSliding = false;
            }
        }
        else
        {
            isWallSliding = false;
            if (!isWalled) wallTimer = maxWallTime; // Si te soltaste de la pared, ¿recuperas agarre? 
            // Nota: Si quieres que tenga que tocar SUELO para recuperar agarre, borra la línea de arriba.
        }

        // --- Salto ---
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                Jump();
            }
            else if (isWallSliding)
            {
                WallJump();
            }
            else if (doubleJumpAvailable) // --- Lógica Doble Salto ---
            {
                Jump();
                doubleJumpAvailable = false; // Gastar el doble salto
            }
        }
        
        // Visualización para el inspector
        canDoubleJump = doubleJumpAvailable;
    }

    void Jump()
    {
        // Reseteamos velocidad Y para que el doble salto sea consistente aunque estés cayendo
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    void WallJump()
    {
        isWallJumping = true;
        doubleJumpAvailable = true; // OPCIONAL: ¿Saltar en pared te devuelve el doble salto? (Estilo Celeste/HK)
        
        float jumpDirection = -transform.localScale.x;
        rb.linearVelocity = new Vector2(wallJumpForce.x * jumpDirection, wallJumpForce.y);
        
        // Flip automático para mirar al otro lado al saltar
        if(transform.localScale.x > 0) 
             transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else
             transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        Invoke("StopWallJump", wallJumpDuration);
    }

void FixedUpdate()
    {
        if (!isWallJumping)
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

            if (moveInput > 0)
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            else if (moveInput < 0)
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);      
        }


        if (rb.linearVelocity.y > 20f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 20f);
        }
    } 

    private void StopWallJump()
    {
        isWallJumping = false;
    }

}