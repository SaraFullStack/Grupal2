using UnityEngine;

public class AttackState : InterfaceEnemyStates
{
    private Enemy _enemy;

    public AttackState(Enemy enemy) => _enemy = enemy;

    public void Enter()
    {
        // Detenemos al enemigo mientras ataca
        if (_enemy.agent != null && _enemy.agent.isActiveAndEnabled)
        {
            _enemy.agent.isStopped = true;
            _enemy.agent.ResetPath();
        }

        /*if (_enemy.animator != null)
            _enemy.animator.SetBool("Walking", false);*/

        // Permite atacar nada más entrar (sin esperar el primer cooldown)
        //_enemy.lastAttackTime = -_enemy.attackCooldown;
    }

    public void Update()
    {
        if (_enemy.target == null) return;

        // Girar para mirar al jugador
        Vector3 dir = _enemy.target.position - _enemy.transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion look = Quaternion.LookRotation(dir);
            _enemy.transform.rotation = Quaternion.Slerp(
                _enemy.transform.rotation, look, Time.deltaTime * 2f);
        }
        // Solo ataca si el jugador está dentro del arco frontal
        // attackAngle = 180  ->  ±90° respecto al frente del enemigo
        float angleToPlayer = Vector3.Angle(_enemy.transform.forward, dir);
        if (angleToPlayer > _enemy.attackAngle / 2f)
            return;   // el jugador está fuera del arco -> no ataca



        // Atacar respetando el tiempo de recarga
        if (Time.time - _enemy.lastAttackTime >= _enemy.attackCooldown)
        {
            Debug.Log(Time.time - _enemy.lastAttackTime);
            _enemy.lastAttackTime = Time.time;
            Attack();
        }
    }

   

    private void Attack()
    {
        Debug.Log("Attacked");
        if (_enemy.animator != null)
            _enemy.animator.SetTrigger("Attack");

        // Hacemos daño al jugador a través de su componente IDamageable (Health)
        IDamageable damageable = _enemy.target.GetComponentInParent<IDamageable>();
        if (damageable != null)
            damageable.TakeDamage(_enemy.attackDamage);
    }

    public void Exit()
    {
        // Reanudamos el movimiento al salir del ataque
        if (_enemy.agent != null && _enemy.agent.isActiveAndEnabled)
            _enemy.agent.isStopped = false;
    }
}
