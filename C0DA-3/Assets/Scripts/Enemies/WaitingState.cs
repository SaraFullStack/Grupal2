using UnityEngine;

public class WaitingState : InterfaceEnemyStates
{
    private Enemy _enemy;
    public WaitingState(Enemy enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {
        if (_enemy.agent != null && _enemy.agent.isActiveAndEnabled)
        {
            _enemy.agent.SetDestination(_enemy.transform.position);
        }
        
        if (_enemy.animator != null)
        {
            _enemy.animator.SetBool("Walking", false);
        }
    }

    public void Update()
    {
        if (Time.time - _enemy.lastTimeDamage > _enemy.waitingTime)
        {
            _enemy.hasMadeDamage = false; 
        }
    }

    public void Exit()
    {
    }
}
