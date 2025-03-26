using System.Collections;
using UnityEngine;
using BehaviorTree;

public class CheckNoEnemiesInScene : Node
{
    public override NodeState Evaluate()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0)
        {
            //Debug.Log("No enemies");
            state = NodeState.SUCCESS;
        }
        else
        {
            //Debug.Log("Enemies: " + enemies.Length);
            state = NodeState.FAILURE;
        }

        return state;
    }
}