using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class TaskAttack : Node
{
    private Animator _animator;
    private Transform _lastTarget;
    private EnemyManager _enemyManager;

    private float _attackTime = 1f;
    private float _attackCounter = 0f;

    private ParticleSystem _attackParticleSystem;

    public TaskAttack(Transform transform)
    {
        _animator = transform.GetComponent<Animator>();
        _attackParticleSystem = transform.Find("AttackParticles")?.GetComponent<ParticleSystem>();

        if (_attackParticleSystem == null)
        {
            Debug.LogWarning("No ParticleSystem found under the given transform!");
        }
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if (target != _lastTarget)
        {
            _enemyManager = target.GetComponent<EnemyManager>();
            _lastTarget = target;
        }

        if (_attackCounter == 0f && _attackParticleSystem != null)
        {
            _attackParticleSystem.Play();
        }

        _attackCounter += Time.deltaTime;
        if (_attackCounter >= _attackTime)
        {
            bool enemyIsDead = _enemyManager.TakeHit();
            if (enemyIsDead)
            {
                ClearData("target");
                _animator.SetBool("Attacking", false);
                _animator.SetBool("Walking", true);

                if (_attackParticleSystem != null)
                {
                    _attackParticleSystem.Stop();
                }
            }
            else
            {
                _attackCounter = 0f;
            }
        }

        state = NodeState.RUNNING;
        return state;
    }
}
