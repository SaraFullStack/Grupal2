using UnityEngine;

public class FollowState :InterfaceEnemyStates
{
    private Enemy _enemy;

    public FollowState (Enemy enemy) => _enemy = enemy;

    public void Enter()
    {
        _enemy.canvasEnemyDetection.enabled = true;
        _enemy.PlayAudio(0); // Usaremos un método auxiliar en enemy
        _enemy.animator.SetBool("Walking", true);
    }

    public void Update()
    {
        if (_enemy.target != null)
            _enemy.agent.SetDestination(_enemy.target.position);
    }

    public void Exit()
    {
        //_enemy.canvasEnemyDetection.enabled = false;
        //_enemy.PlayAudio(1);
    }
}
