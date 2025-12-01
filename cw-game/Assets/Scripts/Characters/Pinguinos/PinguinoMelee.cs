using UnityEngine;
using System.Collections;

public class PinguinoMelee : Enemy
{
    private float dashForce = 10f;
    private int dashDamage = 10;
    private bool hasDashed = false;
    private bool onDash = false;
    private bool damageDealt = false;

    protected override IEnumerator Start()
    {
        yield return StartCoroutine(base.Start()); // Configura rb, animator, nextAttackTime
        life = 45;
        attackRange = 0.29f;
        damage = 15;
        attackDelay = 2f; // El padre usará este valor
        pointsOnDefeat = 50;
    }

    protected override void Update()
    {
        if (playerComponent == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        // LÓGICA DEL DASH (Única de este hijo)
        if (distanceToPlayer <= detectionRange && !hasDashed)
        {
            onDash = true;
            AudioManager.instance.PlayPinMeleeDash();
            hasDashed = true;
            onAttack = false;
            onWalk = false;
        }

        // Si está haciendo Dash, actualizamos animaciones y salimos
        if (onDash)
        {
            SetAnimationStates(onDash, false, false, onDamage);
            return; 
        }

        // SI YA HIZO EL DASH: Dejamos que Papá Enemy maneje el movimiento y el ataque
        base.Update();
        
        // Actualizamos animaciones específicas del hijo (incluyendo dash=false)
        SetAnimationStates(onDash, onWalk, onAttack, onDamage);
    }

    // Sobreescribimos SetAnimationStates para incluir el parámetro "Dash"
    private void SetAnimationStates(bool dash, bool walk, bool attack, bool damage)
    {
        animator.SetBool("Dash", dash);
        base.SetAnimationStates(walk, attack, damage);
    }

    protected override void FixedUpdate()
    {
        if (onDash)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            rb.AddForce(new Vector2(direction.x * dashForce, 0), ForceMode2D.Impulse);
        }
        else
        {
            base.FixedUpdate(); // Movimiento normal
        }
    }

    protected override void OnAttack()
    {
        // Llamamos al padre para que resetee el nextAttackTime
        base.OnAttack(); 
        
        AudioManager.instance.PlayPinMeleeAtack();
        
        // Lógica de daño específica de Melee
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer <= attackRange)
        {
            playerComponent.TakeDamage(damage);
        }
    }

    private void DisableDash()
    {
        onDash = false;
        // Al terminar el dash, permitimos que base.Update() tome el control
    }
    
    // El OnCollisionEnter2D se queda igual que lo tenías
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (onDash && !damageDealt && collision.collider.CompareTag("Player"))
        {
             playerComponent.TakeDamage(dashDamage);
             damageDealt = true;
        }
    }
}