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
        if (_enemy.agent.isActiveAndEnabled)
        {
            _enemy.agent.isStopped = true;
        }
        Object.Destroy(_enemy.gameObject, _tiempoParaDestruir);
    }

    public void Update()
    {
    }

    public void Exit()
    {
    }
}
