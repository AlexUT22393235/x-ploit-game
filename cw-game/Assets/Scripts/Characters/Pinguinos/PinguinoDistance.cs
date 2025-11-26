using UnityEngine;
using System.Collections;

public class PinguinoDistance : Enemy
{
    private GameObject iceSpikePrefab;
    private float velocityIceSpike = 3f;

    protected override IEnumerator Start()
    {
        yield return StartCoroutine(base.Start());
        
        // Configuraciones específicas
        life = 30;
        attackRange = 5f; // Aumenté un poco el rango para que dispare de lejos
        pointsOnDefeat = 25;
        speed = 0f; // TRUCO: Al poner velocidad 0, base.Update() gestiona la IA pero no lo mueve.
        
        // EdgeSensor no es necesario si no se mueve
        edgeSensor = null; 

        iceSpikePrefab = GameObject.Find("Ice spike");
        if (iceSpikePrefab == null) Debug.LogError("¡No se encontró 'Ice spike'!");
    }

    // NO NECESITAS OVERRIDE DE UPDATE
    // base.Update() ya hace: Mirar al jugador, Checar Cooldown, Activar bool Attack.
    
    // Solo necesitamos definir QUÉ pasa cuando ataca
    protected override void OnAttack()
    {
        // Llamamos al padre para resetear el reloj (nextAttackTime)
        base.OnAttack(); 

        Vector3 spawnOffset = new Vector3(.12f * lastDirection, 0.1f, 0f);
        // Instancia el prefab (asegúrate que iceSpikePrefab sea un PREFAB y no un objeto de la escena si quieres instanciar varios)
        GameObject nuevoProyectil = Instantiate(iceSpikePrefab, transform.position + spawnOffset, transform.rotation);
        
        // Ajustes del proyectil
        nuevoProyectil.SetActive(true); // Por si el prefab original estaba desactivado
        Rigidbody2D rbP = nuevoProyectil.GetComponent<Rigidbody2D>();

        if (rbP != null)
        {
            rbP.linearVelocity = new Vector2(lastDirection * velocityIceSpike, 0f);
            AudioManager.instance.PlayPinDisAtack();
        }
    }
}