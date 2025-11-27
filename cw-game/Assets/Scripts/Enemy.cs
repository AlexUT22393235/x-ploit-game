using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    protected EdgeSensor edgeSensor;
    protected Player playerComponent;
    protected Transform playerTransform;
    protected Rigidbody2D rb;
    protected Vector2 movement;
    protected Animator animator;

    protected float detectionRange = 1.5f;
    // CAMBIO: speed ahora es protected para poder ponerlo en 0 en el pingüino de distancia
    protected float speed = .5f;
    protected int damage = 5;
    protected int lastDirection = 1;
    protected float attackDelay = 1.5f;
    protected int pointsOnDefeat = 50;

    protected float nextAttackTime;
    protected int life;
    protected float attackRange;

    protected bool onWalk = false;
    protected bool onAttack = false;
    protected bool onDamage = false;
    private bool die = false;

    protected virtual IEnumerator Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        edgeSensor = GetComponentInChildren<EdgeSensor>(); // Ojo: PinguinoDistance no tiene edgeSensor a veces

        yield return null;

        playerComponent = FindFirstObjectByType<Player>();

        if (playerComponent != null)
        {
            playerTransform = playerComponent.transform;

            // Inicializamos el tiempo de ataque SOLO cuando encontramos al jugador
            nextAttackTime = Time.time;

            // (Opcional) Debug para saber que ya lo enganchó
            // Debug.Log($"Enemigo {gameObject.name} encontró al Player.");
        }
        else
        {
            // 3. Si falla, esperamos 0.5 segundos y reiniciamos el proceso
            // Debug.LogWarning($"Enemigo {gameObject.name} no encuentra al Player, reintentando...");
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(Start());
        }
    }

    protected virtual void Update()
    {
        if (playerComponent == null) { SetAnimationStates(false, false, false); return; }

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        Vector2 direction = (playerTransform.position - transform.position).normalized;

        if (distanceToPlayer <= detectionRange)
        {
            HandleOrientation(direction.x);

            // --- LÓGICA CENTRALIZADA DE ATAQUE ---
            // Si estamos en rango Y ya pasó el tiempo de espera Y no estamos atacando ya
            if (distanceToPlayer <= attackRange)
            {
                onWalk = false;
                movement = Vector2.zero;

                if (Time.time >= nextAttackTime && !onAttack)
                {
                    onAttack = true;
                    // IMPORTANTE: NO reseteamos nextAttackTime aquí. 
                    // Se resetea en OnAttack() cuando se ejecuta el golpe real.
                }
            }
            else if (!onAttack) // Si no está atacando, persigue
            {
                onWalk = true;
                movement = new Vector2(direction.x, 0);
            }

            // Chequeo de bordes (solo si tiene sensor)
            if (edgeSensor != null && !edgeSensor.IsPlatformAhead)
            {
                onWalk = false;
                movement = Vector2.zero;
            }
        }
        else
        {
            movement = Vector2.zero;
            onWalk = false;
        }

        SetAnimationStates(onWalk, onAttack, onDamage);
    }

    protected void HandleOrientation(float xDir)
    {
        if (xDir < 0) { transform.localScale = new Vector3(1, 1, 1); lastDirection = -1; }
        else if (xDir > 0) { transform.localScale = new Vector3(-1, 1, 1); lastDirection = 1; }
    }

    protected virtual void FixedUpdate()
    {
        if (onWalk && !onAttack && !die)
        {
            rb.linearVelocity = new Vector2(movement.x * speed, rb.linearVelocity.y);
        }
    }

    protected virtual void SetAnimationStates(bool walk, bool attack, bool damage)
    {
        animator.SetBool("Walk", walk);
        animator.SetBool("Attack", attack);
        animator.SetBool("Damage", damage);
    }

    // private void FindPlayer()
    // {
    //     playerComponent = FindFirstObjectByType<Player>();
    //     if (playerComponent != null) playerTransform = playerComponent.transform;
    // }

    public virtual void TakeDamage(int damageAmount)
    {
        life -= damageAmount;
        onDamage = true;
        if (life <= 0) { die = true; animator.SetBool("Die", die); }
    }

    // Este método se debe llamar desde un EVENTO DE ANIMACIÓN en Unity
    protected virtual void OnAttack()
    {
        // AQUÍ es donde reiniciamos el reloj. 
        // Solo cuenta el cooldown una vez que el golpe sale.
        nextAttackTime = Time.time + attackDelay;
    }

    private void DisableAttack() { onAttack = false; }
    private void DisableDamage() { onDamage = false; }

    private void Die()
    {
        Destroy(gameObject);
        if (Score.Instance != null) Score.Instance.AddPoints(pointsOnDefeat);
    }
}