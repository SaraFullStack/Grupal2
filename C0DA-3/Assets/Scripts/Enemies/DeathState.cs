using UnityEngine;

public class DeathState : InterfaceEnemyStates
{
    
private Enemy _enemy;
    private float _tiempoParaDestruir = 2f;

    public DeathState(Enemy enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {
        // Detenemos el agente para que no siga moviéndose muerto
        if (_enemy.agent.isActiveAndEnabled)
        {
            _enemy.agent.isStopped = true;
        }

        // Ejecutamos la animación de muerte
        //_enemy.animator.SetBool("Dead", true);

        // Sonido de muerte (Índice 3 según tu script original)
        //_enemy.PlayAudio(3);

        // Actualizamos la puntuación
        //_enemy.AddScore(100);

        // Programamos la destrucción del objeto
        Object.Destroy(_enemy.gameObject, _tiempoParaDestruir);
    }

    public void Update()
    {
        // Normalmente en la muerte no se hace nada en el Update,
        // pero podríamos añadir efectos de desvanecimiento (fade out) aquí.
    }

    public void Exit()
    {
        // No suele ser necesario hacer nada aquí ya que el objeto se destruye
    }
}
