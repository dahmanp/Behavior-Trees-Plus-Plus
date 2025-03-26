using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class TaskInspectBox : Node
{
    private Transform _transform;
    private Animator _animator;
    private GameObject _box;
    private float _inspectionTime = 2f;
    private float _timer = 0f;
    private bool _isInspecting = false;
    private Collider _boxCollider;

    public TaskInspectBox(Transform transform)
    {
        _transform = transform;
        _animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        if (_isInspecting)
        {
            _timer += Time.deltaTime;

            if (_timer >= _inspectionTime)
            {
                _animator.SetBool("Inspecting", false);
                _animator.SetBool("Walking", true);
                _isInspecting = false;

                if (_boxCollider != null)
                {
                    _boxCollider.enabled = false;
                }

                state = NodeState.SUCCESS;
                return state;
            }
        }

        Collider[] colliders = Physics.OverlapSphere(_transform.position, GuardBT.attackRange);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Box"))
            {
                _box = collider.gameObject;

                if (!_isInspecting)
                {
                    _isInspecting = true;
                    _timer = 0f;
                    _animator.SetBool("Inspecting", true);

                    _boxCollider = _box.GetComponent<Collider>();
                }

                MoveToBox();
                //_animator.SetBool("Walking", false);

                state = NodeState.RUNNING;
                return state;
            }
        }

        state = NodeState.FAILURE;
        return state;
    }

    private void MoveToBox()
    {
        if (_box != null)
        {
            Vector3 directionToBox = _box.transform.position - _transform.position;

            float distanceToStop = 1f;
            Vector3 stopPosition = _box.transform.position - directionToBox.normalized * distanceToStop;

            _transform.position = Vector3.MoveTowards(_transform.position, stopPosition, GuardBT.speed * Time.deltaTime);
        }
    }
}
