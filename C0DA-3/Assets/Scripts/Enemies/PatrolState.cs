using UnityEngine;

public class PatrolState : InterfaceEnemyStates
{
    private Enemy _enemy;
    private int _currentPoint = 0;

    public PatrolState(Enemy enemy) => _enemy = enemy;

    public void Enter() 
    {
        if(_enemy.animator != null) _enemy.animator.SetBool("Walking", true);
    }

    public void Update()
    {
        if (_enemy.patrolPointsParent == null || _enemy.patrolPointsParent.childCount == 0) return;

        Vector3 targetPos = _enemy.patrolPointsParent.GetChild(_currentPoint).position;
        _enemy.agent.SetDestination(targetPos);

        if (Vector3.Distance(_enemy.transform.position, targetPos) < _enemy.reachDistance)
        {
            _currentPoint = (_currentPoint + 1) % _enemy.patrolPointsParent.childCount;
        }
    }

    public void Exit() { }
}