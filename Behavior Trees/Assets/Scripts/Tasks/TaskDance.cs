using System.Collections;
using UnityEngine;
using BehaviorTree;

public class TaskDance : Node
{
    private Animator _animator;
    private bool _hasDanced = false;

    public TaskDance(Transform transform)
    {
        _animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        if (_hasDanced)
        {
            state = NodeState.SUCCESS;
            return state;
        }

        _animator.SetBool("Dancing", true);
        _animator.SetBool("Walking", false);
        _animator.SetBool("Attacking", false);

        _hasDanced = true;
        state = NodeState.RUNNING;
        return state;
    }

    public void OnTick(float deltaTime)
    {
        if (_hasDanced)
        {
            _animator.SetBool("Dancing", false);
            state = NodeState.SUCCESS;
        }
    }
}
