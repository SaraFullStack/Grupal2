using UnityEngine;

public class AttackState : InterfaceEnemyStates
{
    private Enemy _enemy;

    public AttackState(Enemy enemy) => _enemy = enemy;

    public void Enter()
    {
        if (_enemy.agent != null && _enemy.agent.isActiveAndEnabled)
        {
            _enemy.agent.isStopped = true;
            _enemy.agent.ResetPath();
        }
    }

    public void Update()
    {
        if (_enemy.target == null) return;
        Vector3 dir = _enemy.target.position - _enemy.transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion look = Quaternion.LookRotation(dir);
            _enemy.transform.rotation = Quaternion.Slerp(
                _enemy.transform.rotation, look, Time.deltaTime * 2f);
        }
        float angleToPlayer = Vector3.Angle(_enemy.transform.forward, dir);
        if (angleToPlayer > _enemy.attackAngle / 2f)
            return;   
        if (Time.time - _enemy.lastAttackTime >= _enemy.attackCooldown)
        {
            _enemy.lastAttackTime = Time.time;
            Attack();
        }
    }

   

    private void Attack()
    {
        if (_enemy.animator != null)
            _enemy.animator.SetTrigger("Attack");
        IDamageable damageable = _enemy.target.GetComponentInParent<IDamageable>();
        if (damageable != null)
            damageable.TakeDamage(_enemy.attackDamage);
    }

    public void Exit()
    {
        if (_enemy.agent != null && _enemy.agent.isActiveAndEnabled)
            _enemy.agent.isStopped = false;
    }
}
