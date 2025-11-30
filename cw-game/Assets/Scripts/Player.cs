using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private float speed = 1f;
    private float force = 180f;
    public int life = 100;
    private int jumps = 0;
    protected int lastDirection = 1;

    private bool isGrounded = false;
    private bool jumping = false;
    private bool onDash = false;
    private bool attacking = false;
    private bool onDamage = false;
    private bool die = false;

    private Vector2 direction;
    private Rigidbody2D rb;
    private Animator animator;

    //Variable para almacenar el tipo de superficie actual
    private SurfaceType currentSurface = SurfaceType.Default;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // if (GameManager.Instance == null)
        // {
        //     Debug.LogError("No se encontró una instancia de GameManager en la escena. Asegúrate de tener uno.");
        // }
    }

    protected virtual void Update()
    {
        if (die)
        {
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
            SetAnimationStates(0, false, false, false, false);
            return;
        }

        transform.Translate(direction * speed * Time.deltaTime);

        if (direction.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            lastDirection = -1;
        }
        else if (direction.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            lastDirection = 1;
        }

        SetAnimationStates(direction.magnitude, jumping, attacking, onDash, onDamage);
    }

    // protected virtual void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.collider.CompareTag("Floor") || collision.collider.CompareTag("Enemy"))
    //     {
    //         isGrounded = true;
    //         jumping = false;
    //     }
    // }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
{
    // Solo detectamos suelo si la etiqueta es correcta
    if (collision.collider.CompareTag("Floor") || collision.collider.CompareTag("Enemy"))
    {
        // Debug.Log("Colisión con suelo o enemigo detectada.");
        // Recorremos los puntos de contacto para ver la dirección del golpe
        foreach (ContactPoint2D contact in collision.contacts)
        {
            // Debug.Log("Normal del contacto: " + contact.normal);
            // Si la normal apunta hacia arriba (aprox), significa que pisamos algo
            if (contact.normal.y > 0.5f)
            {
                // Debug.Log("El jugador ha aterrizado.");
                
                // 1. Lógica original de movimiento
                isGrounded = true;
                jumping = false;
                jumps = 0; 

                // 2. NUEVA LÓGICA: Detección de Superficie
                // Solo buscamos el componente si confirmamos que estamos pisando el objeto
                SurfaceData surfaceInfo = collision.gameObject.GetComponent<SurfaceData>();

                if (surfaceInfo != null)
                {
                    // ¡Encontrado! Guardamos el tipo (Nieve, Piedra, etc.)
                    currentSurface = surfaceInfo.surfaceType;
                }
                else
                {
                    // Si es suelo pero no tiene script, asumimos que es Default (Piedra/Genérico)
                    currentSurface = SurfaceType.Default;
                }
                // ----------------------------------------------------

                break; // Ya encontramos suelo, dejamos de buscar
            }
        }
    }
}

    // protected virtual void OnCollisionExit2D(Collision2D collision)
    // {
    //     if (collision.collider.CompareTag("Floor") || collision.collider.CompareTag("Enemy"))
    //     {
    //         isGrounded = false;
    //     }
    // }

// 3. EL SWITCH PRINCIPAL (Este método será llamado por la Animación)
    public void PlayFootstepSound()
    {
        // Solo reproducimos sonido si estamos pisando el suelo.
        // Esto evita que suene si la animación corre mientras saltamos o caemos.
        if (isGrounded)
        {
            switch (currentSurface)
            {
                case SurfaceType.Snow:
                    AudioManager.instance.PlayFootstepSnow();
                    break;

                case SurfaceType.Stone:
                    AudioManager.instance.PlayFootstepStone();
                    break;

                default: // Default o cualquier caso no configurado
                    AudioManager.instance.PlayFootstepStone(); 
                    break;
            }
        }
    }
    private void SetAnimationStates(float speed, bool jump, bool attack, bool dash, bool damage)
    {
        animator.SetFloat("Speed", speed);
        animator.SetBool("Jump", jump);
        animator.SetBool("Attack", attack);
        animator.SetBool("Dash", dash);
        animator.SetBool("Damage", damage);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (die) return;
        direction = context.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (die) return;
        if (context.performed)
        {
            // Debug.Log("OnJump llamado");
            // Debug.Log("Esta en piso: " + isGrounded);
            if (isGrounded || jumps < 2)
            {
                if (isGrounded)
                {
                    jumps = 0;
                }
                rb.AddForce(Vector2.up * force * 1.6f, ForceMode2D.Impulse);
                jumps++;
                isGrounded = false;
                if (jumps >= 1 && jumps <= 2)
                {
                    switch (jumps)
                    {
                        case 1:
                            jumping = true;
                            AudioManager.instance.PlayProtaDashAir();
                            onDash = false;
                            break;
                        case 2:
                            jumping = false;
                            AudioManager.instance.PlayProtaDashAir();
                            onDash = true;
                            break;
                    }
                }
            }
        }
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        Debug.Log("OnDash llamado");
        if (die) return;
        Debug.Log("Esta en piso: " + isGrounded);
        if (context.performed && isGrounded)
        {
            Debug.Log("Haciendo dash");
            onDash = true;
            AudioManager.instance.PlayProtaDash();
            rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);
        }
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        Debug.Log("OnAttack llamado");
        if (die) return;
        Debug.Log("Esta en piso: " + isGrounded);
        if (context.performed && !attacking && isGrounded)
        {
            Debug.Log("Haciendo ataque");
            attacking = true;
            PlayAttackSound();
            AudioManager.instance.PlayWarriorAttack();
        }
    }

    protected virtual void PlayAttackSound()
    {
        // Si el script Player se usa directamente en el Guerrero, sonará esto:
         AudioManager.instance.PlayWarriorAttack();
    }

    public virtual void TakeDamage(int damageAmount)
    {
        if (die) return;

        life -= damageAmount;
        // Debug.Log(gameObject.name + " ha recibido " + damageAmount + " de daño. Vida restante: " + life);

        onDamage = true;
        AudioManager.instance.PlayProtaDamage();

        if (life <= 0)
        {
            life = 0;
            die = true;
            animator.SetBool("Die", die);

            var inputAction = GetComponent<PlayerInput>();
            if (inputAction != null)
            {
                inputAction.enabled = false;
            }

            if (rb != null)
            {
                rb.simulated = false;
            }

            GameManager.Instance?.PlayerDied();
        }
    }

    private void DisableJump()
    {
        jumping = false;
    }

    private void DisableDash()
    {
        onDash = false;
    }

    private void DisableAttack()
    {
        attacking = false;
    }

    private void DisableDamage()
    {
        onDamage = false;
    }
}