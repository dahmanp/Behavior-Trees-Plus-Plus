using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckIfBlockade : Node
{
    private Transform _transform;
    private float _range = 2f; // Adjust depending on the desired detection range

    public CheckIfBlockade(Transform transform)
    {
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        RaycastHit hit;
        // Cast a ray forward to check if there is a blockade within the range
        if (Physics.Raycast(_transform.position, _transform.forward, out hit, _range))
        {
            if (hit.collider.CompareTag("Blockade")) // Check if it's a blockade
            {
                state = NodeState.SUCCESS;
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
