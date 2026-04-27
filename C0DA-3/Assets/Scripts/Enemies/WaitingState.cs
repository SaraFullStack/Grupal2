using UnityEngine;

public class WaitingState : InterfaceEnemyStates
{
    private Enemy _enemy;

    // Constructor para recibir la referencia del enemigo
    public WaitingState(Enemy enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {
        // Al entrar, detenemos al enemigo y ponemos animación de estar quieto
        if (_enemy.agent != null && _enemy.agent.isActiveAndEnabled)
        {
            _enemy.agent.SetDestination(_enemy.transform.position);
        }
        
        if (_enemy.animator != null)
        {
            _enemy.animator.SetBool("Walking", false);
        }

        Debug.Log("Enemigo entrando en estado de Espera");
    }

    public void Update()
    {
        // Comprobamos si ya pasó el tiempo de espera desde el último daño
        if (Time.time - _enemy.lastTimeDamage > _enemy.waitingTime)
        {
            _enemy.hasMadeDamage = false; 
            // Al poner esto en false, el script Enemy lo sacará de este estado 
            // automáticamente en la siguiente comprobación.
        }
    }

    public void Exit()
    {
        // No necesitamos limpiar nada específico al salir
    }
}
