using UnityEngine;
using UnityEngine.InputSystem; // Si usas el nuevo sistema, si no, usa el Input.GetAxis antiguo

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private float moveInput;

    [Header("Movement")]
    public float moveSpeed = 8f; // Ajustado a tu gusto anterior
    public float jumpForce = 14f;

    [Header("Double Jump")] // --- NUEVA SECCIÓN ---
    public bool canDoubleJump; // Para ver en inspector si está activo
    public int maxJumps = 2; // Máximo de saltos antes de tocar suelo
    private int jumpCount = 0; // Contador de saltos realizados desde el suelo

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

    [Header("Movimiento e Hielo")]
    
    // NUEVAS VARIABLES PARA FISICA DE HIELO
    public float acceleration = 50f;    // Qué tan rápido arranca en suelo normal
    public float deacceleration = 50f;  // Qué tan rápido frena en suelo normal (alto = frenado seco)
    public float iceFriction = 0.5f;    // Qué tan rápido frena en hielo (Bajo = muy resbaloso)
    
    private bool onIce; // Para saber si estamos patinando

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
            jumpCount = 0; // Resetear contador de saltos al tocar suelo
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
            else if (jumpCount < maxJumps)
            {
                Jump();
            }
        }
        
        // Visualización para el inspector
        // mostrar si todavía puede ejecutar saltos adicionales en el aire
        canDoubleJump = (!isGrounded && jumpCount < maxJumps);
    }

    void Jump()
    {
        // Reseteamos velocidad Y para que el doble salto sea consistente aunque estés cayendo
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        // Contar este salto (siempre que no estemos en el suelo)
        if (!isGrounded)
            jumpCount++;
    }

    void WallJump()
    {
        isWallJumping = true;
        // Tras un wall jump consideramos que ya hemos usado 1 salto
        jumpCount = 1;
        
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

        // 1. DETECTAR HIELO
        Collider2D groundCollider = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        if (groundCollider != null && groundCollider.CompareTag("Ice"))
        {
            onIce = true;
        }
        else
        {
            onIce = false;
        }

        // 2. MOVIMIENTO CON INERCIA
        if (!isWallJumping)
        {
            // Calculamos la velocidad objetivo (A donde queremos ir)
            float targetSpeed = moveInput * moveSpeed;
            
            // Calculamos la diferencia entre la velocidad actual y la deseada
            float speedDif = targetSpeed - rb.linearVelocity.x;

            // Decidimos qué tasa de cambio usar (Aceleración o Frenado)
            float accelRate;

            if (onIce)
            {
                // Si estamos en hielo...
                if (Mathf.Abs(moveInput) > 0.01f) 
                    accelRate = acceleration; // Si oprimes teclas, tienes control
                else 
                    accelRate = iceFriction;  // SI NO OPRIMES NADA: Frena muy lento (Resbala)
            }
            else
            {
                // Suelo normal
                if (Mathf.Abs(moveInput) > 0.01f)
                    accelRate = acceleration;
                else
                    accelRate = deacceleration; // Frenado seco habitual
            }

            // Aplicamos la fuerza basada en la diferencia
            float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, 0.9f) * Mathf.Sign(speedDif);
            rb.AddForce(movement * Vector2.right);
        }

        // --- LIMITAR VELOCIDAD MÁXIMA DE CAÍDA/SUBIDA (Tu código existente) ---
        if (rb.linearVelocity.y > 20f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 20f);
        }
    }

}