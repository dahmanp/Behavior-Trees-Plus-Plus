using System.Collections;
using UnityEngine;
using BehaviorTree;

public class TaskDestroyBlockade : Node
{
    private Transform _transform;
    private Animator _animator;
    private float _destroyTime = 2f;
    private float _timer = 0f;

    // Reference to the ParticleSystem
    private ParticleSystem _destroyParticleSystem;

    public TaskDestroyBlockade(Transform transform)
    {
        _transform = transform;
        _animator = transform.GetComponent<Animator>();
        _destroyParticleSystem = transform.Find("AttackParticles")?.GetComponent<ParticleSystem>();

        if (_destroyParticleSystem == null)
        {
            Debug.LogWarning("No ParticleSystem found under the given transform!");
        }
    }

    public override NodeState Evaluate()
    {
        RaycastHit hit;
        if (Physics.Raycast(_transform.position, _transform.forward, out hit, 2f))
        {
            if (hit.collider.CompareTag("Blockade"))
            {
                if (!_animator.GetBool("Attacking"))
                {
                    _animator.SetBool("Attacking", true);
                    _animator.SetBool("Walking", false);
                }

                if (_timer == 0f && _destroyParticleSystem != null)
                {
                    _destroyParticleSystem.Play();
                }

                _timer += Time.deltaTime;

                if (_timer >= _destroyTime)
                {
                    MonoBehaviour.Destroy(hit.collider.gameObject);
                    _animator.SetBool("Attacking", false);

                    if (_destroyParticleSystem != null)
                    {
                        _destroyParticleSystem.Stop();
                    }

                    state = NodeState.SUCCESS;
                }
                else
                {
                    state = NodeState.RUNNING;
                }
            }
            else
            {
                state = NodeState.FAILURE;
            }
        }
        else
        {
            state = NodeState.FAILURE;
        }

        return state;
    }
}
