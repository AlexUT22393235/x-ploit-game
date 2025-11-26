using UnityEngine;

public class SnowyWarrior : Player
{
    //protected override void PlayAttackSound()
    //{
    //    // Aqu� eliges el sonido que quieras para el inicio del ataque del mago
    //    // Puede ser el sonido de cargar magia (Ignis) o el lanzamiento
    //    AudioManager.instance.PlayWizzardIgnis();
    //}
    protected override void Start()
    {
        base.Start();

        life = 125;
    }

    // public override void Update()
    // {
    //     base.Update();
    // }

    // public override void OnCollisionEnter2D(Collision2D collision)
    // {
    //     base.OnCollisionEnter2D(collision);
    // }

    // public override void OnAttack(InputAction.CallbackContext context)
    // {        
    //     base.OnAttack(context);
    // }
}